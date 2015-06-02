using UnityEngine;
using System.Collections;

public class ColorButton : MonoBehaviour {

	public Material activeMaterial;
	public float lifetime = 1; // 0 = infinity

	private Material oldMaterial;
	private Material defaultMaterial;
	private Transform buttonFrame;

	private float alive; 

	// Use this for initialization
	void Start () {
		alive = 0;
		buttonFrame = this.transform.GetChild (1).GetChild (0);
		//defaultMaterial = this.transform.GetChild (1).GetChild(0).GetComponent<Renderer> ().material;
	}
	
	// Update is called once per frame
	void Update () {
		if (alive > 0) {
			alive -= Time.deltaTime;
			if (alive <= 0) {	
				buttonFrame.GetComponent<Renderer> ().material = oldMaterial;
				oldMaterial = null;
			}
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.name == "ColorSphere") {
			alive = lifetime;
			if (oldMaterial == null) {
				oldMaterial = buttonFrame.GetComponent<Renderer> ().material;

				buttonFrame.GetComponent<Renderer> ().material = activeMaterial;
			}
		}
	}

	void OnTriggerStay(Collider col) {
		if (col.name == "ColorSphere") {
			alive = lifetime;
		}
	}
}
