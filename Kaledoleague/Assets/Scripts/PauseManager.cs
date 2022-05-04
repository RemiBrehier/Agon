using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
	public MenuButtonWatcher watcher;
	public bool IsPressed = false; // used to display button state in the Unity Inspector window
	public GameObject pauseMenu;
	public static bool gameIsPaused = false;

	// Start is called before the first frame update
	void Start()
	{
		watcher.menuButtonPress.AddListener(onMenuButtonEvent);
		pauseMenu.SetActive(false);
	}

	public void onMenuButtonEvent(bool pressed)
	{
		IsPressed = pressed;
		if(IsPressed)
		{
			if(gameIsPaused) Resume();
			else Pause();
		}
	}

	public void Pause()
	{
		gameIsPaused = true;
		AudioListener.pause = true;
		pauseMenu.SetActive(true);
		Time.timeScale = 0;
	}

	public void Resume()
	{
		gameIsPaused = false;
		AudioListener.pause = false;
		pauseMenu.SetActive(false);
		Time.timeScale = 1;
	}

	public void Quit()
	{
		Resume();
		GameObject.Find("MainManager").GetComponent<SceneManager>().GenerateMenu();
	}
}
