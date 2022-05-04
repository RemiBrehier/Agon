using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingMasterGenerator : MonoBehaviour
{
	public TextAsset loadedJsonFile;
	public bool m_EvaludationModeTM = false;

	public int module = 1;
	public int level = 1 ;
	public string track = "Starcade, Blue Navy";
	public int colors = 2 ;
	public int targets = 2;
	public int items = 5;
	public int initialspeed  = 1;
	public int speedincrement = 1;
	public int doubletask = 0;
	public int ascending = 0;
	public int homogeneisationdelay = 0;
	public int instructiondelay = 0;
	public int selectorrule = 0;

	public int Index;

	TMItems tmItem ;
	TMItem currentItem ;

    void Start()
    {
		loadedJsonFile = Resources.Load<TextAsset>("GameGenerators/TMGenerator");

		//TextAsset loadedJsonFile = Resources.Load<TextAsset>("GameGenerators/TMGenerator");    
		tmItem = JsonUtility.FromJson<TMItems>(loadedJsonFile.text);	
        Debug.Log("TM INFOS - Loaded " + tmItem.items.Length + " levels") ;
        //assignCurrentParameters() ;

    }

	void assignCurrentParameters()
	{

		if(module == 1)
        {
			currentItem = tmItem.items[level-1];
			Index = level - 1;
			Debug.Log("ITEM : " + level);
		}
		if(module == 2)
        {
			currentItem = tmItem.items[level+3];
			Index = level +3;

		}
		if (module == 3)
		{
			currentItem = tmItem.items[level+7];
			Index = level + 7;


		}
		if (module == 4)
		{
			currentItem = tmItem.items[level+11];
			Index = level + 11;


		}

		if (module == 5)
		{
			currentItem = tmItem.items[16];

		}

		track = currentItem.track ;
		colors = currentItem.colors ;
		targets = currentItem.targets ;
		items = currentItem.items ;
		initialspeed = currentItem.initialspeed ;
		speedincrement = currentItem.speedincrement ;
		doubletask = currentItem.doubletask ;
		ascending = currentItem.ascending ;
		homogeneisationdelay = currentItem.homogeneisationdelay ;
		instructiondelay = currentItem.instructiondelay ;
		selectorrule = currentItem.selectorrule ;
	}


	public void SetModule(int TMValueModule) 
	{
		module = TMValueModule;
		Debug.Log("Module : " + module + ", level : " + level);
		assignCurrentParameters();
	}

	public void SetLevel(int TMValueLevel) 
	{
		Debug.Log("Level : " + TMValueLevel);
		level = TMValueLevel;
		Debug.Log("3 - Module : " + module + ", level : " + level);
		assignCurrentParameters();
	}

	public void EvaluationMode(int value)
	{
		module = value;
		level = 1;
		Debug.Log("Module : " + module + ", level : " + level);
		assignCurrentParameters();
	}

	public void TrainingMode()
	{
		module = 1;
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

	public string GetTrack()
	{
		return track ;
	}

	public int GetColors()
	{
		return colors ;
	}

	public int GetTargets()
	{
		return targets ;
	}

	public int GetItems()
	{
		return items ;
	}

	public int GetInitialSpeed()
	{
		return initialspeed ;
	}

	public int GetSpeedIncrement()
	{
		return speedincrement ;
	}

	public int GetDoubleTask()
	{
		return doubletask ;
	}

	public int GetAscending()
	{
		return ascending ;
	}

	public int GetHomogeneisationDelay()
	{
		return homogeneisationdelay ;
	}

	public int GetInstructionDelay()
	{
		return instructiondelay ;
	}

	public int GetSelectorRule()
	{
		return selectorrule ;
	}

	public void ResetSelection()
	{
		module = 1;
		level = 1;
	}
}


[System.Serializable]
public class TMItems
{
    public TMItem [] items  ;
}

[System.Serializable]
public class TMItem
{
	public int module ;
	public int level ;
	public string track ;
	public int colors ;
	public int targets ;
	public int items ;
	public int initialspeed ;
	public int speedincrement ;
	public int doubletask ;
	public int ascending ;
	public int homogeneisationdelay ;
	public int instructiondelay ;
	public int selectorrule ;
}