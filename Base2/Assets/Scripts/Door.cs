using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	float minimum = 0.75F;
	float maximum = 0.0F;
	public bool move = false;
	float time = 0;
	float duration = 3;

	void Update() {
		if (move) {
			if (time <= 1) {
				time += Time.deltaTime/duration;
			}
			transform.localScale = new Vector3 (transform.localScale.x, Mathf.Lerp (minimum, maximum, time), transform.localScale.z);
		}
	}
	
	void Start () {
	
	}
}