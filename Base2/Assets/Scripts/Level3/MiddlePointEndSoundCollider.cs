using UnityEngine;
using System.Collections;

public class MiddlePointEndSoundCollider : MonoBehaviour
{
	public float eyesClosedDuration = 0.5f;

	private AudioFilesLevel3 audioFiles;
	private EyePosition_L3 eyePosition;
	private BoxCollider[] box;
	private MeshCollider[] mesh;
	private GameObject middlePointDeathCollider;
	private bool shouldBeActive = false;
	
	public void Start() {
		this.audioFiles = GameObject.Find ("GM").GetComponent<AudioFilesLevel3> ();
		this.eyePosition = GameObject.Find ("GM").GetComponent<EyePosition_L3> ();

		box = GameObject.Find ("Plattform 1").GetComponentsInChildren<BoxCollider>();
		mesh = GameObject.Find ("Plattform 1").GetComponentsInChildren<MeshCollider>();

		this.middlePointDeathCollider = GameObject.Find ("MiddleDeathSoundCollider");
		

		// disable first platform
		for (int i = 0; i < box.Length; i++) {
			box[i].enabled = false;
			mesh[i].enabled = false;
		}
	}
	
	protected void OnTriggerStay() {
		if (eyePosition.getEyesClosedDuration() > eyesClosedDuration) {
			if (!this.shouldBeActive) {
				return;
			}

			AudioManager.instance.queueAudioClip (this.audioFiles.MitteAugenkraft);

			// enable first platform
			for (int i = 0; i < box.Length; i++) {
				box[i].enabled = true;
				mesh[i].enabled = true;
			}

			// disable 1 eye death notice
			middlePointDeathCollider.GetComponent<BoxCollider>().enabled = true;

			this.shouldBeActive = false;
		}
	}

	public void setShouldBeActive(bool shouldBeActive) {
		this.shouldBeActive = shouldBeActive;
	}
}

