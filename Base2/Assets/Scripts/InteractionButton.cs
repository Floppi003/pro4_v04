using UnityEngine;
using System.Collections;

public class InteractionButton : MonoBehaviour {
	
	private string loadPrompt;
	private bool pressButton;
	public GameObject exploder;
	public GameObject goalDoor;
	public GameObject platform;
	public GameObject wallDoor;
	public bool pushPop;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	public void Update () {
	}

	public void ButtonPressed() {
		if (Input.GetButtonDown ("ButtonPressed")) {
			if(pushPop){
				// load audio files level 2
				AudioFilesLevel2 audioFiles = GameObject.Find ("GM").GetComponent<AudioFilesLevel2>();
				AudioManager.instance.playSoundEffect(audioFiles.platformMovingSound);
				transform.gameObject.GetComponentInParent<Animator> ().Play ("Push");
			}else{
				transform.gameObject.GetComponentInParent<Animator> ().Play ("PushStay");
			}
		}
	}

	public void TriggerExploder() {
		if (exploder != null) {
			exploder.GetComponent<Explosion>().Explode();
		}
	}

	public void TriggerGoal() {
		if (goalDoor != null) {
			goalDoor.GetComponent<Animator> ().Play ("Open");
		}
	}

	public void TriggerPlatform() {
		if (platform != null) {
			platform.GetComponent<Patrol>().NextPatrolPoint();
		}
	}

	public void TriggerDoor() {
		if (wallDoor != null) {
			//wallDoor.transform.localScale.x += 5;
			Debug.Log ("OPEN the door!");
			wallDoor.GetComponent<Door>().move = true;

			// play sound effect
			AudioFilesLevel2 audioFiles = GameObject.Find ("GM").GetComponent<AudioFilesLevel2>();
			AudioManager.instance.playSoundEffect(audioFiles.firstRiddleSuccessSound);
		}
	}
}