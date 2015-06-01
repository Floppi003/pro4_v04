using UnityEngine;
using System.Collections;

public class LevelLoaderNew : MonoBehaviour {
	public int levelToLoad;
	public GameObject padlock;
	private string loadPrompt;
	//private bool inRange;
	private int unlockedLevel = 1;
	private bool canLoadLevel = true;

	void Start()
	{
		unlockedLevel = PlayerPrefs.GetInt ("Level Unlocked");
		PlayerPrefs.SetInt("Chosen Level", unlockedLevel);

		if (unlockedLevel == 0) 
		{
			unlockedLevel = 1;
		}

		canLoadLevel = levelToLoad <= unlockedLevel ? true : false;
		if (canLoadLevel) {
			//Instantiate (padlock, new Vector3(transform.position.x - 0.7f, 1.5f, transform.position.z), Quaternion.identity);
			MeshRenderer rendererLock = transform.Find ("pCube1").Find ("LevelLock").GetComponent<MeshRenderer> ();
			rendererLock.enabled = false;
			MeshRenderer rendererWhite = transform.Find ("pCube1").Find ("LevelWhite").GetComponent<MeshRenderer> ();
			rendererWhite.enabled = false;
		} else {
			MeshRenderer rendererNumber = transform.Find ("pCube1").Find ("LevelNumber").GetComponent<MeshRenderer> ();
			rendererNumber.enabled = false;
		}
	}

	public void LevelButtonPressed()
	{
		if (canLoadLevel && Input.GetButtonDown ("Interaction")) {
			if (levelToLoad.ToString () == "0") {	//Reset Game
				PlayerPrefs.SetInt("Level Unlocked", 1);
				PlayerPrefs.SetInt ("Current Level", 1);
				Application.LoadLevel("Level 1");
			} else {
				PlayerPrefs.SetInt ("Current Level", levelToLoad);
				PlayerPrefs.SetInt("Chosen Level", levelToLoad);
		//		Application.LoadLevel ("Level " + levelToLoad.ToString ()); //load by name instead of id
			}
			playAnimation();
		}
	}

	public string GetText(){
		string levelText = "";
		if (levelToLoad.ToString () == "0") {
			levelText = "Press [F] to clear the game";
		}else if (canLoadLevel) {
			levelText = "Press [F] to load level " + levelToLoad.ToString ();
		} else {
			levelText = "Level " + levelToLoad.ToString () + " is locked";
		}
		return levelText;
	}

	void playAnimation() {
		GetComponent<Animator> ().Play("Push");
	}
}