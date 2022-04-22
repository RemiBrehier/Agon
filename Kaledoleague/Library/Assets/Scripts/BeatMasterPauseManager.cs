using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class BeatMasterPauseManager : MonoBehaviour
{
	public XRInput xRInput;

	public MenuButtonWatcher watcher;
	public bool IsPressed = false; // used to display button state in the Unity Inspector window
	public GameObject pauseMenu;
	public static bool gameIsPaused = false;

	public BeatMasterSessionManager sessionManager;

	public XRInteractorLineVisual leftRayInteractor, rightRayInteractor;
	public GameObject leftSelectorCollider, rightSelectorCollider;
	public GameObject LeftHandController, RightHandController;
	public GameObject neonBlueSquare, neonRedCircle;

	[SerializeField]
	private SceneManager sceneManager;

	// Start is called before the first frame update
	void Start()
	{
		

		//Resume();
		//watcher.menuButtonPress.AddListener(onMenuButtonEvent);
		pauseMenu.SetActive(false);

		sceneManager = GameObject.FindGameObjectWithTag("MainManager").GetComponent<SceneManager>();

		if(sceneManager == null)
        {
			Debug.LogError("Scene Manager dans MainManager introuvable.");
        }
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
		leftRayInteractor.enabled = true; 
		rightRayInteractor.enabled = true;

		leftSelectorCollider.SetActive(false);
		rightSelectorCollider.SetActive(false);

		gameIsPaused = true;
		AudioListener.pause = true;
		pauseMenu.SetActive(true);

		Time.timeScale = 0;

		/*if (sessionManager.IsInSession())
		{
			sessionManager.SetInteractors(true);
		}*/
	}

	public void Resume()
	{
		xRInput.rightHandLastState = false;

		leftRayInteractor.enabled = false; 
		rightRayInteractor.enabled = false;

		leftSelectorCollider.SetActive(true);
		rightSelectorCollider.SetActive(true);

		gameIsPaused = false;
		AudioListener.pause = false;
		pauseMenu.SetActive(false);
		Time.timeScale = 1;

		/*if (sessionManager.IsInSession())
		{
			sessionManager.SetInteractors(false);
		}*/


	}

	public void Quit()
	{
		Resume();
		Debug.Log("QUITTER");
		sceneManager.GenerateMenu();
	}
}
