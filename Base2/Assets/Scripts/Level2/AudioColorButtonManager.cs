using UnityEngine;
using System.Collections;

public class AudioColorButtonManager : MonoBehaviour {
	private int counterPressed;
	private int counterHit;

	// Use this for initialization
	void Start () {
		counterPressed = 0;
		counterHit = 0;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void pushButton(Material mat, AudioColorButton obj) {
		counterPressed++;
		// TODO: move to hit button
		if (counterPressed >= 5) {
			//obj.resetColor();
			AudioColorButton[] audioButtons = GameObject.Find("ButtonWall").GetComponentsInChildren<AudioColorButton>();
			ColorButton[] colorButtons = GameObject.Find("ButtonWall").GetComponentsInChildren<ColorButton>();
			GameObject buttons = GameObject.Find ("ButtonWall");
			for (int i = 0; i < audioButtons.Length; i++) {

				audioButtons[i].enabled = false;
				colorButtons[i].enabled = true;
				buttons.transform.GetChild(i).tag = "ColorButton";
			}
		}
	}

	public void hitButton(Material mat) {
		counterHit++;
	}
}
