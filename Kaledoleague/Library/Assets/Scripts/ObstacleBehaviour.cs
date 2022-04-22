using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBehaviour : MonoBehaviour
{
	float rotatingSpeed = 720.0f ;
	float speed = 32.0f ;
	GameObject player ;
	Transform targetedPlayer ;

	GameObject soundManager ;
	GameObject sessionManager ;

	Vector3 normalizeDirection;

	bool destroyed = false ;

    void Start()
    {
		player = GameObject.Find("ObstaclesCollider") ;
		Debug.Log("Spawned an obstacle") ;
        player = GameObject.Find("/Player") ;
		soundManager = GameObject.Find("[MANAGER]/SoundManager") ;
		sessionManager = GameObject.Find("[MANAGER]/SessionManager") ;
    }

	public void Init(float hFov, float vFov, int bpm)
	{
        player = GameObject.Find("ObstaclesCollider") ;
		speed = 32*60.0f/bpm ;

	

		float radius = 20.0f/bpm * 64 ;

		float yRot = 22 ;
		int choice = (int) Random.Range(0, 99) ;
		if (choice % 2 == 0)
			yRot = -22 ;

		Vector3 forwardDirection = Quaternion.Euler(-15, yRot, 0) * player.transform.forward  ;
		Vector3 forwardPosition = forwardDirection * radius ;
	
		transform.position = player.transform.position + forwardPosition ;
        transform.LookAt(player.transform) ;
		normalizeDirection = (new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z)  - transform.position).normalized ;

		//transform.localRotation = Quaternion.Euler(90.0f, 0.0f, 90.0f) ;

		float angleBetween = 0.0f;

		Vector3 targetDir = player.GetComponent<Transform>().position - transform.position;
		angleBetween = Vector3.Angle(transform.forward, targetDir);
	}

    // Update is called once per frame
    void FixedUpdate()
    {

		float step =  speed * Time.deltaTime/1.5f ;

		transform.Rotate(rotatingSpeed/2f * Time.deltaTime, rotatingSpeed/2 * Time.deltaTime, rotatingSpeed/2 * Time.deltaTime, Space.World);

		//transform.Rotate(0, rotatingSpeed * Time.deltaTime, 0, Space.World) ;
        transform.position += normalizeDirection * step ;

		if (transform.position.z < player.transform.position.z - 1)
		{
			Debug.Log("Obstacle avoided") ;
			sessionManager.GetComponent<BeatMasterSessionManager>().GoodAvoidance() ;
			Destroy(gameObject) ;
		}
    }


	void OnCollisionEnter(Collision collision)
    {
		if (collision.gameObject.name.Contains("ObstaclesCollider"))
		{
			Debug.Log("Obstacle collided with " + collision.gameObject.name) ;
			soundManager.GetComponent<BeatMasterSoundResults>().PlayObstacleCollided() ;
			sessionManager.GetComponent<BeatMasterSessionManager>().BadAvoidance() ;
			Destroy(gameObject) ;	
		}
    }

	
}
