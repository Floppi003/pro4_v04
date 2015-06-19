using UnityEngine;
using System.Collections;

public class AudioButtonManager : MonoBehaviour {

	public Material[] colorCode; 
	
	private AudioFilesLevel2 audioFiles;

	private int counter;
	private bool open;

	// Use this for initialization
	void Start () {
		counter = 0;
		open = false;
		audioFiles = GameObject.Find ("GM").GetComponent<AudioFilesLevel2>();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void pushButton(Material mat) {
		// add pushed button to counter
		if (open) {return;}

		if (colorCode[counter] == mat) {
			counter++;
			if (counter == colorCode.Length) {
				this.GetComponent<Door>().move = true;
				open = true;
				//TODO: change audio
				AudioManager.instance.playSoundEffect (audioFiles.firstRiddleSuccessSound);
				AudioManager.instance.playAudioClipForced (audioFiles.getAudioClipR3_Geschafft());
			}
		} else {
			counter = 0;
			//TODO: change audio
			AudioManager.instance.playSoundEffect (audioFiles.buttonPressedSound);
			// TODO: Play Error Sound
		}
	}

	public void hitButton(Material mat) {		
		// play audio sound
		AudioManager.instance.playSoundEffect (audioFiles.buttonGazedSound);
	}
}
