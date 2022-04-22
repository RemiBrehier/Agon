using System.Collections ;
using System.Collections.Generic ;
using UnityEngine ;
using TMPro ;

public class BeatMasterTargetBehaviour : MonoBehaviour
{
    TextMeshPro text ;
	GameObject soundManager ;
	GameObject sessionManager ;

	public string shape = "circle" ;
	public string color = "blue" ;
	int rule = 0 ;
	int id = 0 ; 

	public void SetTargetProperties(string _shape, string _color, int _rule, int _id = 0)
	{
		shape = _shape ;
		color = _color ;
		rule = _rule ;
		id = _id ;
	}


    void Start()
    {
		soundManager = GameObject.Find("[MANAGER]/SoundManager") ;
		sessionManager = GameObject.Find("[MANAGER]/SessionManager") ;
    }

	void OnCollisionEnter(Collision collision)
    {
		//Debug.Log("Target collided with " + collision.gameObject.name) ;
		
		if (collision.gameObject.name.Contains("Selector"))
		{
			string selectorColor = collision.gameObject.GetComponent<SelectorBehaviour>().GetColor() ;
			string selectorShape = collision.gameObject.GetComponent<SelectorBehaviour>().GetShape() ;
			//Debug.Log("Object with shape and color = (" + shape + ", " + color + ") was destroyed by object with shape and color = (" + selectorShape + ", " + selectorColor + ")") ; 

			bool left = collision.gameObject.name.Contains("Left") ;

			int result = checkResultForSelectorCollision(selectorColor, selectorShape) ;
			if (result == 0)
				OnTruePositive(left) ;
			else if (result == 1)
				OnFalsePositive(left) ;

			Destroy(gameObject) ;
		}
		else if (collision.gameObject.name.Contains("Player"))
		{
			//Debug.Log("Object with shape and color = (" + shape + ", " + color + ") was destroyed by player") ; 

			int result = checkResultForPlayerCollision() ;
			if (result == 0)
				OnTrueNegative() ;
			else if (result == 1)
				OnFalseNegative() ;

			Destroy(gameObject) ;
		}	
    }

	int checkResultForSelectorCollision(string selectorColor, string selectorShape)
	{
		if (rule == 0)
		{
			if (selectorColor == color && selectorShape == shape)
				return 0 ;
		}
		else if (rule == 1)
		{
			if (selectorShape == shape)
				return 0 ;
		}
		else if (rule == 2)
		{
			if (selectorColor == color)
				return 0 ;
		}
		return 1 ;
	}

	int checkResultForPlayerCollision()
	{
		GameObject [] gos = GameObject.FindGameObjectsWithTag("Selector") ;

		if (rule == 0)
		{
			if ( color == gos[0].GetComponent<SelectorBehaviour>().GetColor() && shape == gos[0].GetComponent<SelectorBehaviour>().GetShape())
				return 1 ;
			else if ( color == gos[1].GetComponent<SelectorBehaviour>().GetColor() && shape == gos[1].GetComponent<SelectorBehaviour>().GetShape())
				return 1 ;
		}
		else if (rule == 1)
		{
			if ( shape == gos[0].GetComponent<SelectorBehaviour>().GetShape() )
				return 1 ;
			else if ( shape == gos[1].GetComponent<SelectorBehaviour>().GetShape() )
				return 1 ;
		}
		else if (rule == 2)
		{
			if ( color == gos[0].GetComponent<SelectorBehaviour>().GetColor() )
				return 1 ;
			else if ( color == gos[1].GetComponent<SelectorBehaviour>().GetColor() )
				return 1 ;
		}
		return 0 ;
	}

	void OnTruePositive(bool left)
	{
		soundManager.GetComponent<BeatMasterSoundResults>().PlayTruePositive() ;
		int _left = 0 ;
		if (!left)
			_left = 1 ;
		sessionManager.GetComponent<BeatMasterSessionManager>().GoodHit(id, color + " " + shape, _left, rule) ;
	}

	void OnFalsePositive(bool left)
	{
		soundManager.GetComponent<BeatMasterSoundResults>().PlayFalsePositive() ;
		sessionManager.GetComponent<BeatMasterSessionManager>().ResetMultiplier() ;
		int _left = 0 ;
		if (!left)
			_left = 1 ;
		sessionManager.GetComponent<BeatMasterSessionManager>().BadHit(id, color + " " + shape, _left, rule) ;

	}

	void OnFalseNegative()
	{
		soundManager.GetComponent<BeatMasterSoundResults>().PlayFalseNegative() ;
		sessionManager.GetComponent<BeatMasterSessionManager>().ResetMultiplier() ;
		sessionManager.GetComponent<BeatMasterSessionManager>().BadMiss(id, color + " " + shape, rule) ;
	}

	void OnTrueNegative()
	{
		soundManager.GetComponent<BeatMasterSoundResults>().PlayTrueNegative() ;
		sessionManager.GetComponent<BeatMasterSessionManager>().IncreaseScore() ;
		sessionManager.GetComponent<BeatMasterSessionManager>().GoodMiss(id, color + " " + shape, rule) ;
	}
}
