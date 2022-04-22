using System.Collections ;
using System.Collections.Generic ;
using UnityEngine ;

using UnityEngine.UI;
using TMPro ;

public class UIManager : MonoBehaviour
{
	[Header("MENU")]
	public Slider m_SliderMenuHeader;
	[SerializeField]
	private GameObject m_InfosHandle;
	[SerializeField]
	private TextMeshProUGUI  m_TextHandle;

	[SerializeField]
	private GameObject[] m_NumberIcon;

	[SerializeField]
	private GameObject[] m_VerifiedCheckIcon;



	public Button m_EntrainementMenu;

	public GameObject Canvas, LoginScreen, PlayerSelectionScreen, GameSelectionScreen, BMParametersSelectionScreen, BMParametersEvalSelectionScreen, TMParametersSelectionScreen, TMParametersEvalSelectionScreen, ZMParametersSelectionScreen, ZMParametersEvalSelectionScreen;
	public GameObject MenuHeader;
	private GameObject currentScreen, MainManager;
	//[SerializeField] private GameObject loading;

	bool isUIVisible = true;
	bool isLoginScreenVisible = true, isPlayerSelectionScreenVisible = false, isGameSelectionScreenVisible = false, isBMParametersSelectionScreenVisible = false, isBMParametersEvalSelectionScreenVisible = false, isTMParametersSelectionScreenVisible = false, isTMParametersEvalSelectionScreenVisible = false, isZMParametersSelectionScreenVisible = false, isZMParametersEvalSelectionScreenVisible = false;

	int gameIndex = 0;

	void Start()
	{
		MenuHeader.SetActive(false);

		m_NumberIcon[0].SetActive(false);
		m_NumberIcon[1].SetActive(true);
		m_NumberIcon[2].SetActive(true);

		m_VerifiedCheckIcon[0].SetActive(false);
		m_VerifiedCheckIcon[1].SetActive(false);
		m_VerifiedCheckIcon[2].SetActive(false);

		Canvas.SetActive(isUIVisible);
		currentScreen = LoginScreen;
		SetLoginScreenVisible();

		MainManager = GameObject.Find("MainManager");
		MainManager.GetComponent<BeatMasterGenerator>().ResetSelection();
		MainManager.GetComponent<TrackingMasterGenerator>().ResetSelection();
		MainManager.GetComponent<ZenMasterGenerator>().ResetSelection();

		//loading.SetActive(false);
	}

	void SetScreensState(bool b1, bool b2, bool b3, bool b4, bool b5, bool b6, bool b7, bool b8, bool b9)
	{
		isLoginScreenVisible = b1;
		isPlayerSelectionScreenVisible = b2;
		isGameSelectionScreenVisible = b3;
		isBMParametersSelectionScreenVisible = b4;
		isBMParametersEvalSelectionScreenVisible = b5;
		isTMParametersSelectionScreenVisible = b6;
		isTMParametersEvalSelectionScreenVisible = b7;
		isZMParametersSelectionScreenVisible = b8;
		isZMParametersEvalSelectionScreenVisible = b9;

		LoginScreen.SetActive(isLoginScreenVisible);
		PlayerSelectionScreen.SetActive(isPlayerSelectionScreenVisible);
		GameSelectionScreen.SetActive(isGameSelectionScreenVisible);

		BMParametersSelectionScreen.SetActive(isBMParametersSelectionScreenVisible);
		BMParametersEvalSelectionScreen.SetActive(isBMParametersEvalSelectionScreenVisible);

		TMParametersSelectionScreen.SetActive(isTMParametersSelectionScreenVisible);
		TMParametersEvalSelectionScreen.SetActive(isTMParametersEvalSelectionScreenVisible);

		ZMParametersSelectionScreen.SetActive(isZMParametersSelectionScreenVisible);
		ZMParametersEvalSelectionScreen.SetActive(isZMParametersEvalSelectionScreenVisible);

	}

	public void ToggleVisible(bool state)
	{
		isUIVisible = !isUIVisible;
		Canvas.SetActive(isUIVisible);
	}

	public void LoadingAnimation()
	{
		SetScreensState(false, false, false, false, false, false, false, false, false);
		//loading.SetActive(true);
	}

	public void SetLoginScreenVisible()
	{
		currentScreen = LoginScreen;
		SetScreensState(true, false, false, false, false, false, false, false, false);
	}

	public void SetPlayerSelectionScreenVisible()
	{
		m_NumberIcon[0].SetActive(false);
		m_VerifiedCheckIcon[0].SetActive(false);

		m_NumberIcon[1].SetActive(true);
		m_VerifiedCheckIcon[1].SetActive(false);

		m_NumberIcon[2].SetActive(true);
		m_VerifiedCheckIcon[2].SetActive(false);



		if (m_SliderMenuHeader.value > 0)
		{
			StartCoroutine(c_MooveSldier(0));
			m_TextHandle.text = "1";
		}

		currentScreen = PlayerSelectionScreen;
		SetScreensState(false, true, false, false, false, false, false, false, false);
		MenuHeader.SetActive(true);
	}

