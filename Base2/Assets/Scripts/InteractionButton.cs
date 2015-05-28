using UnityEngine;
using System.Collections;

public class InteractionButton : MonoBehaviour {
	
	private string loadPrompt;
	private bool pressButton;
	public GameObject exploder;
	public GameObject goalDoor;
	public bool pushPop;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	public void Update () {
	}

	public void ButtonPressed() {
		if (Input.GetButtonDown ("ButtonPressed")) {
			if(pushPop){
				transform.gameObject.GetComponentInParent<Animator> ().Play ("Push");
			}else{
				transform.gameObject.GetComponentInParent<Animator> ().Play ("PushStay");
			}
		}
	}

	public void TriggerExploder() {
		if (exploder != null) {
			exploder.GetComponent<Explosion>().Explode();
		}
	}

	public void TriggerGoal() {
		if (goalDoor != null) {
			goalDoor.GetComponent<Animator> ().Play ("Open");
		}
	}
}
