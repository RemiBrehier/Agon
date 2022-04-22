using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatMasterSoundGameEvents : MonoBehaviour
{

	public AudioSource ruleChanged ;
	public AudioSource selectorChanged ;

	public void PlayRuleChangedEvent()
	{
		ruleChanged.Play() ;
	}

	public void PlaySelectorChangedEvent()
	{
		selectorChanged.Play() ;
	}
}
