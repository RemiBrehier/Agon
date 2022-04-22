using System.Collections;
using System.Collections.Generic;
using UnityEngine ;
using UnityEngine.UI ;

public class BeatMasterGenerator : MonoBehaviour
{
	public TextAsset loadedJsonFile;

	public bool m_EvaluationModeBM = false;

	public int songIndex = 0 ;
	public int elements = 2 ;
	public int colors = 2 ;
	public int hFov = 45 ;
	public int vFov = 45 ;
	public int speed = 0 ;
	public int obstacles = 0 ;
	public string trackname = "" ;
	public int bpm = 0 ;
	public int rulesChanging = 0 ;
	public int selectorsChanging = 0 ;
	public int module = 1 ;
	public int level = 1 ;

	public int Index;

	BMItems bmItem ;
	BMItem currentItem;

	// Start is called before the first frame update
	void Start()
	{
		loadedJsonFile = Resources.Load<TextAsset>("GameGenerators/BMGenerator");

		//TextAsset loadedJsonFile = Resources.Load<TextAsset>("GameGenerators/BMGenerator");    
		bmItem = JsonUtility.FromJson<BMItems>(loadedJsonFile.text);	
		Debug.Log("BM INFOS - Loaded " + bmItem.items.Length + " levels") ;
		//assignCurrentParameters() ;
	}

	void assignCurrentParameters()
	{
		if(module == 1)
        {
			currentItem = bmItem.items[level-1];
			songIndex = level-1;
			Index = level-1;


		}

		else if(module == 2)
        {
			currentItem = bmItem.items[level+5];
			songIndex = level+5;
			Index = level+5;

		}

		else if (module == 3)
		{
			currentItem = bmItem.items[level + 11];
			songIndex = level+11;
			Index = level+11;

		}

		else if (module == 4)
		{
			currentItem = bmItem.items[level + 17];
			songIndex = level + 17;
			Index = level+17;

		}

		trackname = currentItem.track ;
		bpm = currentItem.bpm ;
		elements = currentItem.elements ;
		colors = currentItem.colors ;
		hFov = currentItem.hFov ;
		vFov = currentItem.vFov ;
		speed = currentItem.speed ;
		obstacles = currentItem.obstacles ;
		rulesChanging = currentItem.changeRule ;
		selectorsChanging = currentItem.changeSelector ;
	}

	public void SetModule(int BMValueModule) {
		module = BMValueModule;
		Debug.Log("Module : " + module + ", level : " + level) ;
		assignCurrentParameters() ;
	}

	public void SetLevel(int BMValueLevel) {
		level = BMValueLevel;
		Debug.Log("Module : " + module + ", level : " + level) ;
		assignCurrentParameters() ;
	}

	public void EvaluationMode(int value)
	{
		level = value;
		module = 4;
		Debug.Log("Module : " + module + ", level : " + level);
		assignCurrentParameters();
	}

	public void TrainingMode()
	{
		module = 1;
		level = 1;
		Debug.Log("POURQUOI !!");
		assignCurrentParameters() ;
	}



	public string GetTrackName()
	{
		return trackname ;
	}

	public int GetSong()
	{
		return songIndex ;
	}

	public int GetElements()
	{
		return elements ;
	}

	public int GetColors()
	{
		return colors ;
	}

	public int GetHFov()
	{
		return hFov ;
	}

	public int GetVFov()
	{
		return vFov ;
	}
	
	public int GetSpeed()
	{
		return speed ;
	}
	
	public int GetObstacles()
	{
		return obstacles ;
	}

	public int GetRuleChanging()
	{
		return rulesChanging ;
	}
	
	public int GetSelectorChanging()
	{
		return selectorsChanging ;
	}

	public int GetModule()
	{
		return module ;
	}

	public int GetLevel()
	{
		return level ;
	}

	public int GetBPM()
	{
		return bpm ;
	}

	public void ResetSelection()
	{
		module = 1;
		level = 1;
	}
}


[System.Serializable]
public class BMItems
{
	public BMItem [] items  ;
}

[System.Serializable]
public class BMItem
{
	public int module ;
	public int level ;
	public int bpm ;
	public int hFov ;
	public int vFov ;
	public int speed ;
	public int elements ;
	public int colors ;
	public int obstacles ;
	public string track ;
	public int changeRule ;
	public int changeSelector ;
}