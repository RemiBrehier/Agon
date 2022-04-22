using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideBreathing : MonoBehaviour
{
	public ParticleSystem expulseParticles, impulseParticles;
	float maxSize = 10.0f ;
	float minSize = 0.1f ;

	float lastSize = 0f ;

	public float coefficient = 1.0f ;

	bool isPlaying = false;


	// Start is called before the first frame update
	void Start()
	{
		Stop();
	}

	// Update is called once per frame
	void Update()
	{
		if(impulseParticles && expulseParticles)
		{
			if(isPlaying){
				float size = minSize + Mathf.PingPong(Time.time * 2.0f / 1.2f, (maxSize - minSize)) ;
				//Debug.Log("Size : " + size) ;

				if(lastSize < size)
				{
					impulseParticles.Play();
					expulseParticles.Stop();
				}
				else if(lastSize > size)
				{
					impulseParticles.Stop();
					expulseParticles.Play();
				}
				else
				{
					impulseParticles.Stop();
					expulseParticles.Stop();
				}

				lastSize = size;

				transform.localScale = new Vector3(size * coefficient, size * coefficient, size * coefficient) ;
			}
		}
	}

	public void Play()
	{
		isPlaying = true;
	}

	public void Stop()
	{
		isPlaying = false;
		if(impulseParticles && expulseParticles)
		{
			impulseParticles.Stop();
			expulseParticles.Stop();
		}
	}
}
