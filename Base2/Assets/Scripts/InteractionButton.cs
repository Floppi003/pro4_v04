using UnityEngine;
using System.Collections;

public class InteractionButton : MonoBehaviour {
	
	private string loadPrompt;
	private bool pressButton;
	public GameObject explo;
	GameObject[] exploder;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	public void Update () {
	}

	public void ButtonPressed() {
		if (Input.GetButtonDown ("ButtonPressed")) {
			transform.gameObject.GetComponentInParent<Animator> ().Play ("Push");
		}
	}

	public void TriggerExploder() {
		exploder = GameObject.FindGameObjectsWithTag ("Exploder");
		if (exploder.Length > 0) {
			explo.GetComponent<Explosion>().Explode();
		}
	}

	public void TriggerGoal() {
		Debug.Log ("Goal!!");
	}
}
