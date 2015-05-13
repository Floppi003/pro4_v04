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

	protected void OnEnable() {
		eyePositionDataProvider.Start();
		gazePointDataProvider.Start ();
	}

	void Awake() {
		eyexHost = EyeXHost.GetInstance();
		gazePointDataProvider = eyexHost.GetGazePointDataProvider (GazePointDataMode.Unfiltered);
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

		currentLerpTime = 0.0f;
		currentLerpTimePaused = false;
		

		// load the blinking lights
		blinkingLights = new ArrayList ();
		blinkingLights.Add (GameObject.Find ("Panel_01"));
		blinkingLights.Add (GameObject.Find ("Panel_02"));
		blinkingLights.Add (GameObject.Find ("Panel_03"));

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

		// disable the rendering of the invisible walls
		GameObject.Find ("Level_1_Obstacle_Wall_01").GetComponent<MeshRenderer>().enabled = false;
		GameObject.Find ("Level_1_Obstacle_Wall_02").GetComponent<MeshRenderer>().enabled = false;
		GameObject.Find ("Level_1_Obstacle_Wall_03").GetComponent<MeshRenderer>().enabled = false;
		GameObject.Find ("Level_1_Obstacle_Wall_04").GetComponent<MeshRenderer>().enabled = false;

		Debug.Log ("Start");
	}

	void Update() {
		elapsedTime += Time.deltaTime;


		// play the wakeup sound
		if (timeTillWakeupSoundStarts < elapsedTime && wakeupSoundPlayed == false) {
			wakeupSoundPlayed = true;

			//Debug.Log ("Wakeup sound queued");
			AudioManager.instance.queueAudioClip(audioFiles.wakeupAudioClip);
		}


		// If the wall trigger was triggered then start the countdown till the voice should start
		if (wallTriggerStarted) {
			timeTillWallSoundStarts -= Time.deltaTime;

			if (wallSoundPlayed == false && timeTillWallSoundStarts < 0) {
				// play Audio Sound
				wallSoundPlayed = true;
				AudioManager.instance.queueAudioClip(audioFiles.firstWallAudioClip);
			}
		}


		// open the door if the time is right!
		if (elapsedTime > timeTillFirstDoorOpens && !openFirstDoorAnimationStarted) {
			openFirstDoorAnimationStarted = true;
			GameObject.Find ("FirstDoor").GetComponent<Animator>().Play ("OpenDoor");
		}


		// after the firstWallAudioClip was played check if the player closes his eyes for a second
		if (wallSoundPlayed && !wallsVisible) {

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


		/*
		// if the walls are passed send out rays to check whether the player is looking at a "solar panel"
		//if (wallsVisible) {

		EyeXGazePoint gazePoint = gazePointDataProvider.Last;
		EyeXEyePosition eyePosition = this.eyePositionDataProvider.Last;

		// check if both eyes are opened, only then retreive the last gazePoint
		if (eyePosition.LeftEye.IsValid && eyePosition.RightEye.IsValid) {
			Vector2 screenCoordinates = gazePoint.Screen;
			
			Ray gazeRay = Camera.main.ScreenPointToRay (new Vector3 (screenCoordinates.x, screenCoordinates.y, 0));
			Debug.DrawRay (gazeRay.origin, gazeRay.direction, Color.magenta, 120.0f);
			
			if (Physics.Raycast (gazeRay.origin, gazeRay.direction, out gazeRaycastHit, 40.0f)) {
				
				string gazedObject = gazeRaycastHit.collider.gameObject.name;
				// check if the gazed panel is the one that is currently lerping
				if (gazedObject == ((GameObject)blinkingLights [blinkingLightIndex]).name) {
					// the gazed object is the one that is currently lerping
					if (currentLerpTimePaused == false) {
						currentLerpTimePaused = true;
						gazeStartedPanelColor = ((GameObject)blinkingLights [blinkingLightIndex]).GetComponentInChildren<MeshRenderer> ().material.color;
						this.continuesPanelDisgazeCounter = 0.0f;
					}
				} else {
					this.continuesPanelDisgazeCounter += Time.deltaTime;
				}
			} else {
				Debug.Log ("else from raycast if");
				this.continuesPanelDisgazeCounter += Time.deltaTime;
			}
		} else {
			this.continuesPanelDisgazeCounter += Time.deltaTime;
			Debug.Log ("not valid");
		}
*/


		if (this.continuesPanelDisgazeCounter > 0.5    ) {
			this.currentLerpTimePaused = false;
			this.currentGazedLerpTime = 0.0f;
		}
			
		//}


		if (currentLerpTimePaused) {
			// if the current lerp time is paused this means the user is looking at the panel
			float lerpValue = this.currentGazedLerpTime / this.gazedColorLerpTime;
			((GameObject) blinkingLights[this.blinkingLightIndex]).GetComponentInChildren<MeshRenderer>().material.color = Color.Lerp (this.gazeStartedPanelColor, this.gazePanelColor, lerpValue);

		} else if (currentLerpTime < standardColorLerpTime / 2.0f) {
			// fade-in lerp animation
			float lerpValue = this.currentLerpTime / (this.standardColorLerpTime / 2.0f);
			((GameObject)blinkingLights [blinkingLightIndex]).GetComponentInChildren<MeshRenderer> ().material.color = Color.Lerp (this.standardPanelColor, this.highlightedPanelColor, lerpValue);
		
		} else {
			//Debug.Log("lerp out");
			((GameObject)blinkingLights [blinkingLightIndex]).GetComponentInChildren<MeshRenderer> ().material.color = Color.Lerp (this.highlightedPanelColor, this.standardPanelColor, this.currentLerpTime / (this.standardColorLerpTime / 2.0f) - 1.0f);
		}

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
		}
		// set the current blinking light to the correct color via lerp
	}

	void blinkLight() {
		blinkingLightIndex = Random.Range (0, blinkingLights.Count);
		Debug.Log ("index: " + blinkingLightIndex);
	}

	void activatePanel() {
		((GameObject)blinkingLights [this.blinkingLightIndex]).GetComponentInChildren<MeshRenderer> ().material.color = this.activatedPanelColor;
		this.currentLerpTime = 0.0f;
		this.currentGazedLerpTime = 0.0f;
		this.currentLerpTimePaused = false;
		this.blinkingLights.RemoveAt (this.blinkingLightIndex);
		blinkLight ();
	}

	public void setWallTriggerStarted(bool didStart) {
		wallTriggerStarted = didStart;
	}
}