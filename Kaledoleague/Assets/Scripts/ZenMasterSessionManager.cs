using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI ;

public class ZenMasterSessionManager : MonoBehaviour
{
	GameObject dataManager ;
	GameObject previsionSelector ;
	GameObject evaluationSelector ;
	GameObject bluetoothSelector;
	public GameObject glowingLight;
	float previsionF1Score = .5f ;
	float previsionAchievement = .5f ;
	float evaluationF1Score = .5f ;
	float evaluationAchievement = .5f ;
	private float achievement ;
	bool sessionHasBegun = false ;
	private int GamePhase = 0 ;
	private bool ongoing = false ;
	public int sessionDuration = 3 ;
	float startTime ;
	bool hasRecorded = false ;

    // Start is called before the first frame update
    void Start()
    {
        dataManager = GameObject.Find("DataManager") ;
		previsionSelector = GameObject.Find("PrevisionSelector") ;
		evaluationSelector = GameObject.Find("EvaluationSelector") ;
		bluetoothSelector = GameObject.Find("BluetoothSelector") ;

		PreInitStep();
		//InitPrevisionStep();
    }

    // Update is called once per frame
    void Update()
    {
		if (sessionHasBegun && (Time.time - startTime > (sessionDuration * 60 + 10.0f)) && !hasRecorded)
		{
			hasRecorded = true ;
			InitEvaluationStep() ;
			glowingLight.GetComponent<GuideBreathing>().Stop();
		}
    }

	void PreInitStep()
	{
		bluetoothSelector.SetActive(true);
		previsionSelector.SetActive(false) ;
		evaluationSelector.SetActive(false) ;
	}

	public void InitPrevisionStep()
	{
		bluetoothSelector.SetActive(false);
		previsionSelector.SetActive(true) ;
		evaluationSelector.SetActive(false) ;
	}

	void InitEvaluationStep()
	{
		previsionSelector.SetActive(false) ;
		evaluationSelector.SetActive(true) ;
	}

	public void setPrevisionF1Score(float value)
	{
		previsionF1Score = value / 100.0f ;
	}

	public void setPrevisionAchievement(float value)
	{
		previsionAchievement = value / 100.0f ;
	}

	public void setEvaluationF1Score(float value)
	{
		evaluationF1Score = value / 100.0f ;
	}

	public void setEvaluationAchievement(float value)
	{
		evaluationAchievement = value / 100.0f ;
	}

	public void RegisterPrevisions()
	{
		dataManager.GetComponent<ZenMasterFirestoreDataManager>().InitParameters(previsionF1Score, previsionAchievement) ;
		InitSession() ;
	}

	void InitSession()
	{
		previsionSelector.SetActive(false) ;
		evaluationSelector.SetActive(false) ;
		InitGame() ;
	}

	private void InitGame()
	{
		GamePhase = 0 ;
		startTime = Time.time ;
		sessionHasBegun = true ;
		glowingLight.GetComponent<GuideBreathing>().Play();
	}

	public void RegisterEvaluations()
	{
		dataManager.GetComponent<ZenMasterFirestoreDataManager>().RecordSession(evaluationF1Score, evaluationAchievement) ;
		StartCoroutine(ExitSession()) ;
	}

	IEnumerator ExitSession()
	{
		previsionSelector.SetActive(false) ;
		evaluationSelector.SetActive(false) ;
		yield return new WaitForSeconds(2.0f) ;
		GameObject.Find("MainManager").GetComponent<SceneManager>().GenerateMenu() ;
	}

	public void IncPrevisionF1Score()
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
	}

	public void IncEvaluationF1Score()
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
	}
}
