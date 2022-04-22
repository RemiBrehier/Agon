using System.Collections ;
using System.Collections.Generic ;
using System;
using UnityEngine ;
using UnityEngine.UI ;
using UnityEngine.Networking ;

using Firebase ;
using Firebase.Firestore ;
using Firebase.Extensions ;

using TMPro ;

public class FirestoreLoginManager : MonoBehaviour
{
	public GameObject uiManager ;

	FirebaseFirestore db ;

	string nickname = "" ;
	string passkey = "" ;
	string errorMessage = "" ;
	bool isLoggedIn = false ;
	int counter = 0 ;

	bool wifiActivated = false ;

	void Start()
	{
		//PlayerPrefs.DeleteAll();
		StartCoroutine(CheckInternetConnexion()) ;
		uiManager.GetComponent<UIManager>().SetLoginScreenVisible() ;
	}

	void Update()
	{
		if (isLoggedIn && counter == 0)
		{
			uiManager.GetComponent<UIManager>().SetErrorMessage("") ;
			Debug.Log("You are logged in") ;

			counter++ ;
			PlayerPrefs.SetString("nickname", nickname) ;
			PlayerPrefs.SetString("passkey", passkey) ;
			PlayerPrefs.Save() ;
			uiManager.GetComponent<UIManager>().SetPlayerSelectionScreenVisible() ;
			GetComponent<FirestorePlayersManager>().PopulatePlayerList() ;
		}
	}

	IEnumerator CheckInternetConnexion()
	{
		UnityWebRequest request = new UnityWebRequest("http://google.com") ;
		yield return request.SendWebRequest() ;
		uiManager.GetComponent<UIManager>().SetErrorMessage("") ;

		if (request.error == null)
		{
			wifiActivated = true ;
			InitializeFirebase() ;
		}
		else
		{
			wifiActivated = false ;
			uiManager.GetComponent<UIManager>().SetErrorMessage("Activez votre connexion internet.") ;
		}
	}

	public void CheckNetwork()
	{
		StartCoroutine(CheckInternetConnexion()) ;
	}

	async void InitCredentials()
	{
		string _nickname = PlayerPrefs.GetString("nickname") ;
		string _passkey =  PlayerPrefs.GetString("passkey") ;

		Debug.Log("Registered data are " + _nickname + " / " + _passkey) ;

		if (!string.IsNullOrEmpty(_nickname) && !string.IsNullOrEmpty(_passkey))
		{
			nickname = _nickname ;
			passkey = _passkey ;
			Debug.Log("Trying " + nickname + " / " + passkey) ;
			Query query = db.Collection("Managers").WhereEqualTo("nickname", nickname).WhereEqualTo("passkey", passkey) ;
			await query.GetSnapshotAsync().ContinueWithOnMainThread(querySnapshotTask => {
				isLoggedIn = querySnapshotTask.Result.Count > 0 ;
			}) ;
		}
	}

	public void CheckCredentials()
	{
		if (!wifiActivated)
			CheckNetwork() ;
		else
		{
			GetNickname() ;
			GetPasskey() ;
			Debug.Log("Trying " + nickname + " / " + passkey) ;
			uiManager.GetComponent<UIManager>().SetErrorMessage("Connexion à la base de données ...") ;
			StartCoroutine(WaitForFirebase()) ;
			Query query = db.Collection("Managers").WhereEqualTo("nickname", nickname).WhereEqualTo("passkey", passkey) ;
			query.GetSnapshotAsync().ContinueWithOnMainThread( querySnapshotTask => {
				isLoggedIn = querySnapshotTask.Result.Count > 0 ;
				Debug.Log("Is loggedin ? " + isLoggedIn) ;
				string managerID = "" ;
				//int totalPlaysAllowed = 0, playsLeft = 0;
				foreach (var Document in querySnapshotTask.Result.Documents)
				{
					managerID = Document.Id ;
					//totalPlaysAllowed = Document.GetValue<int>("totalPlaysAllowed");
					//playsLeft = Document.GetValue<int>("playsLeft");
				}
				PlayerPrefs.SetString("managerid", managerID) ;
				//PlayerPrefs.SetInt("totalPlaysAllowed", totalPlaysAllowed);
				//PlayerPrefs.SetInt("playsLeft", playsLeft);
				PlayerPrefs.Save() ;
			}) ;
		}
	}

	IEnumerator WaitForFirebase()
	{
		yield return new WaitForSeconds(3.0f) ;

		if (!isLoggedIn)
		{
			uiManager.GetComponent<UIManager>().SetErrorMessage("Vos identifiants ne sont pas valides.") ;
		}
	}

	public void LogOut()
	{
		Debug.Log("Login out") ;
		isLoggedIn = false ;
		counter = 0 ;
		nickname = "" ;
		passkey = "" ;
		PlayerPrefs.SetString("nickname", "") ;
		PlayerPrefs.SetString("passkey", "") ;
		PlayerPrefs.SetString("managerid", "") ;
		uiManager.GetComponent<UIManager>().SetLoginScreenVisible() ;
	}


	void GetNickname()
	{
		nickname = GameObject.Find("nickname_inputfield").GetComponent<TMP_InputField>().text ;
	}
	void GetPasskey()
	{
		passkey = GameObject.Find("passkey_inputfield").GetComponent<TMP_InputField>().text ;
	}

	void InitializeFirebase()
	{
		FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
		{
			var dependencyStatus = task.Result ;
			if (dependencyStatus == Firebase.DependencyStatus.Available)
			{
	  			Debug.Log("FIREBASE DATABASE >> REACHED") ;
				db = FirebaseFirestore.DefaultInstance ;
				InitCredentials() ;
			}
			else
			{
	  			Debug.Log("FIREBASE DATABASE >> UNREACHABLE") ;
			}
		}) ;
	}

	void UpdatePlaysLeft()
	{
		DocumentReference docRef = db.Collection("Managers").Document(PlayerPrefs.GetString("managerid"));
		docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
		{
			DocumentSnapshot snapshot = task.Result;
			if (snapshot.Exists) {
				Debug.Log(String.Format("Document data for {0} document:", snapshot.Id));

				PlayerPrefs.SetInt("totalPlaysAllowed", snapshot.GetValue<int>("totalPlaysAllowed"));
				PlayerPrefs.SetInt("playsLeft", snapshot.GetValue<int>("playsLeft"));
			}
			else {
				Debug.Log(String.Format("Document {0} does not exist!", snapshot.Id));
			}
		});
	}
}
