using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tobii.EyeX.Framework;

//[AddComponentMenu("Tobii EyeX/Eye Position Data")]
[AddComponentMenu("Tobii EyeX/Gaze Point Data")]

public class Level1 : MonoBehaviour { 

	// eye stuff
	private IEyeXDataProvider<EyeXEyePosition> eyePositionDataProvider;
	private IEyeXDataProvider<EyeXGazePoint> gazePointDataProvider;
	private EyeXHost eyexHost;
	public EyeXEyePosition LastEyePosition { get; private set; }
	private RaycastHit gazeRaycastHit;

	// Blinking lights array
	private ArrayList blinkingLights;

	// audio stuff
	private AudioFilesLevel1 audioFiles;

	// other stuff
	private float elapsedTime;
	private bool openFirstDoorAnimationStarted;
	private bool wallTriggerStarted;
	private bool wallSoundPlayed;
	private bool wakeupSoundPlayed;
	private bool wallsVisible; // default is false, when user closed eyes for a certain amount of time it will be true

	private float timeTillWakeupSoundStarts;
	private float timeTillFirstDoorOpens;
	private float timeTillWallSoundStarts;
	private float timeEyesAreClosed; // when eyes are closed this will count up. when eyes are opened it will get reset
	private float blinkingLightCounter; // counts up, when reached a certain number it will be reset and a light will start to blink

	private int blinkingLightIndex; //stores the index of the current blinking light

	public float standardColorLerpTime;
	public float gazedColorLerpTime;
	private float currentLerpTime;
	private float currentGazedLerpTime;
	private bool currentLerpTimePaused;
	public Color standardPanelColor;
	public Color highlightedPanelColor;
	private Color gazeStartedPanelColor;
	public Color gazePanelColor;
	public Color activatedPanelColor;
	private float continuesPanelDisgazeCounter;
	private EyeXGazePoint previousGazePoint;

	public Color wallLightColor1;
	public Color wallLightColor2;
	public Color wallLightColor3;
	public Color wallLightColor4;
	private string lastGazedWallLightName;

	protected void OnEnable() {
		eyePositionDataProvider.Start();
		gazePointDataProvider.Start ();
	}

	void Awake() {
		eyexHost = EyeXHost.GetInstance();
		gazePointDataProvider = eyexHost.GetGazePointDataProvider (GazePointDataMode.LightlyFiltered);
		eyePositionDataProvider = eyexHost.GetEyePositionDataProvider();

		timeTillFirstDoorOpens = 0.0f; //9.0f;
		timeTillWallSoundStarts = 0.0f; //3.0f;
		timeTillWakeupSoundStarts = 0.0f; //2.0f;
		timeEyesAreClosed = 0.0f;
		blinkingLightCounter = 0.0f;
		
		wakeupSoundPlayed = false;
		openFirstDoorAnimationStarted = false;
		wallTriggerStarted = false;
		wallSoundPlayed = false;


		// load the blinking lights
		blinkingLights = new ArrayList ();
		blinkingLights.Add (GameObject.Find ("Panel_01"));
		blinkingLights.Add (GameObject.Find ("Panel_02"));
		blinkingLights.Add (GameObject.Find ("Panel_03"));
		
		// set a deafult name for lastGazedWallLightName
		this.lastGazedWallLightName = "";

		Debug.Log ("Awake: " + this.ToString());
	}

	protected void OnDisable() {
		eyePositionDataProvider.Stop();
		gazePointDataProvider.Stop ();
	}

	void Start() {
		audioFiles = GameObject.Find ("GM").GetComponent<AudioFilesLevel1> ();

		gazePointDataProvider.Start ();
		eyePositionDataProvider.Start ();

		this.previousGazePoint = this.gazePointDataProvider.Last;

		// disable the rendering of the invisible walls
		GameObject.Find ("Level_1_Obstacle_Wall_01").GetComponent<MeshRenderer>().enabled = false;
		GameObject.Find ("Level_1_Obstacle_Wall_02").GetComponent<MeshRenderer>().enabled = false;
		GameObject.Find ("Level_1_Obstacle_Wall_03").GetComponent<MeshRenderer>().enabled = false;
		GameObject.Find ("Level_1_Obstacle_Wall_04").GetComponent<MeshRenderer>().enabled = false;

		// set all wall lights to first color
		GameObject[] wallLights = GameObject.FindGameObjectsWithTag ("WallLight_Level1");
		foreach (GameObject wallLight in wallLights) {
			wallLight.GetComponentInChildren<MeshRenderer>().material.color = this.wallLightColor1;
		}

		Debug.Log ("Start");
	}

