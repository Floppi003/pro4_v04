﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public Canvas pauseMenu;
	// Count
	private int maxLevels = 4; //amount of levels in game (!=max id = maxLevels - 1)
	private int currentLevel = 1; //start with 1 = id 0
	private int unlockedLevel = 1; //start with 1 = id 0
	
	// Timer variables
	//public Rect timerRect;
	//public Color warningColorTimer;
	//public Color defaultColorTimer;
	private string currentTime;
	
	// GUI SKI
	//public GUISkin skin;
	
	// References
	//public GameObject tokenParent;
	
	private bool completed = false;
	private bool showPauseMenu = false;
	private int winScreenWidth, winScreenHeight;
	
	void Update()
	{
		if (Input.GetButtonDown ("PauseMenu") && !completed) {
			PauseMenu();
		}
	}
	
	public void PauseMenu(){
		showPauseMenu = !showPauseMenu;
		if(showPauseMenu){
			pauseMenu.enabled = true;
			Cursor.visible = true;
			Screen.lockCursor = false;
			Time.timeScale = 0f;
		}else{
			pauseMenu.enabled = false;
			Cursor.visible = false;
			Screen.lockCursor = true;
			Time.timeScale = 1f;
		}
	}

	void Start()
	{
		pauseMenu.enabled = false;
		//		totalTokenCount = tokenParent.transform.childCount;
		
		if (PlayerPrefs.GetInt("Level Unlocked") > 1) //if there are more levels unlocked than level 1, let him play them
		{
			unlockedLevel = PlayerPrefs.GetInt("Level Unlocked");
		} else {
			unlockedLevel = 1;
		}
		
		if (PlayerPrefs.GetInt("Current Level") > 1) //if there are more levels unlocked than level 1, let him play them
		{
			currentLevel = PlayerPrefs.GetInt ("Current Level");
		} else {
			currentLevel = 1;
		}
	}
	
	public void CompleteLevel()
	{
		//showWinScreen = true;
		Time.timeScale = 0f;
		completed = true;
		LoadNextLevel ();
	}
	
	void LoadNextLevel()
	{
		Time.timeScale = 1f;
		if (currentLevel < maxLevels) //current level id (-1) < max level id (-1)
		{
			print ("currentLevel before: " + currentLevel);
			currentLevel += 1;
			print ("currentLevel after: " + currentLevel);
			print (currentLevel);
			SaveGame();
			//Application.LoadLevel ("Level " + currentLevel.ToString ()); //load by name instead of id
			//int nextLevel = currentLevel-1;
			Application.LoadLevel("Level " + currentLevel); //level name
			//DontDestroyOnLoad() - don't reset value with new scene
		} else {
			Application.LoadLevel("Central");
			Screen.lockCursor = false;
			print ("You win!");
		}
	}
	
	void SaveGame()
	{
		if (unlockedLevel < currentLevel) {
			unlockedLevel = currentLevel;
			PlayerPrefs.SetInt ("Level Unlocked", unlockedLevel);
			PlayerPrefs.SetInt ("Current Level", currentLevel);
		}
	}

	public void ToCenter(){
		//currentLevel += 1;
		SaveGame ();
		Application.LoadLevel("main_menu");
		Time.timeScale = 1f;
	}

	public void QuitGame(){
		SaveGame ();
		Application.Quit ();
		Time.timeScale = 1f;
	}
	
	void OnGUI()
	{
		/*
		GUI.skin = skin;
		//GUI.Label (timerRect, currentTime, skin.GetStyle ("Timer"));
		GUI.Label (new Rect(10,10,200,200), "[Esc] Menu");

		if (showPauseMenu)
		{
			Rect winScreenRect = new Rect(Screen.width/2/2, Screen.height/3/2, Screen.width/2, Screen.height/3*2);
			GUI.Box(winScreenRect, "Menu");
			if (GUI.Button(new Rect(winScreenRect.x + winScreenRect.width / 2 - 75, winScreenRect.y + winScreenRect.height / 5*1, 150, 40), "Continue"))
			{
				PauseMenu();
				//LoadNextLevel();
			}
			if (GUI.Button(new Rect(winScreenRect.x + winScreenRect.width / 2 - 75, winScreenRect.y + winScreenRect.height / 5*2, 150, 40), "Center"))
			{
				ToCenter();
			}
			if (GUI.Button(new Rect(winScreenRect.x + winScreenRect.width / 2 - 75, winScreenRect.y + winScreenRect.height / 5*3, 150, 40), "Options"))
			{
				ToCenter();
			}
			if (GUI.Button(new Rect(winScreenRect.x + winScreenRect.width / 2 - 75, winScreenRect.y + winScreenRect.height / 5*4, 150, 40), "Exit"))
			{
				QuitGame();
			}
			
			//GUI.Label(new Rect(winScreenRect.x + 20, winScreenRect.y + 40, 300, 50), " Score");
			//GUI.Label(new Rect(winScreenRect.x + 20, winScreenRect.y + 70, 300, 50), "Completed Level " + currentLevel);
		}
		*/
	}
}