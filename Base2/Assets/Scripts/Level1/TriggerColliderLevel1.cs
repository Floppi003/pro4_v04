using UnityEngine;
using System.Collections;

public class TriggerColliderLevel1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	// Triggers
	void OnTriggerEnter(Collider other) {
		Debug.Log ("OnTriggerEnter");

		if (this.name.Equals ("WallCollider")) {
			GameObject.Find ("GM").GetComponent<Level1> ().setWallTriggerStarted (true);
		} else if (this.name.Equals ("WallLightsCollider")) {
			GameObject.Find ("GM").GetComponent<Level1> ().setPlayerInWallLightsArea(true);
		} else if (this.name.Equals ("PanelCollider")) {
			GameObject.Find ("GM").GetComponent<Level1>().setPlayerInPanelArea(true);
		}
	}
	
	void OnTriggerStay(Collider other) {
		Debug.Log ("OnTriggerStay");
	}
	
	void OnTriggerExit(Collider other) {
		Debug.Log ("OnTriggerExit");

		if (this.name.Equals ("WallLightsCollider")) {
			GameObject.Find ("GM").GetComponent<Level1> ().setPlayerInWallLightsArea(false);
		} else if (this.name.Equals ("PanelCollider")) {
			GameObject.Find ("GM").GetComponent<Level1>().setPlayerInPanelArea(false);
		}
	}
}
