using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	float minimum = 0.75F;
	float maximum = 0.0F;
	public bool move = false;
	float time = 0;
	float duration = 3;

	private bool didPlayR4_ReiseGehtWeiter = false;

	void Update() {
		if (move) {
			if (time <= 1) {
				time += Time.deltaTime/duration;
			}
			transform.localScale = new Vector3 (transform.localScale.x, Mathf.Lerp (minimum, maximum, time), transform.localScale.z);

			if (this.name.Equals ("First Door")) {
				// tell the L2_R1SchalterSoundCollider Script that the player solved the riddle!
				GameObject.Find ("R1SchalterSoundCollider").GetComponent<L2_R1SchalterSoundCollider>().setDidSolveRiddle(true);
			}

			if (this.name.Equals ("Door ColorButtons") && !this.didPlayR4_ReiseGehtWeiter) {
				// it is the red door in room 4 (level 2)
				AudioFilesLevel2 audioFiles = GameObject.Find("GM").GetComponent<AudioFilesLevel2>();
				AudioManager.instance.playSoundEffect (audioFiles.firstRiddleSuccessSound);
				AudioManager.instance.queueAudioClip (audioFiles.R4_ReiseGehtWeiter, 3);
				this.didPlayR4_ReiseGehtWeiter = true;
			}
		}
	}
	
	void Start () {
	
	}
}