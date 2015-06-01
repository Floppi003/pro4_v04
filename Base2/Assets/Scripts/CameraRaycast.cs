﻿using UnityEngine;
using System.Collections;

public class CameraRaycast : MonoBehaviour {

	public Camera cam;
	private RaycastHit interactionRaycastHit;
	private string loadPrompt;
	private bool showHint = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawRay (cam.transform.position, cam.gameObject.transform.forward, Color.cyan, 3);
		if (Physics.Raycast (cam.transform.position, cam.gameObject.transform.forward, out interactionRaycastHit, 3)) {
		
			if (interactionRaycastHit.collider.gameObject.tag == "Interaction") {
				loadPrompt = "Press [F] to interact";
				showHint = true;
				interactionRaycastHit.collider.gameObject.GetComponent<InteractionButton>().ButtonPressed();

				if(Input.GetButtonDown ("ButtonPressed")) {
					if (interactionRaycastHit.collider.gameObject.transform.parent.name == "GoalButton") {
						interactionRaycastHit.collider.gameObject.GetComponent<InteractionButton>().TriggerGoal();
					}else if (interactionRaycastHit.collider.gameObject.transform.parent.name == "PlatformButton") {
						interactionRaycastHit.collider.gameObject.GetComponent<InteractionButton>().TriggerPlatform();
					}else if (interactionRaycastHit.collider.gameObject.transform.parent.name == "DoorButton") {
						interactionRaycastHit.collider.gameObject.GetComponent<InteractionButton>().TriggerDoor();
					}
				}

			} else if (interactionRaycastHit.collider.gameObject.tag == "LevelButton"){
				loadPrompt = interactionRaycastHit.collider.gameObject.GetComponent<LevelLoaderNew>().GetText ();
				showHint = true;
				if(Input.GetButtonDown ("ButtonPressed")) {
					interactionRaycastHit.collider.gameObject.GetComponent<LevelLoaderNew>().LevelButtonPressed();
				}

			} else if (interactionRaycastHit.collider.gameObject.tag == "Exploder"){
				loadPrompt = "Press [F] for explosions";
				showHint = true;
				interactionRaycastHit.collider.gameObject.GetComponent<Explosion>().Explode();
		
			}else {
				loadPrompt = "";
				showHint = false;
			}
	
		} else {
			loadPrompt = "";
			showHint = false;
		}
	}

	void OnGUI()
	{
		if (showHint) {			
			Rect interactionRect = new Rect(2*Screen.width/3, 2*Screen.height/3, Screen.width/2/2, Screen.height/6/2);
			GUI.Box(interactionRect, loadPrompt);
		}
	}
}
