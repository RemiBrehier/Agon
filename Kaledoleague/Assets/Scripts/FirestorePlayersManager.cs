using System.Collections ;
using System.Collections.Generic ;
using UnityEngine ;
using UnityEngine.UI ;
using UnityEngine.Networking ;

using Firebase ;
using Firebase.Firestore ;
using Firebase.Extensions ;

using TMPro ;

public class FirestorePlayersManager : MonoBehaviour
{
	public GameObject playerPrefab ;
	public GameObject uiManager ;
	public GameObject viewPortContent ;

	FirebaseFirestore db ;

	public void PopulatePlayerList()
	{
		GameObject [] gos = GameObject.FindGameObjectsWithTag("PlayerSelectorItem") ;
		foreach (GameObject go in gos)
		{
			Destroy(go) ;
		}

		db = FirebaseFirestore.DefaultInstance ;
		string managerDocId = PlayerPrefs.GetString("managerid") ;
		Query queryTeam = db.Collection("Managers").Document(managerDocId).Collection("Team") ;
		queryTeam.GetSnapshotAsync().ContinueWithOnMainThread( queryTeamSnapshotTask => {
			int y = 0 ;
			foreach (DocumentSnapshot document in queryTeamSnapshotTask.Result.Documents)
			{
				var data = document.ToDictionary() ;
				string playerFirstName = (string) data["firstname"] ;
				string playerLastName = (string) data["lastname"] ;
				string playerNickName = (string) data["nickname"] ;
				string playerPhotoID = "" ;//(string) data["photoid"] ;
				if (document.ContainsField("photoid"))
					playerPhotoID = (string) data["photoid"] ;

				Debug.Log("Player doc id : " + document.Id) ;
				Debug.Log("Player doc infos : " + playerFirstName + " " + playerLastName + " aka " + playerNickName) ;

				GameObject go = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity) ;
				go.GetComponent<PlayerSelectionItemManager>().Init(playerFirstName, playerLastName, playerNickName, document.Id, playerPhotoID) ;
				go.transform.SetParent(viewPortContent.transform) ;
				go.transform.localPosition = new Vector3(0, -135 - (y * 130), 0) ;
				go.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) ;
				y++ ;	
			}
		}) ;
	}


	public void OnSelect(string playerDocId)
	{
		PlayerPrefs.SetString("playerid", playerDocId) ;
		uiManager.GetComponent<UIManager>().SetGameSelectionScreenVisible() ;
	}
}