	void Update() {
		elapsedTime += Time.deltaTime;

		// play the wakeup sound
		/*if (timeTillWakeupSoundStarts < elapsedTime && wakeupSoundPlayed == false) {
			this.playWakeupSound();
		}*/

		// open the door if the time is right!
		if (elapsedTime > timeTillFirstDoorOpens && !openFirstDoorAnimationStarted) {
			this.openFirstDoor();
		}

		// If the wall trigger was triggered then start the countdown till the voice should start
		if (wallTriggerStarted) {
			this.startWallTrigger();
		}
		
		// after the firstWallAudioClip was played check if the player closes his eyes for a second
		if (wallSoundPlayed && !wallsVisible) {
			this.showInvisibleWallsOnEyeClose();
		}
		
		// if the walls are passed send out rays to check whether the player is looking at a "solar panel"
		//if (wallsVisible) {
			this.handlePanelRiddle();
		//}

		this.handleWallLights ();
	}





	// plays the wake-up sound at the beginning of the level
	private void playWakeupSound() {
		wakeupSoundPlayed = true;
		AudioManager.instance.queueAudioClip(audioFiles.wakeupAudioClip);
	}

	// opens the first door in the first room where you wake up
	private void openFirstDoor() {
		openFirstDoorAnimationStarted = true;
		GameObject.Find ("FirstDoor").GetComponent<Animator>().Play ("OpenDoor");
	}

	// when the first invisible wall will be touched this trigger will fire and the method will get called. it plays the "close your eyes" sound
	private void startWallTrigger() {
		timeTillWallSoundStarts -= Time.deltaTime;
		
		if (wallSoundPlayed == false && timeTillWallSoundStarts < 0) {
			// play Audio Sound
			wallSoundPlayed = true;
			AudioManager.instance.queueAudioClip(audioFiles.firstWallAudioClip);
		}
	}

	// shows the invisible walls when the user closed his eyes
	private void showInvisibleWallsOnEyeClose() {
		// check whether left and right eye are closed
		EyeXEyePosition lastEyePosition = eyePositionDataProvider.Last;
		
		//Debug.Log ("lastEyePosition: " + lastEyePosition);
		
		if (!lastEyePosition.RightEye.IsValid && !lastEyePosition.LeftEye.IsValid) {
			timeEyesAreClosed += Time.deltaTime;
		} else {
			timeEyesAreClosed = 0.0f;
		}
		
		if (timeEyesAreClosed > 1.0f) {
			// show the walls
			GameObject.Find ("Level_1_Obstacle_Wall_01").GetComponent<MeshRenderer>().enabled = true;
			GameObject.Find ("Level_1_Obstacle_Wall_02").GetComponent<MeshRenderer>().enabled = true;
			GameObject.Find ("Level_1_Obstacle_Wall_03").GetComponent<MeshRenderer>().enabled = true;
			GameObject.Find ("Level_1_Obstacle_Wall_04").GetComponent<MeshRenderer>().enabled = true;
			
			wallsVisible = true;
		}
	}

	private void handlePanelRiddle() {
		
		EyeXGazePoint gazePoint = gazePointDataProvider.Last;
		EyeXEyePosition eyePosition = this.eyePositionDataProvider.Last;
		
		// check if both eyes are opened, only then retreive the last gazePoint
		if (!gazePoint.Screen.Equals (this.previousGazePoint.Screen)) {
			Vector2 screenCoordinates = gazePoint.Screen;

			Ray gazeRay = Camera.main.ScreenPointToRay (new Vector3 (screenCoordinates.x, screenCoordinates.y, 0));
			Debug.DrawRay(gazeRay.origin, gazeRay.direction, Color.magenta, 120.0f);
			if (Physics.Raycast (gazeRay.origin, gazeRay.direction, out gazeRaycastHit, 40.0f)) {

				string gazedObject = gazeRaycastHit.collider.gameObject.name;

				if (gazedObject.Contains ("Panel_Colored")) {
					//Debug.Log ("Panel gazed");
				}
			}

		} else {
			this.continuesPanelDisgazeCounter += Time.deltaTime;
			Debug.Log ("equal locations");
		}
		
		this.previousGazePoint = gazePoint;
		
		if (this.continuesPanelDisgazeCounter > 0.3) {
			this.continuesPanelDisgazeCounter = 0.0f;
			this.currentLerpTimePaused = false;
			this.currentGazedLerpTime = 0.0f;
		}
		
		
		
		if (currentLerpTimePaused) {
			// if the current lerp time is paused this means the user is looking at the panel
			float lerpValue = this.currentGazedLerpTime / this.gazedColorLerpTime;
			//((GameObject) blinkingLights[this.blinkingLightIndex]).GetComponentInChildren<MeshRenderer>().material.color = Color.Lerp (this.gazeStartedPanelColor, this.gazePanelColor, lerpValue);
			// if the line above is in comment it is the version without lerping
			
		}/* else */ /* without else: version without lerping, with else: version with lerping when staring at the panel*/if (currentLerpTime < standardColorLerpTime / 2.0f) {
			// fade-in lerp animation
			float lerpValue = this.currentLerpTime / (this.standardColorLerpTime / 2.0f);
			((GameObject)blinkingLights [blinkingLightIndex]).GetComponentInChildren<MeshRenderer> ().material.color = Color.Lerp (this.standardPanelColor, this.highlightedPanelColor, lerpValue);
			
		} else {
			//Debug.Log("lerp out");
			((GameObject)blinkingLights [blinkingLightIndex]).GetComponentInChildren<MeshRenderer> ().material.color = Color.Lerp (this.highlightedPanelColor, this.standardPanelColor, this.currentLerpTime / (this.standardColorLerpTime / 2.0f) - 1.0f);
		}
		
		
		
		// Version where you stare at the light without lerping the color while watching
		currentLerpTime += Time.deltaTime;
		if (currentLerpTime > standardColorLerpTime) {
			currentLerpTime = 0.0f;
			blinkLight ();
		}
		
		if (currentLerpTimePaused) {
			// lerp time paused, use the gazedLerpTime
			this.currentGazedLerpTime += Time.deltaTime;
			if (this.currentGazedLerpTime > gazedColorLerpTime) {
				// call the method to delete the object from the array and mark it as done!
				this.activatePanel();
			}
		}
		
		
		/*
			// version where you stare at the light and it lerps
			if (!currentLerpTimePaused) {
				currentLerpTime += Time.deltaTime;
				if (currentLerpTime > standardColorLerpTime) {
					currentLerpTime = 0.0f;
					blinkLight ();
				}

			} else {
				// lerp time paused, use the gazedLerpTime
				this.currentGazedLerpTime += Time.deltaTime;
				if (this.currentGazedLerpTime > gazedColorLerpTime) {
					// call the method to delete the object from the array and mark it as done!
					this.activatePanel();
				}
			}*/
	}


