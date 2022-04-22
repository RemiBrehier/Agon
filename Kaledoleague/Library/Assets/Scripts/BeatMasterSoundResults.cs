using System.Collections ;
using System.Collections.Generic ;
using UnityEngine ;

public class BeatMasterSoundResults : MonoBehaviour
{
	public AudioSource TP ;
	public AudioSource FP ;
	public AudioSource FN ;
	public AudioSource TN ;

	public AudioSource AC ;

	public void PlayTruePositive()
	{
		TP.Play() ;
	}

	public void PlayFalsePositive()
	{
		FP.Play() ;
	}

	public void PlayFalseNegative()
	{
		FN.Play() ;
	}

	public void PlayTrueNegative()
	{
		//TN.Play() ;
	}

	public void PlayObstacleCollided()
	{
		AC.Play() ;
	}

}
