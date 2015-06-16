﻿using UnityEngine;
using System.Collections;

public class AudioButton : MonoBehaviour {
	
	public Material activeMaterial;
	public Material highlightMaterial;
	public float lifetime = 1; // 0 = infinity
	public GameObject door;
	
	private Material oldMaterial;
	private Transform buttonFrame;
	private Transform buttonCenter;

	private float active;
	
	// Use this for initialization
	void Start () {
		active = 0;
		buttonFrame = this.transform.GetChild (1);
		buttonCenter = this.transform.GetChild (2);
		//door = GameObject.Find ("Door AudioButtons");
	}
	
	// Update is called once per frame
	void Update () {
		if (active > 0) {
			active -= Time.deltaTime;
			if (active <= 0) {
				buttonFrame.GetComponent<Renderer>().material = oldMaterial;
				oldMaterial = null;
			}
		}
	}
	
	private void changeColor(Transform obj) {
		oldMaterial = obj.GetComponent<Renderer> ().material;
		
		obj.GetComponent<Renderer> ().material = highlightMaterial;
	}
	
	public void push() {
		// animate rein und wieder raus
		this.GetComponent<Animator> ().Play ("Push");
		// send to AudioButtonManager
		door.GetComponent<AudioButtonManager>().pushButton(activeMaterial);
	}
	
	public void hit(Collider col) {
		if (active <= 0) {
			if (oldMaterial == null) {
				door.GetComponent<AudioButtonManager>().hitButton(activeMaterial);
				
				changeColor (buttonFrame);
				active = 0.5f;
			}
		}
	}
}