	private void handleWallLights() {
		EyeXGazePoint gazePoint = gazePointDataProvider.Last;
		Vector2 screenCoordinates = gazePoint.Screen;
			
		Ray gazeRay = Camera.main.ScreenPointToRay (new Vector3 (screenCoordinates.x, screenCoordinates.y, 0));
		Debug.DrawRay (gazeRay.origin, gazeRay.direction, Color.magenta, 40.0f);
			
		if (Physics.Raycast (gazeRay.origin, gazeRay.direction, out gazeRaycastHit, 40.0f)) {
			// get the gazed object
			GameObject gazedObject = gazeRaycastHit.collider.gameObject;

			if (gazedObject.name.Contains ("Panel") && !gazedObject.name.Equals (this.lastGazedWallLightName)) {
				// now check which color it has, set it to another color
				Color gazedObjectColor = gazedObject.GetComponentInChildren<MeshRenderer>().material.color;

				if (gazedObjectColor.Equals(this.wallLightColor1)) {
					gazedObject.GetComponentInChildren<MeshRenderer>().material.color = this.wallLightColor2;

				}/* else if (gazedObjectColor.Equals (this.wallLightColor2)) {
					gazedObject.GetComponentInChildren<MeshRenderer>().material.color = this.wallLightColor3;

				}  else if (gazedObjectColor.Equals (this.wallLightColor3)) {
					gazedObject.GetComponentInChildren<MeshRenderer>().material.color = this.wallLightColor4;

				}  else if (gazedObjectColor.Equals (this.wallLightColor4)) {
					gazedObject.GetComponentInChildren<MeshRenderer>().material.color = this.wallLightColor1;
				}*/
			}

			this.lastGazedWallLightName = gazedObject.name;
		}
	}

	// picks randomly a new panel to start blinking
	private void blinkLight() {
		// if there are still lights left then find randomly a new one that should blink
		if (this.blinkingLights.Count != 0) {
			blinkingLightIndex = Random.Range (0, blinkingLights.Count);
		}
	}

	private void activatePanel() {
		((GameObject)blinkingLights [this.blinkingLightIndex]).GetComponentInChildren<MeshRenderer> ().material.color = this.activatedPanelColor;
		this.currentLerpTime = 0.0f;
		this.currentGazedLerpTime = 0.0f;
		this.currentLerpTimePaused = false;
		this.blinkingLights.RemoveAt (this.blinkingLightIndex);
		blinkLight ();
		
		// if it was the last activated panel open the door and play special sound
		if (this.blinkingLights.Count == 0) {
			this.openGoalDoor ();
			AudioManager.instance.playAudioClipForced(this.audioFiles.lastActivated);
		} else {
			// it wasn't the last one, play normal activate sound
			AudioManager.instance.playAudioClipForced (this.audioFiles.activated);
		}
	}

	private void openGoalDoor() {
		GameObject.Find ("GoalDoor").GetComponent<Animator> ().Play ("Open");
	}

	public void setWallTriggerStarted(bool didStart) {
		wallTriggerStarted = didStart;
	}
}