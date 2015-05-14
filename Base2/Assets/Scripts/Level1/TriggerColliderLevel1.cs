﻿using UnityEngine;
using System.Collections;

public class TriggerColliderLevel1 : MonoBehaviour {

	private GameManager gm;
	private AudioFilesLevel1 audioFiles;
	private Level1 level1;

	// Use this for initialization
	void Start () {
		this.gm = GameObject.Find ("GM").GetComponent<GameManager> ();
		this.audioFiles = GameObject.Find ("GM").GetComponent<AudioFilesLevel1> ();
		this.level1 = GameObject.Find ("GM").GetComponent<Level1> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	// Triggers
	void OnTriggerEnter(Collider other) {
		Debug.Log ("OnTriggerEnter");

		if (this.name.Equals ("WallCollider")) {
			// play wall sound
			AudioManager.instance.playSoundEffect(audioFiles.wallHit);

			// also change the value in level1 script
			this.level1.setWallTriggerStarted (true);

			// also call the method to push the player back
			this.level1.pushPlayerBack();

		} else if (this.name.Equals ("WallLightsCollider")) {
			level1.setPlayerInWallLightsArea (true);

		} else if (this.name.Equals ("PanelCollider")) {
			level1.setPlayerInPanelArea (true);
		} else if (this.name.Equals ("CowCollider")) {
			AudioManager.instance.playSoundEffect(audioFiles.cow);
		}
	}
	
	void OnTriggerStay(Collider other) {
		Debug.Log ("OnTriggerStay");
	}
	
	void OnTriggerExit(Collider other) {
		Debug.Log ("OnTriggerExit");

		if (this.name.Equals ("WallLightsCollider")) {
			level1.setPlayerInWallLightsArea(false);
		} else if (this.name.Equals ("PanelCollider")) {
			level1.setPlayerInPanelArea(false);
		}
	}
}
