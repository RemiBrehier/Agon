using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorBehaviour : MonoBehaviour
{
	public string color ;
	public string shape ;

	public string GetColor()
	{
		return color ;
	}

	public string GetShape()
	{
		return shape ;
	}

	public void SetColorAndShape(string _color, string _shape)
	{
		shape = _shape ;
		color = _color ;
	}
}
