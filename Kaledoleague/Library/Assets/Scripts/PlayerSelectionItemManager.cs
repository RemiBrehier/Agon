using System.Collections ;
using System.Collections.Generic ;
using UnityEngine ;
using UnityEngine.UI ;
using UnityEngine.Networking ;
using TMPro ;

public class PlayerSelectionItemManager : MonoBehaviour
{
	private string docID ;
	public GameObject avatar ;

	public void Init(string firstname, string lastname, string nickname, string docId, string avatarURL)
	{
		transform.Find("identity_text").gameObject.GetComponent<TextMeshProUGUI>().text = "<b>" + firstname + " " + lastname + "</b>\n<i>aka " + nickname + "</i>";
		docID = docId ;
		Debug.Log(avatarURL) ;
		
		if (avatarURL != "")
		{
			Debug.Log("Avatar URL : " + avatarURL) ;
			StartCoroutine(ApplyAvatarImage(avatarURL)) ;
		}
		else
		{
			avatar.GetComponent<Image>().color = new Vector4(1.0f, 0.341f, 0.133f, 1) ;
		}
		
	}

	IEnumerator ApplyAvatarImage(string url)
    {

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError) 
            Debug.Log(request.error);
        else
		{
            Texture2D tex = ((DownloadHandlerTexture) request.downloadHandler).texture;
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
            avatar.GetComponent<Image>().overrideSprite = sprite;
        }
    } 

	public void OnSelect()
	{
		Debug.Log("Selected " + docID) ;
		GameObject.Find("/FirestoreManagement").GetComponent<FirestorePlayersManager>().OnSelect(docID) ;
	}

}
