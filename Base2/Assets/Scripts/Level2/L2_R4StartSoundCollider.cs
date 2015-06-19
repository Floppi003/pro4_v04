using UnityEngine;
using System.Collections;

public class L2_R4StartSoundCollider : MonoBehaviour
{
	private AudioFilesLevel2 audioFiles;
	private bool didPlay = false;
	
	// Use this for initialization
	void Start () {
		this.audioFiles = GameObject.Find ("GM").GetComponent<AudioFilesLevel2> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	
	void OnTriggerEnter() {
		if (!didPlay) {
			didPlay = true;
			AudioManager.instance.playAudioClipForced (this.audioFiles.R4_MengeSchalter);
		}
	}
	
	void OnTriggerStay() {}
	
	void OnTriggerExit() {}
}

