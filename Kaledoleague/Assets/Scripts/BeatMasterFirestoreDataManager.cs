using System.Collections ;
using System.Collections.Generic ;
using UnityEngine ;
using System ;

using Firebase.Firestore ;
using Firebase.Extensions ;

public class BeatMasterFirestoreDataManager : MonoBehaviour
{
	FirebaseFirestore db ;
	DocumentReference sessionDoc ;

	//string [] songs = {"Never Stop, by FM84", "Carpenter Brut, bu Turbo Killer", "Sub Focus, by Solar System"} ;

	string managerDocID, playerDocID, sessionDocId, trackName, nickname ;

	bool sessionWasCreated = false ;
	bool isUpdating = false ;

	int speed, obstacles, bpm, elements, colors, hFov, vFov, module, level, rulesChanging, selectorChanging ;

	Dictionary<string, object> metaData, insight, lastState, currentEvent ;
	Dictionary <string, object> events = new Dictionary <string, object>() ;

	int eventCounter = 0 ;
	
	// Start is called before the first frame update
	void Start()
	{
		db = FirebaseFirestore.DefaultInstance ;

		managerDocID = PlayerPrefs.GetString("managerid") ;
		playerDocID = PlayerPrefs.GetString("playerid") ;

		Debug.Log("Dec refs : " + managerDocID + " - " + playerDocID) ;

	}

	void Update()
	{
		if (sessionWasCreated && !isUpdating && eventCounter > 0)
		{
			StartCoroutine(UpdateSession()) ;
		}
	}

	public void InitParameters(float previsionF1Score, float previsionAchievement)
	{	
		int songIndex = 0 ;
		if (GameObject.Find ("MainManager") != null)
		{
			GameObject initializer = GameObject.Find("MainManager") ;
			BeatMasterGenerator bmg = initializer.GetComponent<BeatMasterGenerator>() ;
			
			module = bmg.GetModule() ;
			level = bmg.GetLevel() ;
			trackName = bmg.GetTrackName() ;
			bpm = bmg.GetBPM() ;
			songIndex = bmg.GetSong() ;
			elements = bmg.GetElements() ;
			colors = bmg.GetColors() ;
			hFov = bmg.GetHFov() ;
			vFov = bmg.GetVFov() ;
			speed = bmg.GetSpeed() ;
			obstacles = bmg.GetObstacles() ;
			rulesChanging = bmg.GetRuleChanging() ;
			selectorChanging = bmg.GetSelectorChanging() ;
			//initializer.GetComponent<DontDestroy>().DestroyAnyway() ;
		}
		
		db.Collection("Managers").Document(managerDocID).Collection("Team").Document(playerDocID).UpdateAsync(
				"lastviewed", Firebase.Firestore.FieldValue.ServerTimestamp
			) ;
		
		db.Collection("Managers").Document(managerDocID).Collection("Team").Document(playerDocID)
		.GetSnapshotAsync().ContinueWithOnMainThread(task =>
		{
			DocumentSnapshot snapshot = task.Result;
			if (snapshot.Exists) {
				var data = snapshot.ToDictionary() ;
				nickname = (string) data["nickname"] ;
			} else {
				Debug.Log(String.Format("Document {0} does not exist!", snapshot.Id));
			}
			Debug.Log("CRETE SESSION F1 : "+previsionF1Score);

			Debug.Log("CRETE SESSION ACH : " + previsionAchievement);

			CreateSession(previsionF1Score, previsionAchievement) ;
		});
	}


	// Update is called once per frame
	IEnumerator UpdateSession()
	{
		isUpdating = true ;
		yield return new WaitForSeconds(5.0f) ;
		sessionDoc.UpdateAsync("currentstate", currentEvent) ;
		sessionDoc.UpdateAsync("currentstate" + ".timestamp", Firebase.Firestore.FieldValue.ServerTimestamp) ;
		isUpdating = false ;
	}

	public void UpdateCurrentData(Dictionary<string, object> _currentEvent)
	{
		currentEvent = _currentEvent ;
		events.Add(eventCounter.ToString(), _currentEvent) ;
		eventCounter++ ;
		lastState = _currentEvent ;

		/*
		if (sessionWasCreated)
		{
			sessionDoc.UpdateAsync("current", _currentEvent) ;
			sessionDoc.UpdateAsync("current" + ".timestamp", Firebase.Firestore.FieldValue.ServerTimestamp) ;
		}
		*/
	}

	public void RecordSession(float evaluationF1Score, float evaluationAchievement)
	{
		insight.Add("evaluationf1score", evaluationF1Score);
		insight.Add("evaluationachievement", evaluationAchievement);
		Dictionary<string, object> recordedSessions = new Dictionary<string, object>
		{
			{ "metadata", metaData },
			{ "insight", insight },
			{ "session", events },
			{ "laststate", lastState },
			{ "timestamp", Firebase.Firestore.FieldValue.ServerTimestamp }
		} ;
		Debug.Log("BEATMASTER Firebase - recording session");

		db.Collection("Managers").Document(managerDocID).Collection("Team").Document(playerDocID).Collection("RecordedSessions").AddAsync(recordedSessions) ;
	}

	void CreateSession(float previsionF1Score, float previsionAchievement)
	{
		metaData = new Dictionary<string, object>
		{
			{ "game", "BeatMaster" },
			{ "song", trackName },
			{ "bpm", bpm },
			{ "module", module },
			{ "level", level },
			{ "player", nickname },
			{ "speed", speed },
			{ "elements", elements },
			{ "colors", colors },
			{ "ruleschanging", rulesChanging },
			{ "selectorchanging", selectorChanging },
			{ "obstacles", obstacles },
			{ "hfov", hFov },
			{ "vfov", vFov },
			{ "version", "210629" }
		} ;

		insight = new Dictionary<string, object>
		{
			{ "previsionf1score", previsionF1Score},
			{ "previsionachievement", previsionAchievement},
		} ;

		Dictionary<string, object> liveSessions = new Dictionary<string, object>
		{
			{ "metadata", metaData },
			{ "timestamp", Firebase.Firestore.FieldValue.ServerTimestamp }
		} ;
		Debug.Log("BEATMASTER Firebase - creating session") ;

		db.Collection("Managers").Document(managerDocID).Collection("Team").Document(playerDocID).Collection("LiveSessions").AddAsync(liveSessions).ContinueWithOnMainThread(task => {
			Debug.Log("Went here at least") ;
			sessionDoc = task.Result ;
			sessionDocId = sessionDoc.Id ;
			Debug.Log(String.Format("Added document with ID: {0}.", sessionDocId)) ;
			sessionWasCreated = true ;
		});
	}
}
