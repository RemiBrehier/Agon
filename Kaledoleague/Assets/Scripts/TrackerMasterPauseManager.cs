using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TrackerMasterPauseManager : MonoBehaviour
{
	public XRInput xRInput;
	public MenuButtonWatcher watcher;
	public bool IsPressed = false; // used to display button state in the Unity Inspector window
	public GameObject pauseMenu;
	public static bool gameIsPaused = false;

	// Start is called before the first frame update
	void Start()
	{
		//watcher.menuButtonPress.AddListener(onMenuButtonEvent);
		Resume();

		pauseMenu.SetActive(false);
	}


	public void onMenuButtonEvent(bool pressed)
	{
		IsPressed = pressed;
		if(IsPressed)
		{
			if (gameIsPaused)
			{
				Resume();
			}
			else
			{
				Pause();
			}
		}


	}

	public void Pause()
	{
		gameIsPaused = true;
		AudioListener.pause = true;
		pauseMenu.SetActive(true);
		Time.timeScale = 0;

		foreach(GameObject ob in GameObject.FindGameObjectsWithTag("Sphere"))
		{
			var orb = ob.GetComponent<OrbController>();
			if(orb.IsChosable() || !orb.IsStopped()) orb.SetColor(Color.gray);
			if(orb.IsChosable()) ob.GetComponent<XRSimpleInteractable>().enabled = false;
		}
	}

	public void Resume()
	{
		xRInput.rightHandLastState = false;
 
		gameIsPaused = false;
		AudioListener.pause = false;
		pauseMenu.SetActive(false);
		Time.timeScale = 1;

		foreach(GameObject ob in GameObject.FindGameObjectsWithTag("Sphere"))
		{
			var orb = ob.GetComponent<OrbController>();
			if(!orb.IsChosable() && !orb.IsStopped()) orb.RefreshColor();
			if(orb.IsChosable()) ob.GetComponent<XRSimpleInteractable>().enabled = true;
		}

	}

	public void Quit()
	{
		Resume();
		GameObject.Find("MainManager").GetComponent<SceneManager>().GenerateMenu();
	}
}
