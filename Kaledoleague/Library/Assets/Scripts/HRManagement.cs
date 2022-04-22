using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro ;

public class HRManagement : MonoBehaviour
{
	float heartrate ;
	List < float > heartrates = new List < float >() ;
	float hrv = 0 ;
	float hrNorm = 0 ;
	float hrvNorm = 0 ;

    public TextMeshProUGUI text ;
	public GameObject crystal;


    // Start is called before the first frame update
    void Start()
    {
		for (int i = 0 ; i < 60 ; i++ )
		{
			heartrates.Add(60.0f) ;
		}

		float offset = (.5f + hrvNorm * 3);
		if(crystal) crystal.transform.localScale = new Vector3(offset, offset, offset);
    }

    // Update is called once per frame
    void UpdateDisplay()
    {
		SetWatchText("HR = " + heartrate + " BPM\nHRV = " + (hrvNorm * 100.0f).ToString("F2") + " %") ;

		GameObject [] gos = GameObject.FindGameObjectsWithTag("Crystal") ;
		foreach (var go in gos)
		{
			go.GetComponent<CrystalShine>().SetLumen(hrvNorm) ;
		}

		float offset = (.5f + hrvNorm * 3);
		if(crystal) crystal.transform.localScale = new Vector3(offset, offset, offset);
    }

	void SetWatchText(string message)
	{
		text.text = message ;
	}

	public void GetHRValue(double value)
	{
		heartrate = (float) value ;
		Debug.Log("Got " + value) ;
		heartrates.Add(heartrate) ;
		heartrates.RemoveAt(0) ;
		UpdateData() ;
	}

	void UpdateData()
	{
		float newHRNorm = (heartrate - 30.0f) / 100.0f ;
		if (newHRNorm > 1)
			newHRNorm = 1 ;
		if (newHRNorm < 0)
			newHRNorm = 0 ;

		hrNorm = .9f * hrNorm + .1f * newHRNorm ;

		float newHRV = 0 ;
		for (int i = 0 ; i < heartrates.Count - 1 ; i++)
		{
			newHRV += (60000 / heartrates[i] - 60000 / heartrates[i+1]) * (60000 / heartrates[i] - 60000 / heartrates[i+1]) ; 
		}
		newHRV = Mathf.Sqrt(newHRV / (heartrates.Count - 1) ) ;

		float newNormHRV = (Mathf.Log(1 + newHRV) * 15)/100.0f ;
		if (newNormHRV > 1)
			newNormHRV = 1 ;
		if (newNormHRV < 0)
			newNormHRV = 0 ;

		hrvNorm = .9f * hrvNorm + .1f * newNormHRV ;

		Debug.Log("HR = " + heartrate) ;
		Debug.Log("HRV = " + newHRV) ;

		Debug.Log("HR normed= " + hrNorm) ;
		Debug.Log("HRV normed = " + hrvNorm) ;

		UpdateDisplay() ;
	}
}
