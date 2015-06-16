using UnityEngine;
using System.Collections;

public class AudioColorButtonManager : MonoBehaviour {
	private int counterPressed;
	private int counterHit;
	private float timeToChange = -1;

	// Use this for initialization
	void Start () {
		counterPressed = 0;
		counterHit = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (timeToChange > 0) {
			timeToChange -= Time.deltaTime;
			if (timeToChange <= 0) {

			}
		}
	}

	public void pushButton(Material mat, AudioColorButton obj) {
		counterPressed++;
	}

	public void hitButton(Material mat, AudioColorButton obj) {
		counterHit++;
		Debug.LogError (counterHit);

		if (counterHit >= 2) {
			timeToChange = 5;
			obj.resetColor();
			obj.gazePoint.GetComponent<GazePointDataComponent>().active = true;
			
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
}
