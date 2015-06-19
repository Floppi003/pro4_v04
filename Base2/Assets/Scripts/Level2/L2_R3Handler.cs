using UnityEngine;
using System.Collections;

public class L2_R3Handler : MonoBehaviour
{

	private AudioFilesLevel2 audioFiles;

	private int roomTwoStage = 0; // stage 0: stimme sagt noch keine button farbe
	// stage 1: stimme sagt dir die farbe an
	private int buttonsPressed = 0;

	// Use this for initialization
	void Start () {
		this.audioFiles = GameObject.Find ("GM").GetComponent<AudioFilesLevel2>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void didPressAudioButton() {
		this.buttonsPressed++;

		if (this.roomTwoStage == 0) {
			// if stage 0 then voice should say you should press the blue button
			if (this.buttonsPressed == 1) {
				AudioManager.instance.playAudioClipForced(this.audioFiles.R3_BlauerSchalter);
			}
			
			
			if (this.buttonsPressed >= 2) {
				AudioManager.instance.playAudioClipForced(this.audioFiles.R3_Planaenderung);
				this.roomTwoStage = 1;
			}
		}
	}
}