	public void SetGameSelectionScreenVisible()
	{
		m_NumberIcon[0].SetActive(false);
		m_VerifiedCheckIcon[0].SetActive(true);

		m_NumberIcon[1].SetActive(false);
		m_VerifiedCheckIcon[1].SetActive(false);

		m_NumberIcon[2].SetActive(true);
		m_VerifiedCheckIcon[2].SetActive(false);

		if (m_SliderMenuHeader.value != 1)
		{
			StartCoroutine(c_MooveSldier(1));
			m_TextHandle.text = "2";
		}

		currentScreen = GameSelectionScreen;
		SetScreensState(false, false, true, false, false, false, false, false, false);
		FirestoreLoginManager manager = GameObject.Find("FirestoreManagement").GetComponent<FirestoreLoginManager>();
	}

	public void SetBMParametersSelectionScreenVisible()
	{
		gameIndex = 0;

		m_NumberIcon[0].SetActive(false);
		m_VerifiedCheckIcon[0].SetActive(true);

		m_NumberIcon[1].SetActive(false);
		m_VerifiedCheckIcon[1].SetActive(true);

		m_NumberIcon[2].SetActive(false);
		m_VerifiedCheckIcon[2].SetActive(false);

		if (m_SliderMenuHeader.value != 2)
		{
			StartCoroutine(c_MooveSldier(2));
			m_EntrainementMenu.interactable = true;
			m_TextHandle.text = "3";
		}
		currentScreen = BMParametersSelectionScreen;
		SetScreensState(false, false, false, true, false, false, false, false, false);
	}

	public void SetBMParametersEvalSelectionScreenVisible()
	{
		gameIndex = 0;

		m_NumberIcon[0].SetActive(false);
		m_VerifiedCheckIcon[0].SetActive(true);

		m_NumberIcon[1].SetActive(false);
		m_VerifiedCheckIcon[1].SetActive(true);

		m_NumberIcon[2].SetActive(false);
		m_VerifiedCheckIcon[2].SetActive(false);

		if (m_SliderMenuHeader.value != 2)
		{
			StartCoroutine(c_MooveSldier(2));
			m_EntrainementMenu.interactable = true;
			m_TextHandle.text = "3";
		}
		currentScreen = BMParametersSelectionScreen;
		SetScreensState(false, false, false, false, true, false, false, false, false);
	}

	public void SetTMParametersSelectionScreenVisible()
	{
		gameIndex = 1;

		m_NumberIcon[0].SetActive(false);
		m_VerifiedCheckIcon[0].SetActive(true);

		m_NumberIcon[1].SetActive(false);
		m_VerifiedCheckIcon[1].SetActive(true);

		m_NumberIcon[2].SetActive(false);
		m_VerifiedCheckIcon[2].SetActive(false);

		if (m_SliderMenuHeader.value != 2)
		{
			StartCoroutine(c_MooveSldier(2));
			m_EntrainementMenu.interactable = true;
			m_TextHandle.text = "3";

		}
		currentScreen = TMParametersSelectionScreen;
		SetScreensState(false, false, false, false, false, true, false, false, false);
	}

	public void SetTMParametersEvalSelectionScreenVisible()
	{
		gameIndex = 1;

		m_NumberIcon[0].SetActive(false);
		m_VerifiedCheckIcon[0].SetActive(true);

		m_NumberIcon[1].SetActive(false);
		m_VerifiedCheckIcon[1].SetActive(true);

		m_NumberIcon[2].SetActive(false);
		m_VerifiedCheckIcon[2].SetActive(false);

		if (m_SliderMenuHeader.value != 2)
		{
			StartCoroutine(c_MooveSldier(2));
			m_EntrainementMenu.interactable = true;
			m_TextHandle.text = "3";

		}
		currentScreen = TMParametersSelectionScreen;
		SetScreensState(false, false, false, false, false, false, true, false, false);
	}

	public void SetZMParametersSelectionScreenVisible()
	{
		gameIndex = 2
			;
		m_NumberIcon[0].SetActive(false);
		m_VerifiedCheckIcon[0].SetActive(true);

		m_NumberIcon[1].SetActive(false);
		m_VerifiedCheckIcon[1].SetActive(true);

		m_NumberIcon[2].SetActive(false);
		m_VerifiedCheckIcon[2].SetActive(false);

		if (m_SliderMenuHeader.value != 2)
		{
			StartCoroutine(c_MooveSldier(2));
			m_EntrainementMenu.interactable = true;
			m_TextHandle.text = "3";

		}
		currentScreen = ZMParametersSelectionScreen;
		SetScreensState(false, false, false, false, false, false, false, true, false);
	}

