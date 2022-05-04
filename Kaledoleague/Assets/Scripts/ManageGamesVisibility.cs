using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageGamesVisibility : MonoBehaviour
{
	public GameObject [] games ;

	int index = 0 ;
    // Start is called before the first frame update
    void Start()
    {
		foreach (GameObject go in games)
		{
			go.SetActive(false) ;			
		}
		games[0].SetActive(true) ;
    }

    public void Backward()
    {
		index-- ;
		if (index < 0)
			index = games.Length - 1 ;
		SetGameVisible() ;
    }

	public void Forward()
    {
		index = (index + 1) % games.Length ;
		SetGameVisible() ;
    }

	void SetGameVisible()
	{
		Debug.Log("New index is " + index) ;
		foreach (GameObject go in games)
		{
			go.SetActive(false) ;
		}
		games[index].SetActive(true) ;
	}
}
