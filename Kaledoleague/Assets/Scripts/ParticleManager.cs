using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
	[SerializeField] private ParticleSystem expulseParticles, impulseParticles;

    // Start is called before the first frame update
    void Start()
    {
        expulseParticles.Stop();
		impulseParticles.Stop();
		StartCoroutine(CycleBreathing());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	IEnumerator CycleBreathing()
	{
		while(true)
		{
			impulseParticles.Play();
			yield return new WaitForSeconds(3f);

			impulseParticles.Stop();
			yield return new WaitForSeconds(3f);

			expulseParticles.Play();
			yield return new WaitForSeconds(3f);

			expulseParticles.Stop();
			yield return new WaitForSeconds(3f);
		}
	}
}
