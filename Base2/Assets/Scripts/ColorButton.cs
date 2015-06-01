using UnityEngine;
using System.Collections;

public class ColorButton : MonoBehaviour {

	public Material selectedMaterial;
	private Material oldMaterial;
	public Material defaultMaterial;

	private float alive; 

	// Use this for initialization
	void Start () {
		alive = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (alive > 0) {
			alive -= Time.deltaTime;
			if (alive <= 0) {	
				this.transform.GetChild (1).GetChild(0).GetComponent<Renderer> ().material = oldMaterial;
				oldMaterial = null;
			}
		}
	}

	void OnTriggerEnter(Collider col) {
		alive = 1;
		if (oldMaterial == null) {
			oldMaterial = this.transform.GetChild (1).GetChild (0).GetComponent<Renderer> ().material;

			this.transform.GetChild (1).GetChild (0).GetComponent<Renderer> ().material = selectedMaterial;
		}
	}

	void OnTriggerStay(Collider col) {
		alive = 1;
		/*if (col.tag == "RedButton") {
			col.GetComponent<Renderer> ().material = selectedMaterial;
		} else if (col.tag == "GreenButton") {
			
		} else if (col.tag == "BlueButton") {

		}*/
	}

	void OnTriggerExit(Collider col) {
		
		Debug.Log ("weg");
	}
}
