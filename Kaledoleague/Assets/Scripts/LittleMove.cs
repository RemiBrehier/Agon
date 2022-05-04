using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleMove : MonoBehaviour
{
	float dx = 0, dy = 0, dz = 0 ;
	bool inCoroutine = false ;

	Color color01 = new Color(1.0f, 0, 0, .7f) ;
	Color color02 = new Color(0, 0, 1.0f, .7f) ;

	Color currentColor ;

    // Start is called before the first frame update
    void Start()
    {
		currentColor = color02 ;
        GetComponent<MeshRenderer>().material.color = currentColor ;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		
		transform.position = Vector3.Lerp(transform.position, new Vector3 (transform.position.x + dx, transform.position.y + dy, transform.position.z + dz), .1f * Time.deltaTime) ;
		if (!inCoroutine)
			StartCoroutine(FloatingEffect()) ;
    }

	IEnumerator FloatingEffect()
	{
		inCoroutine = true ;

		dx = Random.Range(0, 10) / 5.0f - 1.0f ;
		dy = Random.Range(0, 10) / 5.0f - 1.0f  ;
		dz = Random.Range(0, 10) / 5.0f - 1.0f ;

		yield return new WaitForSeconds(5.0f) ;

		inCoroutine = false ;
	}

}