	public void SetZMParametersEvalSelectionScreenVisible()
	{
		gameIndex = 2; 

		m_NumberIcon[0].SetActive(false);
		m_VerifiedCheckIcon[0].SetActive(true);

		m_NumberIcon[1].SetActive(false);
		m_VerifiedCheckIcon[1].SetActive(true);

		m_NumberIcon[2].SetActive(false);
		m_VerifiedCheckIcon[2].SetActive(false);

		if (m_SliderMenuHeader.value != 2)
		{
			StartCoroutine(c_MooveSldier(2));
			m_EntrainementMenu.interactable = true;
			m_TextHandle.text = "3";

		}
		currentScreen = ZMParametersSelectionScreen;
		SetScreensState(false, false, false, false, false, false, false, false, true);
	}

	public void SetErrorMessage(string errorMessage)
	{
		currentScreen.transform.Find("errormessage_text").gameObject.GetComponent<TextMeshProUGUI>().text = errorMessage;
	}

	public void SetGame(int index)
	{
		//gameIndex = index;
		if (index == 0)
			SetBMParametersSelectionScreenVisible();
		else if (index == 1)
			SetBMParametersEvalSelectionScreenVisible();
		else if (index == 2)
			SetTMParametersSelectionScreenVisible();
		else if (index == 3)
			SetTMParametersEvalSelectionScreenVisible();
		else if (index == 4)
			SetZMParametersSelectionScreenVisible();
		else if (index == 5)
			SetZMParametersEvalSelectionScreenVisible();
		else if (index == 6)
			MainManager.GetComponent<SceneManager>().GenerateGame();


	}

	public void StartGame()
	{
		MainManager.GetComponent<SceneManager>().GenerateGame();
		SetLoginScreenVisible();
	}

	public void BMStartGameDiscover(int index)
	{
		BMTrainMode();
		SetGame(index);
		MainManager.GetComponent<SceneManager>().GenerateGame();
		SetLoginScreenVisible();
	}

	public void TMStartGameDiscover(int index)
	{
		TMTrainMode();
		SetGame(index);
		MainManager.GetComponent<SceneManager>().GenerateGame();
		SetLoginScreenVisible();
	}

	public void BMEvalMode(int value)
	{
		MainManager.GetComponent<BeatMasterGenerator>().EvaluationMode(value);
		MainManager.GetComponent<BeatMasterGenerator>().m_EvaluationModeBM = true;
	} 

	public void BMTrainMode()
	{
		MainManager.GetComponent<BeatMasterGenerator>().TrainingMode();
		MainManager.GetComponent<BeatMasterGenerator>().m_EvaluationModeBM = false;

	}

	public void SetBMModule(int BMvalueMod)
	{
		MainManager.GetComponent<BeatMasterGenerator>().SetModule(BMvalueMod);
	}

	public void SetBMLevel(Slider BMSliderValue)
	{
		int Value = (int) BMSliderValue.value;
		MainManager.GetComponent<BeatMasterGenerator>().SetLevel(Value+1);
	}

	public void SetTMModule(int TMvalueMod)
	{
		MainManager.GetComponent<TrackingMasterGenerator>().SetModule(TMvalueMod);
	}

	public void SetTMLevel(Slider TMSliderValue)
	{
		int Value = (int)TMSliderValue.value;
		MainManager.GetComponent<TrackingMasterGenerator>().SetLevel(Value+1);
	}

	public void TMEvalMode(int value)
	{
		MainManager.GetComponent<TrackingMasterGenerator>().EvaluationMode(value);
		MainManager.GetComponent<TrackingMasterGenerator>().m_EvaludationModeTM = true;

	}

	public void TMTrainMode()
	{
		MainManager.GetComponent<TrackingMasterGenerator>().TrainingMode();
		MainManager.GetComponent<TrackingMasterGenerator>().m_EvaludationModeTM = false;

	}



	public void ZMEvalMode(int value)
	{
		MainManager.GetComponent<ZenMasterGenerator>().EvaluationMode();
	}

	public void ZMTrainMode()
	{
		MainManager.GetComponent<ZenMasterGenerator>().TrainingMode();
		StartGame();
	}

	public void SetZMModule(int value)
	{
		MainManager.GetComponent<ZenMasterGenerator>().SetModule(value);
	}

	public void SetZMLevel(Slider SliderValue)
	{
		int Value = (int)SliderValue.value;
		MainManager.GetComponent<ZenMasterGenerator>().SetLevel(Value+1);
	}

	public int GetGameIndex()
	{
		return gameIndex;
	}

	IEnumerator c_MooveSldier(float DestValueSlider)
	{
		float speed = 0.9f;
		float StartValue = m_SliderMenuHeader.value;
		float timeScale = 0f;

		m_InfosHandle.SetActive(false);
		while (timeScale < 1f)
		{
			timeScale += Time.deltaTime * speed;
			m_SliderMenuHeader.value = Mathf.Lerp(m_SliderMenuHeader.value, DestValueSlider, timeScale);
			yield return null;
		}
		m_InfosHandle.SetActive(true);
	}
}
