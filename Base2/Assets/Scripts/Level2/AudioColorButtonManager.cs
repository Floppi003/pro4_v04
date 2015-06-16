﻿using UnityEngine;
using System.Collections;

public class AudioColorButtonManager : MonoBehaviour {
	public int tries = 2;
	public float delayTime = 5;

	private int counterPressed;
	private int counterHit;
	private float timeToChange = -1;
	
	private AudioColorButton[] audioButtons;
	private ColorButton[] colorButtons;
	private GameObject buttons;
	private AudioColorButton obj;
	
	// Use this for initialization
	void Start () {
		counterPressed = 0;
		counterHit = 0;
		
		buttons = GameObject.Find ("ButtonWall");
		audioButtons = buttons.GetComponentsInChildren<AudioColorButton>();
		colorButtons = buttons.GetComponentsInChildren<ColorButton>();
	}
	
	// Update is called once per frame
	void Update () {
		if (timeToChange > 0) {
			timeToChange -= Time.deltaTime;
			if (timeToChange <= 0) {
				// activate color functionality
				for (int i = 0; i < audioButtons.Length; i++) {
					colorButtons[i].enabled = true;
					buttons.transform.GetChild(i).tag = "ColorButton";
				}
				// enable highlighting
				obj.gazePoint.GetComponent<GazePointDataComponent>().active = true;
			}
		}
	}

	public void pushButton(Material mat, AudioColorButton obj) {
		counterPressed++;
	}

	public void hitButton(Material mat, AudioColorButton obj) {
		counterHit++;
		if (counterHit >= tries) {
			this.obj = obj;
			timeToChange = delayTime;

			// reset Color and lock highlighting
			obj.resetColor();
			obj.gazePoint.GetComponent<GazePointDataComponent>().active = false;

			// deactivate audio functionality
			for (int i = 0; i < audioButtons.Length; i++) {
				audioButtons[i].enabled = false;
			}
		}
	}
}