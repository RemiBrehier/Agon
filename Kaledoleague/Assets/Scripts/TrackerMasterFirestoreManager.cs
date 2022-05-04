using System.Collections ;
using System.Collections.Generic ;
using UnityEngine ;
using System ;

using Firebase.Firestore ;
using Firebase.Extensions ;

public class TrackerMasterFirestoreManager : MonoBehaviour
{
	FirebaseFirestore db ;
	DocumentReference sessionDoc ;

	Dictionary <string, object> events = new Dictionary <string, object>() ;
	Dictionary<string, object> metaData, insight, lastState, currentEvent;

	string nickname, managerDocID, playerDocID, sessionDocId ;
	string track = "" ;

	bool sessionWasCreated = false ;
	bool isUpdating = false ;

	int module = 0 ;
	int level = 0 ;
	int colors = 2 ;
	int targets = 1 ;
	int items = 3 ;
	int initialspeed = 1 ;
	int speedincrement = 1 ;
	int doubletask = 0 ;
	int ascending = 0 ;
	int homogeneisationdelay = 0 ;
	int instructiondelay = 0 ;
	int selectorrule = 0 ;
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
		/*
		if (sessionWasCreated && !isUpdating && eventCounter > 0)
		{
			StartCoroutine(UpdateSession()) ;
		}
		*/
	}

	public void InitParameters(float previsionF1Score, float previsionAchievement)
	{	
		if (GameObject.Find ("MainManager") != null)
		{
			GameObject initializer = GameObject.Find("MainManager") ;
			TrackingMasterGenerator tmg = initializer.GetComponent<TrackingMasterGenerator>() ;
			module = tmg.GetModule() ;
			level = tmg.GetLevel() ;
			track = tmg.GetTrack() ;
			colors = tmg.GetColors() ;
			targets = tmg.GetTargets() ;
			items = tmg.GetItems() ;
			initialspeed = tmg.GetInitialSpeed() ;
			speedincrement = tmg.GetSpeedIncrement() ;
			doubletask = tmg.GetDoubleTask() ;
			ascending = tmg.GetAscending() ;
			homogeneisationdelay = tmg.GetHomogeneisationDelay() ;
			instructiondelay = tmg.GetInstructionDelay() ;
			selectorrule = tmg.GetSelectorRule() ;
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
			CreateSession(previsionF1Score, previsionAchievement) ;
		});
	}

	// Update is called once per frame
	/*
	IEnumerator UpdateSession()
	{
		isUpdating = true ;
		sessionDoc.UpdateAsync("currentState", currentEvent) ;
		sessionDoc.UpdateAsync("currentState" + ".timestamp", Firebase.Firestore.FieldValue.ServerTimestamp) ;
		yield return new WaitForSeconds(5.0f) ;
		isUpdating = false ;

	}
	*/

	public void UpdateCurrentData(Dictionary<string, object> _currentEvent)
	{
		currentEvent = _currentEvent ;
		events.Add(eventCounter.ToString(), _currentEvent) ;
		eventCounter++ ;
		
		sessionDoc.UpdateAsync("currentstate", _currentEvent) ;
		sessionDoc.UpdateAsync("currentstate" + ".timestamp", Firebase.Firestore.FieldValue.ServerTimestamp) ;
		lastState = _currentEvent;
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
		Debug.Log("TRACKERMASTER Firebase - recording session");

		db.Collection("Managers").Document(managerDocID).Collection("Team").Document(playerDocID).Collection("RecordedSessions").AddAsync(recordedSessions) ;
	}

	void CreateSession(float previsionF1Score, float previsionAchievement)
	{
		metaData = new Dictionary<string, object>
		{
			{ "game", "TrackerMaster" },
			{ "player", nickname },
			{ "module", module },
			{ "level", level },
			{ "targets", targets },
			{ "items", items },
			{ "initialSpeed", initialspeed },
			{ "speedIncrement", speedincrement },
			{ "track", track },
			{ "colors", colors },
			{ "doubleTask", doubletask },
			{ "ascending", ascending },
			{ "homogeneisationDelay", homogeneisationdelay },
			{ "instructionDelay", instructiondelay },
			{ "selectorRule", selectorrule },
			{ "version", "210629" }
		} ;

		insight = new Dictionary<string, object>
		{
			{ "previsionf1score", previsionF1Score },
			{ "previsionachievement", previsionAchievement },
		};

		Dictionary<string, object> liveSessions = new Dictionary<string, object>
		{
			{ "metadata", metaData },
			{ "timestamp", Firebase.Firestore.FieldValue.ServerTimestamp }
		};
		Debug.Log("TRACKERMASTER Firebase - creating session") ;

		db.Collection("Managers").Document(managerDocID).Collection("Team").Document(playerDocID).Collection("LiveSessions").AddAsync(liveSessions).ContinueWithOnMainThread(task => {
			sessionDoc = task.Result ;
			sessionDocId = sessionDoc.Id ;
			Debug.Log(String.Format("Added document with ID: {0}.", sessionDocId)) ;
			sessionWasCreated = true ;
		});
	}

	private int GetTotalPlaysAllowed()
	{
		string managerID = PlayerPrefs.GetString("managerid");
		DocumentReference docRef = db.Collection("Manager").Document(managerID);
		int totalPlays = 0;

		docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
		{
			DocumentSnapshot snapshot = task.Result;
			if (snapshot.Exists) {
				Debug.Log(String.Format("Document data for {0} document:", snapshot.Id));
				snapshot.TryGetValue<int>("totalPlaysAllowed", out totalPlays);
			} else {
				Debug.Log(String.Format("Document {0} does not exist!", snapshot.Id));
			}
		});

		return totalPlays;
	}
}
