using System.Collections ;
using System.Collections.Generic ;
using UnityEngine ;

// This script make crystals be luminous like a breathing rythm
public class CrystalShine : MonoBehaviour
{
	public float inf = 0 ;
    private Material material;
	float lumen = 0 ;

    void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    void Start()
    {
        material.SetFloat("_EmissionPower", 0) ;
    }

	public void SetLumen(float value)
	{
		float newLumen = (value - inf) / (1.0f - inf) ;
		if (newLumen > 1)
			newLumen = 1 ;
		if (newLumen < 0)
			newLumen = 0 ;

		lumen = .9f * lumen + .1f * newLumen ;
		
        material.SetFloat("_EmissionPower", lumen * 2) ;
	}

}
