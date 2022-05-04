using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZenMasterGenerator : MonoBehaviour
{
	private int module = 1 ;
	private int level = 1 ;

	ZMItems zmItem ;
	ZMItem currentItem ;

	// Start is called before the first frame update
	void Start()
	{
		/*
		TextAsset loadedJsonFile = Resources.Load<TextAsset>("GameGenerators/ZMGenerator");    
		zmItem = JsonUtility.FromJson<ZMItems>(loadedJsonFile.text);	
		Debug.Log("Loaded " + bmItem.items.Length + " levels") ;
		assignCurrentParameters() ;
		*/
	}

	void assignCurrentParameters()
	{
		/*
		currentItem = zmItem.items[module * 4 + (level - 1)] ;
		Debug.Log("Level " + (module * 6 + (level - 1))) ;
		*/
	}

	public void SetModule(int value) {
		module = value /*+ 1*/;
		Debug.Log("Module : " + module + ", level : " + level) ;
		assignCurrentParameters() ;
	}

	public void SetLevel(int value) {
		level = value /*+ 1*/ ;
		Debug.Log("Module : " + module + ", level : " + level) ;
		assignCurrentParameters() ;
	}

	public void EvaluationMode()
	{
		level = module;
		module = 4;
		Debug.Log("Module : " + module + ", level : " + level) ;
		assignCurrentParameters() ;
	}

	public void TrainingMode()
	{
		module = 0;
		level = 1;
		Debug.Log("Module : " + module + ", level : " + level) ;
		assignCurrentParameters() ;
	}

	public int GetModule()
	{
		return module ;
	}

	public int GetLevel()
	{
		return level ;
	}

	public void ResetSelection()
	{
		module = 1;
		level = 1;
	}
}

[System.Serializable]
public class ZMItems
{
	public ZMItem [] items  ;
}

[System.Serializable]
public class ZMItem
{
	public int module ;
	public int level ;
}