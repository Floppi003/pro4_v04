using UnityEngine;
using System.Collections;

public class AudioColorButton : MonoBehaviour {
	
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
				resetColor ();
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
		door.GetComponent<AudioColorButtonManager>().pushButton(activeMaterial, this);
	}
	
	public void hit(Collider col) {
		active += 2 * Time.deltaTime;

		if (active <= 1) {
			if (oldMaterial == null) {
				door.GetComponent<AudioColorButtonManager>().hitButton(activeMaterial);
				
				changeColor (buttonFrame);
				active = 0.5f;
			}
		}
	}

	public void resetColor() {
		if (oldMaterial != null) {
			buttonFrame.GetComponent<Renderer> ().material = oldMaterial;
			oldMaterial = null;
		}
	}
}
