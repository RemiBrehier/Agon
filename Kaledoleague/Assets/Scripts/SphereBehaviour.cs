using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBehaviour : MonoBehaviour
{
	float heartRate = 0 ;
	float nHR = 0 ;
	List < float > heartRates = new List < float > () ;
	float hrv = 0 ;
	float nHrv = 0 ;

	void Start()
	{
		DisplayResults() ;
	}


	public void GetHeartRate(double data)
	{
		heartRate = (float) data ;
		ComputeValues() ;
		
	}

	void ComputeValues()
	{
		heartRates.Add(heartRate) ;

		nHR = (heartRate - 30) / (130.0f - 30.0f) ;
		if (nHR > 1)
			nHR = 1 ;
		if (nHR < 0)
			nHR = 0 ;


		if (heartRates.Count > 60)
			heartRates.RemoveAt(0) ;

		float sum = 0 ;
		for (int i = 1 ; i < heartRates.Count ; i++)
		{
			float ibiN = 60.0f / heartRates[i], ibiN_1 = 60.0f / heartRates[i-1] ;
			sum += ( ibiN - ibiN_1 ) * ( ibiN - ibiN_1 ) ;
		}

		hrv = Mathf.Sqrt(sum / (heartRates.Count - 1)) ;
		nHrv = Mathf.Log(25 * (1 + hrv)) / 10.0f ;
		if (nHrv > 1)
			nHrv = 1 ;

		DisplayResults() ;
	}

	void DisplayResults()
	{
		Debug.Log(nHR + ", " + nHrv) ;
		transform.localScale = new Vector3(10 * nHrv, 10 * nHrv, 10 * nHrv ) ;
		GetComponent<Renderer>().material.SetColor("_Color", new Color(nHR, 0.0f, nHR, 1.0f )) ;
	}

}
