using System.Collections ;
using System.Collections.Generic ;
using UnityEngine ;
using UnityEngine.Video ;

public class PlaySplashscreenVideo : MonoBehaviour
{
	public float minimumTime = 8.0f ;
	public string scene = "MenuScene" ;

	public Fader fader;

    void Start()
    {
		StartCoroutine(PlayVideo()) ;
    }

	IEnumerator PlayVideo()
	{

		yield return new WaitForSeconds(minimumTime) ;

		fader.CloseFader();

		AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene) ;
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
	}

}
