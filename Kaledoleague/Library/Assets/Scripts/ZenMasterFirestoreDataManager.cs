using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Firebase.Firestore ;
using Firebase.Extensions ;

public class ZenMasterFirestoreDataManager : MonoBehaviour
{
	FirebaseFirestore db ;
	DocumentReference sessionDoc ;

	Dictionary <string, object> events = new Dictionary <string, object>() ;
	Dictionary<string, object> metaData, insight, lastState, currentEvent;
	
	string managerDocID, playerDocID, sessionDocId, nickname ;
	bool sessionWasCreated = false ;
	int eventCounter = 0 ;

	float hrv = 0;

    // Start is called before the first frame update
    void Start()
    {
        db = FirebaseFirestore.DefaultInstance ;

		managerDocID = PlayerPrefs.GetString("managerid") ;
		playerDocID = PlayerPrefs.GetString("playerid") ;

		Debug.Log("Dec refs : " + managerDocID + " - " + playerDocID) ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void InitParameters(float previsionF1Score, float previsionAchievement)
	{
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
		Debug.Log("ZENMASTER Firebase - recording session");

		db.Collection("Managers").Document(managerDocID).Collection("Team").Document(playerDocID).Collection("RecordedSessions").AddAsync(recordedSessions) ;
	}

	void CreateSession(float previsionF1Score, float previsionAchievement)
	{
		metaData = new Dictionary<string, object>
		{
			{ "game", "ZenMaster" },
			{ "player", nickname },
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
}
