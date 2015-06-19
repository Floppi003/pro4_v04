using UnityEngine;
using System.Collections;

public class MiddlePointDoneSoundCollider : MonoBehaviour
{
	private bool didPlay = false;
	private AudioFilesLevel3 audioFiles;
	private GameObject bridgeDeathCollider;
	private GameObject middlePointCollider;
	
	public void Start() {
		this.audioFiles = GameObject.Find ("GM").GetComponent<AudioFilesLevel3> ();
		bridgeDeathCollider = GameObject.Find ("BridgeDeathSoundCollider");
		middlePointCollider = GameObject.Find ("MiddlePointEndSoundCollider");
	}
	
	protected void OnTriggerEnter() {
		if (!didPlay) {
			AudioManager.instance.playAudioClipForced (this.audioFiles.BrueckeGeschafft);
			bridgeDeathCollider.GetComponent<BoxCollider>().enabled = false;
			middlePointCollider.GetComponent<BoxCollider>().enabled = true;
			didPlay = true;
		}
	}
}

