using System.Collections ;
using System.Collections.Generic;
using UnityEngine ;
using UnityEngine.UI;
using TMPro;

public class SceneManager : MonoBehaviour
{
	private AsyncOperation asyncLoad;
	private float progress;
	private bool StartGame = false;
	private bool StartMenu = false;

	[Header("FADER")]
	[SerializeField]
	private GameObject m_fader;
	[SerializeField, Tooltip("Dur�e du fondu")]
	private float m_FadeDuration;
	[SerializeField, Tooltip("Valeur de fin de fondu")]
	private Color opaqueColor = Color.black;
	[SerializeField, Tooltip("Valeur de fin de fondu")]
	private Color transparentColor = Color.clear;

	[Header("LOAD SCREEN")]
	[SerializeField]
	private GameObject m_loadingScreenGame;
	[SerializeField]
	public Slider m_slider;
	[SerializeField]
	public TextMeshProUGUI m_ProgressText;


    private void Start()
    {
		//Am�lioration de la fluidit� de l'application
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 120;

		//Fonction r�cup�rant le GameObject "Fader" et "LoadingScreenGame" � chaque changement de sc�ne.
		LoadAndFade();
	}

    void Update()
    {
		if (m_loadingScreenGame == null || m_fader == null)
		{
			LoadAndFade();
		}

		//Lancement du fondu au noir
		if(StartMenu == false)
        {
			if (StartGame == false && progress >= 1)
			{
				StartGame = true;
				progress = 0;
				StartCoroutine(c_CloseFader());
			}
		}
	}

	public void LoadAndFade()
    {
		m_loadingScreenGame = GameObject.FindGameObjectWithTag("Loader");
		m_fader = GameObject.FindGameObjectWithTag("Fader");

		if (m_loadingScreenGame != null)
        {
			//R�cup�ration du slider de chargement
			GameObject slider;
			slider = m_loadingScreenGame.transform.Find("LoadingSlider").gameObject;
			m_slider = slider.GetComponent<Slider>();

			//R�cup�ration du champ texte pour indiqu� le pourcentage du chargement de la sc�ne appel�e
			GameObject text;
			text = m_loadingScreenGame.transform.Find("LoadingSlider/Progress_Text").gameObject;
			m_ProgressText = text.GetComponent<TextMeshProUGUI>();

			m_loadingScreenGame.SetActive(false);
		}
		else
        {
			Debug.LogWarning("Ecran de chargement introuvable. V�rifier que le tag soit bien appliqu� sur le GameObject de l'interface de chargement");
		}

		if (m_fader == null)
        {
			Debug.LogWarning("Ecran de fondu au noir introuvable. V�rifier que le tag soit bien appliqu� sur le GameObject de fondu au noir");
		}
	}


	public void GenerateMenu()
	{
		StartMenu = true;
		StartCoroutine(c_CloseFader());
		StartCoroutine(LoadYourAsyncScene("MenuScene"));
	}

	public void GenerateGame()
	{
		StartMenu = false;
		StartGame = false;

		//Chargement d'un jeu selon l'index s�lectionn�
		int gameIndex = GameObject.Find("UIManager").GetComponent<UIManager>().GetGameIndex() ;

		if (gameIndex == 0)
			StartCoroutine(LoadYourAsyncScene("BeatMaster"));
		else if (gameIndex == 1)
			StartCoroutine(LoadYourAsyncScene("TrackerMaster"));
		else if (gameIndex == 2)
			StartCoroutine(LoadYourAsyncScene("ZenMaster"));
		else if (gameIndex == 3)
			StartCoroutine(LoadYourAsyncScene("VoileScene"));
	}

	IEnumerator LoadYourAsyncScene(string scene)
    {
		if(StartMenu == true)
        {
			yield return new WaitForSeconds(2f);
        }
		//Chargement de la sc�ne appel� en asynchrone
		asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene);

		if(StartMenu == false)
        {
			asyncLoad.allowSceneActivation = false;
		}
		else
        {
			asyncLoad.allowSceneActivation = true;
		}

		m_loadingScreenGame.SetActive(true);

		while (!asyncLoad.isDone)
		{
			//Indication visuel du chargement en pourcentage de la sc�ne appel� via l'interface "LoadingScreenGame"
			progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
			m_slider.value = progress;
			m_ProgressText.text = progress * 100 + "%";
			yield return null;
		}
	}

	//Coroutine de fondu au noir � la fin du chargement en asynchrone de la scene appel�e
	public IEnumerator c_CloseFader()
	{
		float counter = 0;
		Debug.Log("ENTER FADER");
		while (counter < m_FadeDuration)
		{
			Debug.Log("GO FADER");

			counter += Time.deltaTime;
			float colorTime = counter / m_FadeDuration;
			m_fader.GetComponent<Image>().color = Color.Lerp(transparentColor, opaqueColor, counter / m_FadeDuration);

			if (m_fader.GetComponent<Image>().color.a >= 1 && asyncLoad.allowSceneActivation == false)
			{ 
				//Activation du chargement de la sc�ne apr�s l'alpha � 1 du fondu au noir
				m_fader.GetComponent<Image>().color = opaqueColor;
				asyncLoad.allowSceneActivation = true;
			}
			yield return null;
		}
	}
}
