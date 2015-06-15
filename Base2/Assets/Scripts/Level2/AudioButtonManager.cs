using UnityEngine;
using System.Collections;

public class AudioButtonManager : MonoBehaviour {

	public Material[] colorCode; 

	private int counter;
	private bool open;

	// Use this for initialization
	void Start () {
		counter = 0;
		open = false;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void pushButton(Material mat) {
		// add pushed button to counter
		if (!open && colorCode[counter] == mat) {
			counter++;
			Debug.Log (counter);
			if (counter == colorCode.Length - 1) {
				this.GetComponent<Door>().move = true;
				open = true;
			}
		} else {
			counter = 0;
			// TODO: Play Error Sound
		}
	}
}
