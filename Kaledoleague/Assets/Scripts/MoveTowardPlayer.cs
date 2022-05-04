using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardPlayer : MonoBehaviour
{
	GameObject player ; 
	float speed = 1.0f ;
	
    void Start()
	{
		player = GameObject.Find("/Player/MetaTarget") ;
    }

	public void Init(float hFov, float vFov, int bpm)
	{
		player = GameObject.Find("/Player/MetaTarget") ;
		speed = 32*60.0f/bpm ;
		float radius = 60.0f/bpm * 64 ;

		float yRot = (int) Random.Range(-hFov/2, hFov/2) ;
		float xRot = 0 ;
		if (Random.Range(0, 20) < 1)
			xRot = - (int) Random.Range(-22, vFov-22) ;
		else
			xRot = - (int) Random.Range(-22, 23) ;

		Quaternion q = Quaternion.identity ;
		q.eulerAngles = new Vector3(xRot, yRot, 0) ;
		Vector3 forwardDirection = Quaternion.Euler(xRot, yRot, 0) * player.transform.forward  ;
		Vector3 forwardPosition = forwardDirection * radius ;
		Vector3 targetPosition = player.transform.position + forwardPosition ;
		
		transform.position = targetPosition ;
        transform.LookAt(player.transform) ;
	}

    void FixedUpdate()
    {
		float step =  speed * Time.deltaTime ;
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step) ;
    }
}
