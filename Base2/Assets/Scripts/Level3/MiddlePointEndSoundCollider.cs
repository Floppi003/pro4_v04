using UnityEngine;
using System.Collections;

public class MiddlePointEndSoundCollider : MonoBehaviour
{
	public float eyesClosedDuration = 2;

	private AudioFilesLevel3 audioFiles;
	private EyePosition_L3 eyePosition;
	
	public void Start() {
		this.audioFiles = GameObject.Find ("GM").GetComponent<AudioFilesLevel3> ();
		this.eyePosition = GameObject.Find ("GM").GetComponent<EyePosition_L3> ();
	}
	
	protected void OnTriggerStay() {
		if (eyePosition.getEyesClosedDuration() > eyesClosedDuration) {
			AudioManager.instance.playAudioClipForced (this.audioFiles.MitteAugenkraft);
			this.gameObject.GetComponent<BoxCollider>().enabled = false;
		}
	}
}

