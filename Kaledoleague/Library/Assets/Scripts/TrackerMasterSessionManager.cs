using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI ;
using TMPro;


public class TrackerMasterSessionManager : MonoBehaviour
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
	Color [] _colors = { Color.red, Color.blue, Color.green } ;
	int targetColorIndex = 0 ;

	private AudioSource audioSource;
	[SerializeField]
	private List<GameObject> gos ;
	public GameObject prefabSphere ;

	public GameObject leftHand,rightHand;
	public GameObject m_indicator;
	
	GameObject dataManager ;

	public GameObject previsionSelector ;
	public GameObject evaluationSelector ;
	GameObject previsionF1ScoreSlider ;
	GameObject previsionAchievementSlider ;
	GameObject evaluationF1ScoreSlider ;
	GameObject evaluationAAchievementSlider ;

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

	float previsionF1Score = .5f ;
	float previsionAchievement = .5f ;
	float evaluationF1Score = .5f ;
	float evaluationAchievement = .5f ;

	GameObject [] instructionsBoards ;

	public TextMeshProUGUI text ;

	bool sessionHasBegun = false ;

	private int GamePhase = 0 ;
	private bool ongoing = false ;
	private int score = 0 ;
	private float speed = 0.0f ;
	private float timeFollowing = 8.0f ;
	private float timeChoosing = 8.0f ;
	private bool isInside = false ;
	private bool isCroissant = true;
	private bool isLeftHand = true;

	private float achievement ;
	private int numberOfTries = 0 ;
	private int numberOfSuccess = 0 ;

	int stage = 1 ;
	int totalNumberOfTargets = 1 ;
	int totalNumberOfItems = 3 ;

	int module = 0 ;
	int level = 0 ;
	int colors = 2 ;
	int targets = 1 ;
	int items = 3 ;
	int initialspeed = 1 ;
	int speedincrement = 1 ;
	int doubletask = 0 ;
	int ascending = 0 ;
	int homogeneisationdelay = 0 ;
	int instructiondelay = 0 ;
	int selectorrule = 0 ;
	string track = "" ;

	int gp = 0 ;
	int fp = 0 ;
	int fn = 0 ;
	int gn = 0 ;

	int[] sequence;

	bool wasPlaying = false ;

	public int sessionDuration = 6 ;
	float startTime ;
	float elapsedTime = 0 ;

	bool hasRecorded = false ;

	float maxScore = 600.0f ;

	List < float > responseTime = new List < float > () ;

	void Start()
	{

		previsionF1Score = (m_EntSliderScore.value + 1) / 100.0f;
		previsionAchievement = (m_EntSliderAccomplissement.value + 1) / 100.0f;

		m_MainManager = GameObject.FindGameObjectWithTag("MainManager");

		if (m_MainManager == null)
		{
			Debug.LogError("MainManager introuvable");
		}
		else if (m_MainManager != null)
		{

			if (m_MainManager.GetComponent<BeatMasterGenerator>().m_EvaluationModeBM == true)
			{
				m_TitlePrevisionSelector.text = "Évaluation";
				m_TitleEvaluationSelector.text = "Évaluation";
			}
			else if (m_MainManager.GetComponent<BeatMasterGenerator>().m_EvaluationModeBM == false)
			{
				m_TitlePrevisionSelector.text = "Entraînement";
				m_TitleEvaluationSelector.text = "Entraînement";
			}

		}

		m_indicator.SetActive(false);
		gos = new List<GameObject>() ;
		dataManager = GameObject.Find("DataManager") ;
		//previsionSelector = GameObject.Find("PrevisionSelector") ;
		//evaluationSelector = GameObject.Find("EvaluationSelector") ;
		audioSource = gameObject.GetComponent<AudioSource>();

		instructionsBoards = GameObject.FindGameObjectsWithTag("InstructionBoard") ;


		if (GameObject.Find ("MainManager") != null)
		{
			GameObject initializer = GameObject.Find("MainManager") ;
			TrackingMasterGenerator tmg = initializer.GetComponent<TrackingMasterGenerator>() ;
			module = tmg.GetModule() ;
			level = tmg.GetLevel() ;
			track = tmg.GetTrack() ;
			colors = tmg.GetColors() ;
			targets = tmg.GetTargets() ;
			items = tmg.GetItems() ;
			initialspeed = tmg.GetInitialSpeed() ;
			speedincrement = tmg.GetSpeedIncrement() ;
			doubletask = tmg.GetDoubleTask() ;
			ascending = tmg.GetAscending() ;
			homogeneisationdelay = tmg.GetHomogeneisationDelay() ;
			instructiondelay = tmg.GetInstructionDelay() ;
			selectorrule = tmg.GetSelectorRule() ;

			Debug.Log("module : " + module);
			Debug.Log("level : " + level);
			Debug.Log("track : " + track);
			Debug.Log("colors : " + colors);
			Debug.Log("targets : " + targets);
			Debug.Log("items : " + items);
			Debug.Log("initialspeed : " + initialspeed);
			Debug.Log("speedincrement : " + speedincrement);
			Debug.Log("doubletask : " + doubletask);
			Debug.Log("homogeneisationdelay : " + homogeneisationdelay);
			Debug.Log("instructiondelay : " + instructiondelay);
			Debug.Log("selectorrule : " + selectorrule);
		}
		else if (GameObject.Find("MainManager") == null)
		{
			Debug.LogError("MainManger introuvable");
		}

		if (module == 0)
		{
			Debug.Log("Training Module detected, changing session duration to 2 minutes.");
			sessionDuration = 2;
		}
		else
		{
			Debug.Log("Normal Module detected, changing session duration to 6 minutes.");
			sessionDuration = 6;
		}

		AudioClip clip;

		if(module == 5 || level ==0)
        {
			clip = Resources.Load<AudioClip>("Songs/TMSongs/module1level1");

		}
		else
        {
			clip = Resources.Load<AudioClip>("Songs/TMSongs/module" + module + "level" + level);
		}

		audioSource.clip = clip ;
		
		InitPrevisionStep() ;
	}


	public bool WasPlaying()
	{
		return wasPlaying ;
	}

	public bool IsPlaying()
	{
		return isInside ;
	}

	public void SetWatchText(string message)
	{
		text.text = message ;	
	}

	void Update()
	{
		if (sessionHasBegun && (Time.time - startTime < (sessionDuration * 60)))
		{
			float remainingTime = sessionDuration * 60 - (Time.time - startTime) ;
			elapsedTime = (sessionDuration * 60.0f - remainingTime) / ( sessionDuration * 60.0f) ;
			SetWatchText("Score : " + score + "\nNiveau : " + stage + "\nTemps restant : " + Mathf.Floor(remainingTime/60).ToString("00") + "'" + Mathf.Floor(remainingTime % 60).ToString("00") + "''") ;
		}

		if (sessionHasBegun && !ongoing && (Time.time - startTime < (sessionDuration * 60)))
		{
			switch(GamePhase)
			{
				case 0 :
					StartCoroutine(prepareStage()) ;
					break ;
				case 1 : 
					StartCoroutine(followBalls()) ;
					break ;
				case 2 :
					StartCoroutine(chooseBalls()) ;
					break ;
				case 3 :
					StartCoroutine(resumeStage()) ;
					break ;
				default :
					break ;
			}
		}
		
		if (sessionHasBegun && (Time.time - startTime > (sessionDuration * 60 + 10.0f)) && !hasRecorded)
		{
			hasRecorded = true ;
			CleanGame() ;
			InitEvaluationStep() ;
			SetActiveInteractors(leftHand, true);
			SetActiveInteractors(rightHand, true);
		}
		
	}

	IEnumerator ExitSession()
	{
		previsionSelector.SetActive(false) ;
		evaluationSelector.SetActive(false) ;
		SetBoardsMessage("Chargement ...") ;

		GameObject SceneManager;
		SceneManager = GameObject.Find("MainManager");
		SceneManager.GetComponent<SceneManager>().GenerateMenu();

		yield return null;
	}

	void CleanGame()
	{
		foreach (var go in gos)
		{
			Destroy(go) ;
		}
		gos.Clear() ;
	}

	private void InitGame()
	{

		GamePhase = 0 ;
		audioSource.Play() ;
		foreach (var go in gos)
		{
			Destroy(go) ;
		}
		gos.Clear() ;
		initStats() ;
		startTime = Time.time ;
		if(module == 5) Random.InitState(42);
		sessionHasBegun = true ;
	}

	private void initStats()
	{
		score = 0 ;
		stage = 1 ;
		speed = initialspeed ;
		totalNumberOfItems = items ;
		totalNumberOfTargets = targets ;
		gp = 0 ;
		fp = 0 ;
		fn = 0 ;
		gn = 0 ;
	}

	private void clearGame()
	{
		GamePhase = 0 ;
		ongoing = false ;
		audioSource.Pause() ;
	}

	private IEnumerator prepareStage()
	{
		ongoing = true ;
		SetBoardsMessage("") ;

		if (colors != 2)
		{
			targetColorIndex = Random.Range(0, 3);
		}

		if(level != 1) isCroissant = Random.value > .5f;

		if(selectorrule == 1) isLeftHand = Random.value > .5f;
		
		if(level <= 2)
		{
			if(isCroissant) sequence = CreateAscendingSuccessiveSequence(totalNumberOfTargets);
			else sequence = CreateDescendingSuccessiveSequence(totalNumberOfTargets);
		}
		else
		{
			if(isCroissant) sequence = CreateAscendingSequence(totalNumberOfTargets);
			else sequence = CreateDescendingSequence(totalNumberOfTargets);
		}

		SetActiveInteractors(leftHand, true);
		SetActiveInteractors(rightHand, true);
		
		//We create the target balls
		for (int i = 0 ; i < totalNumberOfTargets ; i++)
		{
			Vector3 randomPosition = new Vector3(0.0f + Random.Range(-12.0f, 12.0f), Random.Range(0.5f, 2.0f), 0.0f + Random.Range(-12.0f, 12.0f)) ;
			GameObject go = Instantiate(prefabSphere, randomPosition, transform.rotation) ;
			
			go.GetComponent<OrbController>().SetOriginalColor(_colors[targetColorIndex]);
			go.GetComponent<OrbController>().SetTarget() ;
			if(module == 4) go.GetComponent<OrbController>().SetNumber(sequence[i]);
			gos.Add(go) ;
			Debug.Log("Created a target") ;
		}

		//We create the non-target balls
		for (int i = 1 ; i < colors; i++)
		{
			int targets = colors < 3 ? (totalNumberOfItems - totalNumberOfTargets) : totalNumberOfTargets ;
			int[] array;

			if(level <= 2)
			{
				if(isCroissant) array = CreateAscendingSuccessiveSequence(targets);
				else array = CreateDescendingSuccessiveSequence(targets);
			}
			else
			{
				if(isCroissant) array = CreateAscendingSequence(targets);
				else array = CreateDescendingSequence(targets);
			}

			for(int j = 0; j < targets; j++)
			{
				Vector3 randomPosition = new Vector3(0.0f + Random.Range(-12.0f, 12.0f), Random.Range(0.5f, 2.0f), 0.0f + Random.Range(-12.0f, 12.0f)) ;
				GameObject go = Instantiate(prefabSphere, randomPosition, transform.rotation) ;
			
				go.GetComponent<OrbController>().SetOriginalColor(_colors[(targetColorIndex + i) % colors]);
				if(module == 4) go.GetComponent<OrbController>().SetNumber(array[j]);
				gos.Add(go) ;
				Debug.Log("Created a non-target");
			}
		}

		yield return new WaitForSeconds(1) ;

		GamePhase++ ;
		ongoing = false ;
	}

	private IEnumerator followBalls()
	{
		ongoing = true ;

		string message = "Suis les capsules " ;
		switch(targetColorIndex)
		{
			case 0:
				message += "rouges";
				break;
			case 1:
				message += "bleues";
				break;
			case 2:
				message += "vertes";
				break;
		}

		if(module == 4)
		{
			if(isCroissant) message += " d'ordre croissant";
			else message += " d'ordre decroissant";
		}

		foreach (var go in gos)
		{
			go.GetComponent<OrbController>().SetSpeed(speed) ;
			go.GetComponent<OrbController>().BeginAnimation() ;
		}
		
		if (instructiondelay == 0 && homogeneisationdelay == 0)
		{
			SetBoardsMessage(message) ;
			yield return new WaitForSeconds(timeFollowing) ;
		}
		else
		{
			float timeBeforeHomogeneisation = (1 - .25f * homogeneisationdelay) * timeFollowing ;
			Debug.Log("Time before homogeneisation " + timeBeforeHomogeneisation) ;
			Debug.Log("Total " + timeFollowing) ;
			float timeBeforeInstruction = .25f * instructiondelay * timeFollowing ;
			float remainingTime = timeFollowing ;
			if (timeBeforeHomogeneisation > timeBeforeInstruction)
			{
				remainingTime = timeFollowing - timeBeforeHomogeneisation ;
				yield return new WaitForSeconds(timeBeforeInstruction) ;
				SetBoardsMessage(message) ;
				yield return new WaitForSeconds(timeBeforeHomogeneisation - timeBeforeInstruction) ;
				foreach (var go in gos)
				{
					go.GetComponent<OrbController>().SetColor(Color.gray);
					go.GetComponent<OrbController>().ShowText(false);
				}
				yield return new WaitForSeconds(remainingTime) ;

			}
			else
			{
				remainingTime = timeFollowing - timeBeforeInstruction ;
				yield return new WaitForSeconds(timeBeforeHomogeneisation) ;
				foreach (var go in gos)
				{
					go.GetComponent<OrbController>().SetColor(Color.gray);
					go.GetComponent<OrbController>().ShowText(false);
				}
				yield return new WaitForSeconds(timeBeforeInstruction - timeBeforeHomogeneisation) ;
				SetBoardsMessage(message) ;
				yield return new WaitForSeconds(remainingTime) ;
			}

		}
		GamePhase++ ;
		ongoing = false ;
	}

	private IEnumerator chooseBalls()
	{
		Debug.Log("IS CHOSEN - 02");

		ongoing = true ;

		if(selectorrule == 1)
		{
			SetActiveInteractors(leftHand, isLeftHand);
			SetActiveInteractors(rightHand, !isLeftHand);
			if(isLeftHand) VibrateController(UnityEngine.XR.InputDeviceCharacteristics.Left);
			else VibrateController(UnityEngine.XR.InputDeviceCharacteristics.Right);
		}

		foreach (var go in gos)
		{
			var controller = go.GetComponent<OrbController>();

			controller.StopAnimation() ;
			controller.SetColor(Color.gray);
			controller.ShowText(false);
			controller.SetChosable(true) ;
		}

		bool hasAnError = false ;
		bool hasEveryTarget = false ;
		float _startTime = Time.realtimeSinceStartup  ;

		while ( !hasAnError && !hasEveryTarget && (Time.realtimeSinceStartup - _startTime < timeChoosing) )
		{
			Debug.Log("IS CHOSEN - 03");

			int goodAnswers = 0;
			int wrongAnswers = 0 ;

			foreach (var go in gos)
			{
				Debug.Log("IS CHOSEN - 04");
				var controller = go.GetComponent<OrbController>();

				if(controller.IsChosen())
				{
					Debug.Log("IS CHOSEN - 01");

					if ( controller.IsATarget())
					{
						if(module == 4 && sequence[goodAnswers] != controller.number)
						{
							wrongAnswers++;
						}
						else
						{
							goodAnswers++;
							//yield return new WaitForSeconds(1.5f);
						}
					}
					else if ( !controller.IsATarget())
					{
						wrongAnswers++;
						//yield return new WaitForSeconds(1.5f);
					}
				}
			}
			hasAnError = (wrongAnswers > 0) ;
			hasEveryTarget = (goodAnswers == totalNumberOfTargets) ;

			Debug.Log("hasEveryTarget : " + hasEveryTarget);

			yield return new WaitForSeconds(0.05f) ;
		}

		GamePhase++ ;
		ongoing = false ;
		yield return new WaitForSeconds(0.1f) ;
	}

	private IEnumerator resumeStage()
	{
		ongoing = true ;

		int localScore = 0 ;
		bool won = true ;
		int a = 0 ;
		int b = 0 ;
		int c = 0 ;
		int d = 0 ;

		if (responseTime.Count > 0)
		{
			responseTime.Clear() ;
			responseTime = new List < float > () ;
		}

		foreach (var go in gos)
		{
			var controller = go.GetComponent<OrbController>();

			controller.SetChosable(false) ;
			if ( (controller.IsATarget() && controller.IsChosen() ) || (!controller.IsATarget() && !controller.IsChosen() ) )
			{
				numberOfSuccess++ ;
				numberOfTries++ ;
				localScore++ ;
				
			}
			else
			{
				numberOfTries++ ;
				won = false ;
			}

			achievement = (1.0f * numberOfSuccess) / (1.0f * numberOfTries) ;

			if ( controller.IsATarget() && controller.IsChosen() )
			{
				a++ ;
				responseTime.Add(controller.GetResponseTime()) ;
			}
			if (controller.IsATarget() && !controller.IsChosen() )
			{
				b++ ;
			}
			if ( !controller.IsATarget() && controller.IsChosen() )
			{
				c++ ;
				if (responseTime.Count > 0)
					responseTime[0] = controller.GetResponseTime() ;
				else
					responseTime.Add(controller.GetResponseTime()) ;

			}
			if ( !controller.IsATarget() && !controller.IsChosen() )
				d++ ;
		}

		if (responseTime.Count > 0 && b == 0)
			responseTime.Sort() ;
		else if (responseTime.Count > 0)
		{
			float f = responseTime[0] ;
			responseTime.Clear() ;
			responseTime = new List <float> () ;
			responseTime.Add(f) ;
		}

		Debug.Log("RESPONSE TIME LENGTH: " + responseTime.Count) ;

		for (int i = 0 ; i < responseTime.Count ; i++)
		{
			Debug.Log("RESPONSE TIME : " + i + "/ " + responseTime[i]) ;
		}

		gp += a ;
		fn += b ;
		fp += c ;
		gn += d ;

		if (localScore < 0)
			  localScore = 0 ;
			
		score += localScore ;

		if (level > 1)
			wasPlaying = true ;

		float accuracy = 0 ;
		if (gp + fp > 0)
		{
			accuracy = (1.0f * gp) / (gp + fp) ;
		}

		float sensibility = 0 ;
		if (gp + fn > 0)
		{
			sensibility = (1.0f * gp) / (gp + fn) ;
		}

		float efficiency = 0 ;
		if (gp + fn + fp + gn > 0)
		{
			efficiency = (1.0f * (gp + gn)) / (gp + + fp + fn + gn) ;
		}

		float f1score = 0 ;
		if (accuracy + sensibility > 0)
		{
			f1score = 2.0f * accuracy * sensibility / (accuracy + sensibility) ;
		}

		float remainingTime = sessionDuration*60 - (Time.time - startTime) ;
		SetWatchText("Score : " + score + "\nNiveau : " + stage + "\nTemps restant : " + Mathf.Floor(remainingTime/60).ToString("00") + "'" + Mathf.Floor(remainingTime % 60).ToString("00") + "''") ;

		List <object> rTime = new List<object>();
		for (int i = 0 ; i < responseTime.Count ; i++)
			rTime.Add(responseTime[i]) ;

		Debug.Log("ACHIEVEMENT UPDATE TM : " + achievement);

		Dictionary<string, object> _event = new Dictionary<string, object>() ;
		_event.Add( "score", score ) ;
		_event.Add( "stage", stage ) ;
		_event.Add( "totalNumberOfTargets", totalNumberOfTargets ) ;
		_event.Add( "totalNumberOfItems", totalNumberOfItems ) ;
		_event.Add( "speed", speed ) ;
		_event.Add( "confusionMatrix", new List<object>() {gp, fp, fn, gn} ) ;
		_event.Add( "responseTime", rTime) ;
		_event.Add( "accuracy", accuracy ) ;
		_event.Add( "efficiency", efficiency ) ;
		_event.Add( "sensibility", sensibility ) ;
		_event.Add( "f1score", f1score ) ;
		_event.Add( "achievement", achievement ) ;
		_event.Add( "elapsedTime", elapsedTime) ;
		_event.Add( "won", won) ;
		if(selectorrule == 1){
			_event.Add( "leftHand", isLeftHand);
			//_event.Add( "rightHand", !isLeftHand);
		}

		if(module != 0) dataManager.GetComponent<TrackerMasterFirestoreManager>().UpdateCurrentData(_event) ;

		if (won)
		{
			

			stage++ ;

			if (m_encouragementManager.m_Encouragement == true)
			{
				Debug.Log("Passage");
				//yield return new WaitForSeconds(0.5f);
				m_audiosourceEncouragement.PlayOneShot(m_goodEncouragement[Random.Range(0, m_goodEncouragement.Count)], 1f);
			}

			if (stage % 8 == 4)
				totalNumberOfTargets++ ;
			else if (stage % 2 == 0)
				totalNumberOfItems++ ;
			speed *= (1.0f + speedincrement/10.0f) ;
		}
		else
        {
			if (m_encouragementManager.m_Encouragement == true)
			{
				Debug.Log("Passage");
				//yield return new WaitForSeconds(0.5f);
				m_audiosourceEncouragement.PlayOneShot(m_badEncouragement[Random.Range(0, m_badEncouragement.Count)], 1f);
			}
		}

		yield return new WaitForSeconds(1.0f) ;

		foreach (var go in gos)
		{
			Destroy(go) ;
		}
		gos.Clear() ;

		GamePhase = 0 ;
		ongoing = false ;
	}

	void SetBoardsMessage(string message)
	{
		foreach( var go in instructionsBoards){
			go.GetComponent<TextMeshProUGUI>().text = message ;

			//go.GetComponent<TextMeshProUGUI>().color = _colors[targetColorIndex];
		}
	}

	void InitPrevisionStep()
	{
		previsionSelector.SetActive(true) ;
		evaluationSelector.SetActive(false) ;
	}

	void InitSession()
	{
		m_indicator.SetActive(true);

		previsionSelector.SetActive(false) ;
		evaluationSelector.SetActive(false) ;
		InitGame() ;
	}

	void InitEvaluationStep()
	{
		m_indicator.SetActive(false);

		previsionSelector.SetActive(false) ;
		evaluationSelector.SetActive(true) ;
	}

	/*public void setPrevisionF1Score(float value)
	{
		previsionF1Score = value / 100.0f ;
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

	public void RegisterPrevisions()
	{
		previsionF1Score = (m_EntSliderScore.value+1) / 100.0f;
		previsionAchievement = (m_EntSliderAccomplissement.value+1) / 100.0f;

		if (module != 0) dataManager.GetComponent<TrackerMasterFirestoreManager>().InitParameters(previsionF1Score, previsionAchievement) ;
		InitSession() ;
	}

	public void RegisterEvaluations()
	{
		evaluationF1Score = (m_EvalSliderScore.value+1) / 100.0f;
		evaluationAchievement = (m_EvalSliderAccomplissement.value+1) / 100.0f;

		if (module != 0) dataManager.GetComponent<TrackerMasterFirestoreManager>().RecordSession(evaluationF1Score, evaluationAchievement) ;
		StartCoroutine(ExitSession()) ;
	}


	private int[] CreateAscendingSuccessiveSequence(int size, int max = 10)
	{
		int start = Random.Range(1, max + 1);
		int[] tab = new int[size];

		tab[0] = start;
		for(int i = 1; i < tab.Length; i++)
		{
			tab[i] = ++start; 
		}

		return tab;
	}

	private int[] CreateDescendingSuccessiveSequence(int size, int max = 10)
	{
		int[] tab = CreateAscendingSuccessiveSequence(size, max);
		System.Array.Reverse(tab);

		return tab;
	}

	private int[] CreateAscendingSequence(int size, int max = 100)
	{
		int[] tab = new int[size];
		int split = max / size;

		for(int i = 0; i < size; i++)
		{
			tab[i] = Random.Range(i * split + 1, (i+1) * split);
		}

		return tab;
	}

	private int[] CreateDescendingSequence(int size, int max = 100)
	{
		int[] tab = CreateAscendingSequence(size, max);
		System.Array.Reverse(tab);

		return tab;
	}

	private void VibrateController(UnityEngine.XR.InputDeviceCharacteristics characteristic)
	{
		List<UnityEngine.XR.InputDevice> devices = new List<UnityEngine.XR.InputDevice>(); 

		UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(characteristic, devices);

		foreach (var device in devices)
		{
		    UnityEngine.XR.HapticCapabilities capabilities;
		    if (device.TryGetHapticCapabilities(out capabilities))
		    {
		            if (capabilities.supportsImpulse)
		            {
		                uint channel = 0;
		                float amplitude = 0.5f;
		                float duration = 0.5f;
		                device.SendHapticImpulse(channel, amplitude, duration);
		            }
		    }
		}
	}

	private void SetActiveInteractors(GameObject hand, bool active)
	{
		hand.GetComponent<UnityEngine.XR.Interaction.Toolkit.XRRayInteractor>().enabled = active;
		hand.GetComponent<UnityEngine.XR.Interaction.Toolkit.XRInteractorLineVisual>().enabled = active;
	}
}
