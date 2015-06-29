using UnityEngine;
using System.Collections;

public class JPDeathSoundCollider : MonoBehaviour
{
	private AudioFilesLevel3 audioFiles;
	
	public void Start() {
		this.audioFiles = GameObject.Find ("GM").GetComponent<AudioFilesLevel3> ();
	}
	
	protected void OnTriggerEnter() {
		AudioManager.instance.queueAudioClip (/*choose audioClip*/ null, 1);
		Debug.LogError ("DEAD");
	}
}