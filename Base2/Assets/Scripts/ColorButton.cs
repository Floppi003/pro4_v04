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
		}
	}

	void OnTriggerEnter(Collider col) {
		alive = 1;
		Debug.Log ("drin");
	}

	void OnTriggerStay(Collider col) {
		Debug.Log ("jetzt");
		/*if (col.tag == "RedButton") {
			col.GetComponent<Renderer> ().material = selectedMaterial;
		} else if (col.tag == "GreenButton") {
			
		} else if (col.tag == "BlueButton") {

		}*/
	}

	void OnTriggerExit(Collider col) {
		if (col.tag == "RedButton") {
			col.GetComponent<Renderer> ().material = defaultMaterial;
			
			col.transform.parent.gameObject.GetComponent<Animator> ().Play("ButtonPress");
		} else if (col.tag == "GreenButton") {
			
		} else if (col.tag == "BlueButton") {
			
		}
	}
}
