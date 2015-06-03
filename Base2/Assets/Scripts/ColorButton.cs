﻿using UnityEngine;
using System.Collections;

public class ColorButton : MonoBehaviour {

	public Material activeMaterial;
	public float lifetime = 1; // 0 = infinity
	public GameObject door;

	private Material oldMaterial;
	private Transform buttonFrame;
	private Transform buttonCenter;

	private float alive; 
	private bool pushed;

	// Use this for initialization
	void Start () {
		alive = 0;
		pushed = false;
		buttonFrame = this.transform.GetChild (1);
		buttonCenter = this.transform.GetChild (2);
		door = GameObject.Find ("Door ColorButtons");
		//defaultMaterial = this.transform.GetChild (1).GetChild(0).GetComponent<Renderer> ().material;
	}
	
	// Update is called once per frame
	void Update () {
		if (alive > 0) {
			alive -= Time.deltaTime;
			if (alive <= 0) {	
				buttonFrame.GetComponent<Renderer> ().material = oldMaterial;
				buttonCenter.GetComponent<Renderer> ().material = oldMaterial;
				oldMaterial = null;
			}
		}
	}

	private void changeColor(Transform obj) {

		oldMaterial = obj.GetComponent<Renderer> ().material;
			
		obj.GetComponent<Renderer> ().material = activeMaterial;
	}

	public void push() {
		if (!pushed) {
			// animate rein
			this.GetComponent<Animator> ().Play ("PushStay");

			pushed = true;
			alive = 0; 
			if (oldMaterial == null) {
				changeColor (buttonFrame);
			}
			changeColor (buttonCenter);

			door.GetComponent<ColorButtonManager>().pushButton(activeMaterial);
		} else {
			// animate raus
			this.GetComponent<Animator> ().Play ("Pop");
			door.GetComponent<ColorButtonManager>().popButton(activeMaterial);

			pushed = false;
			alive = 0.001f;
		}
	}

	public void hit(Collider col) {
		if (!pushed) {
			alive = lifetime;
			if (oldMaterial == null) {
				changeColor (buttonFrame);
			}
		}
	}
}
