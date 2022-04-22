using System.Collections ;
using System.Collections.Generic ;
using UnityEngine ;

using TMPro ;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class BeatMasterSessionManager : MonoBehaviour
{

	[SerializeField]
	private GameObject m_MainManager;

	[Header("ENCOURAGEMENT")]
	[SerializeField]
	private EncouragementManager m_encouragementManager;
	[SerializeField]
	private List<AudioClip> m_goodEncouragement;
	[SerializeField]
	private List<AudioClip> m_badEncouragement;
	[SerializeField]
	private AudioSource m_audiosourceEncouragement;
	private int GoodScoreHitCount = 0;
	private int BadScoreHitCount = 0;


	[Header("COLOR")]
	public Color[] ChoiceColor;

	Vector4 [] materialColors = { Color.blue, Color.red, Color.green, Color.yellow } ;
	private string [] _colors = new string[] {"blue", "red", "green", "yellow"} ;
	private string [] _shapes = new string[] {"circle", "square", "triangle", "cross"} ;

	public GameObject target ;
	public GameObject obstacle ;

	[Header("ENTRAINEMENT")]
	[SerializeField]
	private Slider m_EntSliderScore;
	[SerializeField]
	private Slider m_EntSliderAccomplissement;
	[SerializeField]
	private TextMeshProUGUI m_TitlePrevisionSelector;

	[Header("EVALUATION")]
	[SerializeField]
	private Slider m_EvalSliderScore;
	[SerializeField]
	private Slider m_EvalSliderAccomplissement;
	[SerializeField]
	private TextMeshProUGUI m_TitleEvaluationSelector;

	int song = 0 ;
	int elements = 2 ;
	int colors = 3 ;
	int hFov = 45 ;
	int vFov = 45 ;
	int speed = 3 ;
	int obstacles = 10 ;
	int ruleChanging = 1 ;
	int selectorChanging = 1 ;
	int module = 0 ;
	int level = 0 ;
	int bpm = 60 ;

	GameObject soundManager ;
	GameObject dataManager ;

	GameObject RuleBoard ;
	GameObject ScoreBoard ;
	GameObject MultiplierBoard ;
	[SerializeField] private GameObject loading ;

	[SerializeField] private GameObject previsionSelector ;
	[SerializeField] private GameObject evaluationSelector ;
	GameObject previsionF1ScoreSlider ;
	GameObject previsionAchievementSlider ;
	GameObject evaluationF1ScoreSlider ;
	GameObject evaluationAAchievementSlider ;

	public XRInteractorLineVisual leftRayInteractor, rightRayInteractor ;
	public GameObject leftSelectorCollider, rightSelectorCollider ;
	public GameObject LeftHandController, RightHandController;

	public GameObject neonBlueSquare, neonRedCircle ;
	[SerializeField]
	private AudioSource audioSource ;

	bool sessionHasBegun = false ;

	bool inCoroutine = false ;
	bool shouldExit = false ;

	float timeBetweenSpawns = 1.0f ;
	float timeBetweenChanges = 8.0f ;
	float timeOfLastChange = 0.0f ;

	float score = 0 ;
	float maxScore = 0 ;
	int multiplier = 1 ;
	int maxMultiplier = 1 ;

	int id = 0 ;
	int rule = 0 ;
	string selectorCombination = "" ;

	private int [] confusionMatrix = new int [] {0, 0, 0, 0} ;

	float f1score = 0.0f ;
	float achievement = 0.0f ;
	float efficiency = 0.0f ; 
	float sensibility = 0.0f ;
	float accuracy = 0.0f ;

	float shapeAchieved = 0.0f ;
	float shapeTotal = 0.0f ;
	float colorAchieved = 0.0f ;
	float colorTotal = 0.0f ;
	float shapeAndColorAchieved = 0.0f ;
	float shapeAndColorTotal = 0.0f ;

	float previsionF1Score = .5f ;
	float previsionAchievement = .5f ;
	float evaluationF1Score = .5f ;
	float evaluationAchievement = .5f ;

	int position = -1 ;
	string type = "" ;
	int hand = -1 ;

	int oldLeftColor = 0 ;
	int oldLeftShape = 0 ;

    // Start is called before the first frame update
    void Awake()
    {

		previsionF1Score = (m_EntSliderScore.value + 1) / 100.0f;
		previsionAchievement = (m_EntSliderAccomplissement.value + 1) / 100.0f; 

		leftRayInteractor = GameObject.Find("LeftHandController").GetComponent<XRInteractorLineVisual>();
		rightRayInteractor = GameObject.Find("RightHandController").GetComponent<XRInteractorLineVisual>();

		neonBlueSquare = GameObject.Find("NeonBlueSquare");
		neonRedCircle = GameObject.Find("NeonRedCircle");

		leftSelectorCollider = GameObject.Find("LeftSelectorCollider");
		rightSelectorCollider = GameObject.Find("RightSelectorCollider");

		LeftHandController = GameObject.Find("LeftHandController");
		RightHandController = GameObject.Find("RightHandController");

		//previsionSelector = GameObject.Find("Entrainement_Selector") ;
		//evaluationSelector = GameObject.Find("Evaluation_Selector") ;

		InitPrevisionStep();

		m_MainManager = GameObject.FindGameObjectWithTag("MainManager");

		if(m_MainManager == null)
        {
			Debug.LogError("MainManager introuvable");
        }
        else if(m_MainManager != null)
        {

			if(m_MainManager.GetComponent<BeatMasterGenerator>().m_EvaluationModeBM == true)
            {
				m_TitlePrevisionSelector.text = "Évaluation";
				m_TitleEvaluationSelector.text = "Évaluation";
			}
			else if(m_MainManager.GetComponent<BeatMasterGenerator>().m_EvaluationModeBM == false)
            {
				m_TitlePrevisionSelector.text = "Entraînement";
				m_TitleEvaluationSelector.text = "Entraînement";
			}

        }
		//loading = GameObject.Find("LoadingSlider") ;
		loading.SetActive(false) ;
		soundManager = GameObject.Find("[MANAGER]/SoundManager") ;
		dataManager = GameObject.Find("[MANAGER]/DataManager") ;

		GameObject [] gos = GameObject.FindGameObjectsWithTag("RuleBoard") ;
		RuleBoard = gos[0] ;
		gos = GameObject.FindGameObjectsWithTag("ScoreBoard") ;
		ScoreBoard = gos[0] ;
		gos = GameObject.FindGameObjectsWithTag("MultiplierBoard") ;
		MultiplierBoard = gos[0] ;

		RuleBoard.GetComponent<TextMeshProUGUI>().text = "Formes et couleurs" ;
		ScoreBoard.GetComponent<TextMeshProUGUI>().text = "Score : 0" ;
		MultiplierBoard.GetComponent<TextMeshProUGUI>().text = "Séquence : 0" ;

		if (GameObject.Find ("MainManager") != null)
		{
			//Récupération des data du Beat Master généré à la scéne d'accueil
			GameObject initializer = GameObject.Find("MainManager") ;
			BeatMasterGenerator bmg = initializer.GetComponent<BeatMasterGenerator>() ;
			module = bmg.GetModule() ;
			level = bmg.GetLevel() ;
			song = bmg.GetSong() ;
			elements = bmg.GetElements() ;
			colors = bmg.GetColors() ;
			hFov = bmg.GetHFov() ;
			vFov = bmg.GetVFov() ;
			speed = bmg.GetSpeed() ;
			obstacles = bmg.GetObstacles() ;
			ruleChanging = bmg.GetRuleChanging() ;
			selectorChanging = bmg.GetSelectorChanging() ;
			bpm = bmg.GetBPM() ;

			Debug.Log("module : "+ module);
			Debug.Log("level : " + level);
			Debug.Log("song : " + song);
			Debug.Log("elements : " + elements);
			Debug.Log("colors : " + colors);
			Debug.Log("hFov : " + hFov);
			Debug.Log("vFov : " + vFov);
			Debug.Log("speed : " + speed);
			Debug.Log("obstacles : " + obstacles);
			Debug.Log("ruleChanging : " + ruleChanging);
			Debug.Log("selectorChanging : " + selectorChanging);
			Debug.Log("bpm : " + bpm);

		}
		else if (GameObject.Find("MainManager") == null)
        {
			Debug.LogError("MainManger introuvable");
        }
		//obstacles = 0 ;

		audioSource = GetComponent<AudioSource>() ;

		AudioClip clip ;
		/*if (module == 4 || module == 0)
			clip = Resources.Load<AudioClip>("Songs/BMSongs/module1level1");
		else
		{
			if (song < 10)
				clip = Resources.Load<AudioClip>("Songs/BMSongs/module" + module + "level" + level);
			else
				clip = Resources.Load<AudioClip>("Songs/BMSongs/module1level1") ;
		}*/

		if (module == 4 || song == 0)
		{
			clip = Resources.Load<AudioClip>("Songs/BMSongs/module1level1");

		}
		else
        {
			clip = Resources.Load<AudioClip>("Songs/BMSongs/module" + module + "level" + level);
		}

		audioSource.clip = clip ;

		timeBetweenSpawns = 60.0f / bpm ;
		switch (speed)
		{
			case 0 :
				timeBetweenSpawns *= 2 ;
				break ;
			case 1 :
				timeBetweenSpawns *= 1 ;
				break ;
			case 2 :
				timeBetweenSpawns /= 2 ;
				break ;
			case 3 :
				timeBetweenSpawns /= 3 ;
				break ;
			case 4 :
				timeBetweenSpawns /= 4 ;
				break ;
			case 5	 :
				timeBetweenSpawns /= 8 ;
				break ;
			default :
				break ;
		}



		
    }


	void InitPrevisionStep()
	{
		Debug.Log("INIT PREVISION STEP");
		neonBlueSquare.SetActive(false) ;
		neonRedCircle.SetActive(false) ;

		leftSelectorCollider.SetActive(false) ;
		rightSelectorCollider.SetActive(false) ;

		/*LeftHandController.SetActive(false);
		RightHandController.SetActive(false);*/

		previsionSelector.SetActive(true) ;
		evaluationSelector.SetActive(false) ;

		leftRayInteractor.enabled= true ;
		rightRayInteractor.enabled = true;

		/*RuleBoard.SetActive(false) ;
		ScoreBoard.SetActive(false) ;*/
	}



	void InitSession()
	{
		Debug.Log("INIT SESSION");


		neonBlueSquare.SetActive(true) ;
		neonRedCircle.SetActive(true) ;

		LeftHandController.SetActive(true);
		RightHandController.SetActive(true);

		leftSelectorCollider.SetActive(true) ;
		rightSelectorCollider.SetActive(true) ;

		previsionSelector.SetActive(false) ;
		evaluationSelector.SetActive(false) ;

		leftRayInteractor.enabled = false;
		rightRayInteractor.enabled = false;

		/*RuleBoard.SetActive(true) ;
		ScoreBoard.SetActive(true) ;*/

		if(module == 4)
		{
			Random.InitState(42);
			Debug.Log("Module 4 detected, setting seed for random ...");
		}

		sessionHasBegun = true ;
		timeOfLastChange = Time.time ;
		audioSource.Play() ;
	}

	void InitEvaluationStep()
	{
		Debug.Log("INIT EVALUATION STEP");
		GameObject [] indicators = GameObject.FindGameObjectsWithTag("NeonIndicator") ;
		foreach (var indicator in indicators)
			indicator.SetActive(false) ;

		leftSelectorCollider.SetActive(false) ;
		rightSelectorCollider.SetActive(false) ;

		/*LeftHandController.SetActive(false);
		RightHandController.SetActive(false);
		*/
		previsionSelector.SetActive(false) ;
		evaluationSelector.SetActive(true) ;

		leftRayInteractor.enabled = true ;
		rightRayInteractor.enabled = true;

		/*RuleBoard.SetActive(false) ;
		ScoreBoard.SetActive(false) ;*/
	}


	//Incrémentation et décrementation du slider à l'aide des boutons en mode entrainement
	public void IncEntScore()
    {
		m_EntSliderScore.value += 1;
	}

	public void DecEntScore()
	{
		m_EntSliderScore.value -= 1;
	}

	public void IncEntAccomp()
	{
		m_EntSliderAccomplissement.value += 1;
	}

	public void DecEntAccomp()
	{
		m_EntSliderAccomplissement.value -= 1;
	}


	//Incrémentation et décrementation du slider à l'aide des boutons en mode Evaluation
	public void IncEvalScore()
	{
		m_EvalSliderScore.value += 1;
	}

	public void DecEvalScore()
	{
		m_EvalSliderScore.value -= 1;
	}

	public void IncEvalAccomp()
	{
		m_EvalSliderAccomplissement.value += 1;
	}

	public void DecEvalAccomp()
	{
		m_EvalSliderAccomplissement.value -= 1;
	}


	/*public void IncPrevisionF1Score()
	{
		GameObject score = GameObject.Find("PrevisionSelector/Canvas/Slider - F1Score (Value)") ;
		float value = score.GetComponent<Slider>().value ;
		if (value < 100)
			score.GetComponent<Slider>().value = value + 1 ;
	}

	public void DecPrevisionF1Score()
	{
		GameObject score = GameObject.Find("PrevisionSelector/Canvas/Slider - F1Score (Value)") ;
		float value = score.GetComponent<Slider>().value ;
		if (value > 0)
			score.GetComponent<Slider>().value = value - 1 ;
	}

	public void IncPrevisionAchievement()
	{
		//+1
		GameObject score = GameObject.Find("PrevisionSelector/Canvas/Slider - Achievement (Value)") ;
		float value = score.GetComponent<Slider>().value ;
		if (value < 100)
			score.GetComponent<Slider>().value = value + 1 ;
	}

	public void DecPrevisionAchievement()
	{
		GameObject score = GameObject.Find("PrevisionSelector/Canvas/Slider - Achievement (Value)") ;
		float value = score.GetComponent<Slider>().value ;
		if (value > 0)
			score.GetComponent<Slider>().value = value - 1 ;
	}*/

	/*public void IncEvaluationF1Score()
	{
		//+1
		GameObject score = GameObject.Find("EvaluationSelector/Canvas/Slider - F1Score (Value)") ;
		float value = score.GetComponent<Slider>().value ;
		if (value < 100)
			score.GetComponent<Slider>().value = value + 1 ;
	}

	public void DecEvaluationF1Score()
	{
		GameObject score = GameObject.Find("EvaluationSelector/Canvas/Slider - F1Score (Value)") ;
		float value = score.GetComponent<Slider>().value ;
		if (value > 0)
			score.GetComponent<Slider>().value = value - 1 ;
	}

	public void IncEvaluationAchievement()
	{
		//+1
		GameObject score = GameObject.Find("EvaluationSelector/Canvas/Slider - Achievement (Value)") ;
		float value = score.GetComponent<Slider>().value ;
		if (value < 100)
			score.GetComponent<Slider>().value = value + 1 ;
	}

	public void DecEvaluationAchievement()
	{
		GameObject score = GameObject.Find("EvaluationSelector/Canvas/Slider - Achievement (Value)") ;
		float value = score.GetComponent<Slider>().value ;
		if (value > 0)
			score.GetComponent<Slider>().value = value - 1 ;
	}*/

	/*public void setPrevisionF1Score(float value)
	{
		previsionF1Score = value / 100.0f;
	}

	public void setPrevisionAchievement(float value)
	{
		previsionAchievement = value / 100.0f ;
	}*/

	/*public void setEvaluationF1Score(float value)
	{
		evaluationF1Score = value / 100.0f ;
	}

	public void setEvaluationAchievement(float value)
	{
		evaluationAchievement = value / 100.0f ;
	}*/

	public void RegisterPrevisions()
	{

		previsionF1Score = (m_EntSliderScore.value+1) / 100.0f;
		previsionAchievement = (m_EntSliderAccomplissement.value+1) / 100.0f;

		if (module != 0) dataManager.GetComponent<BeatMasterFirestoreDataManager>().InitParameters(previsionF1Score, previsionAchievement) ;
		InitSession() ;
	}


	public void RegisterEvaluations()
	{
		evaluationF1Score = (m_EvalSliderScore.value+1) / 100.0f;
		evaluationAchievement = (m_EvalSliderAccomplissement.value+1) / 100.0f;

		if (module != 0) dataManager.GetComponent<BeatMasterFirestoreDataManager>().RecordSession(evaluationF1Score, evaluationAchievement) ;
		StartCoroutine(ExitSession()) ;
	}

    void Update()
    {
		if ( (audioSource.time >= audioSource.clip.length -1) && !shouldExit)
		{
			sessionHasBegun = false ;
			shouldExit = true ;
			Debug.Log("Should exit soon") ;
			InitEvaluationStep() ;
		}

		if (sessionHasBegun && (audioSource.time < audioSource.clip.length - 5))
		{
			if (!inCoroutine)
				StartCoroutine(PlayEvent()) ;
		}
    }

	
	IEnumerator ExitSession()
	{
		loading.SetActive(true) ;
		previsionSelector.SetActive(false) ;
		evaluationSelector.SetActive(false) ;

		GameObject SceneManager;
		SceneManager = GameObject.Find("MainManager");
		SceneManager.GetComponent<SceneManager>().GenerateMenu();
		yield return null;


	}

	IEnumerator PlayEvent()
	{
		inCoroutine = true ;

		bool willSpawn = Mathf.Floor(Random.Range(0, 8)) < 3 ;

		int RandomRuleChange = 1 ;	
		if (ruleChanging > 0)
			RandomRuleChange = (int) Mathf.Floor(Random.Range(0, 50 / ruleChanging)) ;

		int RandomSelectorChange = 1 ;
		if (selectorChanging > 0)
			RandomSelectorChange = (int) Mathf.Floor(Random.Range(0, 50 / selectorChanging)) ;

		int RandomObstacle = 1 ;
		if (obstacles > 0)
			RandomObstacle = (int) Mathf.Floor(Random.Range(0, 50 / obstacles * speed)) ;

		if ( (Time.time - timeOfLastChange > timeBetweenChanges) && ruleChanging > 0 && RandomRuleChange == 0)
		{
			timeOfLastChange = Time.time ;
		    yield return new WaitForSeconds(4 * 60.0f / bpm) ;
			
			int ruleInc = (int) ( Mathf.Floor(Random.Range(0, 99) ) ) % 2 + 1 ;

			rule = ( rule  + ruleInc ) % 3 ;

			soundManager.GetComponent<BeatMasterSoundGameEvents>().PlayRuleChangedEvent() ;
			UpdateSessionData("RuleChanging", id) ;
			
			if (rule == 0)
				RuleBoard.GetComponent<TextMeshProUGUI>().text = "Formes et couleurs" ;
			else if (rule == 1)
				RuleBoard.GetComponent<TextMeshProUGUI>().text = "Formes" ;
			else if (rule == 2)
				RuleBoard.GetComponent<TextMeshProUGUI>().text = "Couleurs" ;

			Debug.Log("Rule changed : " + rule) ;
		    yield return new WaitForSeconds(1 * 60.0f / bpm) ;

		}
		else if ((Time.time - timeOfLastChange > timeBetweenChanges) && selectorChanging > 0 && RandomSelectorChange == 0)
		{
			timeOfLastChange = Time.time ;
		    yield return new WaitForSeconds(4 * 60.0f / bpm) ;
			soundManager.GetComponent<BeatMasterSoundGameEvents>().PlaySelectorChangedEvent() ;

			int leftColor = (int)  Mathf.Floor(Random.Range(0, colors)) ;
			if (leftColor >= colors)
				leftColor = 0 ;

			int leftShape = (int) Mathf.Floor(Random.Range(0, elements)) ;
			if (leftShape >= elements)
				leftShape = 0 ;

			if (leftColor == oldLeftColor && leftShape == oldLeftShape)
			{
				if (Mathf.Floor(Random.Range(0, 2)) > 1)
					leftShape = (leftShape + 1) % elements ;
				else
					leftColor = (leftColor + 1) % colors ;
			}
			oldLeftColor = leftColor ;
			oldLeftShape = leftShape ;

			int rightColor = (leftColor + (int) ( Mathf.Floor(Random.Range(0, 99) ) ) % (colors-1) + 1) % colors ;
			int rightShape = (leftShape + (int) ( Mathf.Floor(Random.Range(0, 99) ) ) % (elements-1) + 1) % elements ;

			selectorCombination = "Left : " + _colors[leftColor] + " - " + _shapes[leftShape] + " / Right : " + _colors[rightColor] + " - " + _shapes[rightShape] ;
			Debug.Log("Selector changed : " + selectorCombination) ;
			// change Weapon Attribute for Weapon TAG
			GameObject selectorLeft = GameObject.Find("/Player/Camera Offset/LeftHandController/LeftSelectorCollider") ;
			selectorLeft.GetComponent<SelectorBehaviour>().SetColorAndShape(_colors[leftColor], _shapes[leftShape]) ;
			GameObject selectorRight = GameObject.Find("/Player/Camera Offset/RightHandController/RightSelectorCollider") ;
			selectorRight.GetComponent<SelectorBehaviour>().SetColorAndShape(_colors[rightColor], _shapes[rightShape]) ;

			// change Weapon NEON INDICATOR TAG
			GameObject [] indicators = GameObject.FindGameObjectsWithTag("NeonIndicator") ;
			foreach (var indicator in indicators)
				Destroy(indicator) ;

			GameObject handControllerLeft = GameObject.Find("/Player/Camera Offset/LeftHandController") ;
			GameObject handControllerRight = GameObject.Find("/Player/Camera Offset/RightHandController") ;

			//Récupération du gameobject Left ou Right SelectorCollider pour instancier le nouveau selecteur en enfant.
			GameObject LeftSelectorCollider = GameObject.Find("/Player/Camera Offset/LeftHandController/LeftSelectorCollider");
			GameObject RightSelectorCollider = GameObject.Find("/Player/Camera Offset/RightHandController/RightSelectorCollider");


			string leftObjectName = "Prefabs/Neon" + UpperFirst(_colors[leftColor]) + UpperFirst(_shapes[leftShape]) ;
			string rightObjectName = "Prefabs/Neon" + UpperFirst(_colors[rightColor]) + UpperFirst(_shapes[rightShape]) ;

			GameObject leftNewIndicator = Instantiate(Resources.Load(leftObjectName) as GameObject, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0)) ;
			
			GameObject rightNewIndicator = Instantiate(Resources.Load(rightObjectName) as GameObject, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0)) ;

			//leftNewIndicator.transform.SetParent(handControllerLeft.transform) ;
			leftNewIndicator.transform.SetParent(LeftSelectorCollider.transform);
			leftNewIndicator.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f) ;
			leftNewIndicator.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f) ;
			leftNewIndicator.transform.localScale = new Vector3(0.14f, 0.14f, 0.05f) ;

			//rightNewIndicator.transform.SetParent(handControllerRight.transform) ;
			rightNewIndicator.transform.SetParent(RightSelectorCollider.transform);
			rightNewIndicator.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f) ;	
			rightNewIndicator.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f) ;
			rightNewIndicator.transform.localScale = new Vector3(0.14f, 0.14f, 0.05f) ;
		    yield return new WaitForSeconds(2 * 60.0f / bpm) ;

		}
		else if (obstacles > 0 && RandomObstacle == 0)
		{
			SpawnObstacle() ;
		}
		else if (willSpawn)
		{
			SpawnTarget() ;
		}
		yield return new WaitForSeconds(timeBetweenSpawns) ;
		
		inCoroutine = false ;
	}


	void SpawnTarget()
	{
		int choiceMesh = (int) Mathf.Floor(Random.Range(0, elements)) ;
		int choiceColor = (int) Mathf.Floor(Random.Range(0, colors)) ;

		GameObject _target = Instantiate(target) ;

		if (choiceMesh == 0)
		{

			Mesh mesh = Resources.Load<Mesh>("Meshes/Sphere");
			_target.GetComponent<MeshFilter>().mesh = mesh;
		}
		else if (choiceMesh == 1)
		{
			Mesh mesh = Resources.Load<Mesh>("Meshes/Cube");
			_target.GetComponent<MeshFilter>().mesh = mesh;
		}
		else if (choiceMesh == 2)
		{
			Mesh mesh = Resources.Load<Mesh>("Meshes/Pyramide") ;
			_target.GetComponent<MeshFilter>().mesh = mesh ;
		}
		else if (choiceMesh == 3)
		{
			/*GameObject obj = Resources.Load<GameObject>("Meshes/Cross") ;
			Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh ;

			_target.GetComponent<MeshFilter>().mesh = mesh ;*/

			Mesh mesh = Resources.Load<Mesh>("Meshes/Croix");
			_target.GetComponent<MeshFilter>().mesh = mesh;
		}

		_target.GetComponent<MeshRenderer>().material.color = materialColors[choiceColor] ;
		_target.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", ChoiceColor[choiceColor]);
		_target.GetComponent<TrailRenderer>().startColor = ChoiceColor[choiceColor];
		_target.GetComponent<TrailRenderer>().endColor = ChoiceColor[choiceColor];

		_target.GetComponent<MoveTowardPlayer>().Init(hFov, vFov, bpm) ;

		_target.GetComponent<BeatMasterTargetBehaviour>().SetTargetProperties(_shapes[choiceMesh], _colors[choiceColor], rule, 0) ;

	}

	void SpawnObstacle()
	{
		GameObject _obstacle = Instantiate(obstacle) ;	
		_obstacle.GetComponent<ObstacleBehaviour>().Init(hFov, vFov, bpm) ;
	}

	public void IncreaseScore()
	{
	
		if (m_encouragementManager.m_Encouragement == true)
        {
			if(!m_audiosourceEncouragement.isPlaying)
            {
				GoodScoreHitCount++;
				bool PlayAudio = false;

				if (GoodScoreHitCount > 10)
				{
					PlayAudio = true;
					if (PlayAudio == true)
					{
						m_audiosourceEncouragement.PlayOneShot(m_goodEncouragement[Random.Range(0, m_goodEncouragement.Count)], 1f);
						PlayAudio = false;
						GoodScoreHitCount = 0;
						BadScoreHitCount = 0;
					}
				}		
			}		
		}

		if (multiplier < 10)
			multiplier++ ;
		if (maxMultiplier < 10)
			maxMultiplier++ ;
		score += Mathf.Pow(2, multiplier) ;
		maxScore += Mathf.Pow(2, maxMultiplier) ;
		ScoreBoard.GetComponent<TextMeshProUGUI>().text = "Score : " + score ;

		string multiplierMessage = "Séquence : " ;
		for (int i = 0 ; i < multiplier ; i++ )
			multiplierMessage += "*" ;
		MultiplierBoard.GetComponent<TextMeshProUGUI>().text = multiplierMessage ;
	}


	public void ResetMultiplier()
	{

		if (m_encouragementManager.m_Encouragement == true)
		{
			if (!m_audiosourceEncouragement.isPlaying)
			{
				BadScoreHitCount++;
				bool PlayAudio = false;

				if (BadScoreHitCount > 10)
				{
					PlayAudio = true;
					if (PlayAudio == true)
					{
						m_audiosourceEncouragement.PlayOneShot(m_badEncouragement[Random.Range(0, m_badEncouragement.Count)], 1f);
						PlayAudio = false;
						BadScoreHitCount = 0;
						GoodScoreHitCount = 0;
					}
				}		
			}
		}

		multiplier = 1 ;
		if (maxMultiplier < 10)
			maxMultiplier++ ;
		maxScore += Mathf.Pow(2, maxMultiplier) ;

		string multiplierMessage = "Séquence : 0" ;
		MultiplierBoard.GetComponent<TextMeshProUGUI>().text = multiplierMessage ;
	}

	private string UpperFirst(string text)
 	{
		return char.ToUpper(text[0]) + ((text.Length > 1) ? text.Substring(1).ToLower() : string.Empty) ;
	}

	void UpdateConfusionScores()
	{
		float gp = 1.0f * confusionMatrix[0] ;
		float fp = 1.0f * confusionMatrix[1] ;
		float fn = 1.0f * confusionMatrix[2] ;
		float gn = 1.0f * confusionMatrix[3] ;
		
		if (gp > 0 || fp > 0)
		{
			accuracy = gp / (gp + fp) ;
		}
		if (gp > 0 || fn > 0)
		{
			sensibility = gp / (gp + fn) ;
		}

		if (accuracy > 0 || sensibility > 0)
		{
			f1score = 2.0f * accuracy * sensibility / (accuracy + sensibility) ;
		}

		if (gp >0 || fp > 0 || fn > 0 || gn > 0 )
			efficiency = (gp + gn) / (gp + fp + fn + gn) ;
	}
	

    public void GoodHit(int id, string t, int h, int r)
    {
		type = t ;
		hand = h ;

		IncreaseScore() ;

        confusionMatrix[0]++ ;
		UpdateConfusionScores() ;
		UpdateSessionData("TruePositive", id) ;

		if (r == 0)
		{
			shapeAndColorAchieved++ ;
			shapeAndColorTotal++ ;
		}
		if (r == 1)
		{
			shapeAchieved++ ;
			shapeTotal++ ;
		}
		if (r == 2)
		{
			colorAchieved++ ;
			colorTotal++ ;
		}
    }

    public void BadHit(int id, string t, int h, int r)
    {
		type = t ;
		hand = h ;

		ResetMultiplier() ;

        confusionMatrix[1]++ ;
		UpdateConfusionScores() ;
		UpdateSessionData("FalsePositive", id) ;
	
		if (r == 0)
		{
			shapeAndColorTotal++ ;
		}
		if (r == 1)
		{
			shapeTotal++ ;
		}
		if (r == 2)
		{
			colorTotal++ ;
		}
    }

    public void GoodMiss(int id, string t, int r)
    {
		type = t ;

		IncreaseScore() ;

		if (r == 0)
		{
			shapeAndColorAchieved++ ;
			shapeAndColorTotal++ ;
		}
		if (r == 1)
		{
			shapeAchieved++ ;
			shapeTotal++ ;
		}
		if (r == 2)
		{
			colorAchieved++ ;
			colorTotal++ ;
		}

		confusionMatrix[3]++ ;
		UpdateSessionData("TrueNegative", id) ;
    }

    public void BadMiss(int id, string t, int r)
    {
		type = t ;

        ResetMultiplier() ;

        confusionMatrix[2]++ ;
		UpdateConfusionScores() ;
		UpdateSessionData("FalseNegative", id) ;

		if (r == 0)
		{
			shapeAndColorTotal++ ;
		}
		if (r == 1)
		{
			shapeTotal++ ;
		}
		if (r == 2)
		{
			colorTotal++ ;
		}
    }

    public void GoodAvoidance()
    {
		UpdateSessionData("GoodAvoidance", -1) ;
    }

    public void BadAvoidance()
    {
        ResetMultiplier() ;
		UpdateSessionData("BadAvoidance", -1) ;
    }

	void UpdateSessionData(string _eventName, int _id)
	{
		if (maxScore > 0)
			achievement = score / maxScore ;
		Debug.Log("ACHIEVEMENT UPDATE BM : " + achievement);

		Dictionary<string, object> _event = new Dictionary<string, object>() ;
        _event.Add( "eventType", _eventName ) ;
        _event.Add( "id", _id ) ;
        _event.Add( "selectorCombination", selectorCombination ) ;
        _event.Add( "rule", rule ) ;
        _event.Add( "type", type ) ;
        _event.Add( "hand", hand ) ;
        _event.Add( "score", score ) ;
        _event.Add( "multiplier", multiplier ) ;
        _event.Add( "confusionMatrix", confusionMatrix ) ;
        _event.Add( "accuracy", accuracy ) ;
		_event.Add( "sensibility", sensibility ) ;
		_event.Add( "efficiency", efficiency ) ;
		_event.Add( "f1score", f1score ) ;
		_event.Add( "achievement", achievement ) ;
		_event.Add( "elapsedTime", audioSource.time / audioSource.clip.length ) ;

		if (shapeTotal > 0)
			_event.Add( "ruleShapeAchievement", shapeAchieved / shapeTotal ) ;
		else
			_event.Add( "ruleShapeAchievement", 0 ) ;
		if (colorTotal > 0)
			_event.Add( "ruleColorAchievement", colorAchieved / colorTotal ) ;
		else
			_event.Add( "ruleColorAchievement", 0 ) ;
		if (shapeAndColorTotal > 0)
			_event.Add( "ruleShapeAndColorAchievement", shapeAndColorAchieved / shapeAndColorTotal ) ;
		else
			_event.Add( "ruleShapeAndColorAchievement", 0 ) ;

        if(module != 0) dataManager.GetComponent<BeatMasterFirestoreDataManager>().UpdateCurrentData(_event) ;
	}

	public bool IsInSession()
	{
		return sessionHasBegun;
	}

	public void SetInteractors(bool raysEnabled)
	{
		leftRayInteractor.enabled = raysEnabled; 
		rightRayInteractor.enabled = raysEnabled ;

		leftSelectorCollider.SetActive(!raysEnabled) ;
		rightSelectorCollider.SetActive(!raysEnabled) ;

		neonBlueSquare.SetActive(!raysEnabled) ;
		neonRedCircle.SetActive(!raysEnabled) ;
	}
}
