using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class OrbController : MonoBehaviour
{
	public bool m_SourceSPhere = false;

	[SerializeField]
	private GameObject m_Capsule;

	private Vector3 targetPosition ;
	private Vector3 updatedPosition ;
	public float alpha = .1f ;
	public float speed = 1.0f ;
	public int number = 0;

	private Color originalColor;

	private bool updatingPosition = false ;
	private bool stopped = true ;
	private bool isSelectable ;

	private bool isATarget = false ;
	private bool wasChosen = false ;
	private bool isChosable = false ;

	public TextMeshProUGUI textMesh;
	public AudioSource audioSource;
	public ParticleSystem particles;
	
	float responseTime = 0 ;
	float startTime = 0 ;

	// Start is called before the first frame update
	void Start()
	{


		targetPosition = new Vector3(3, 0, 3);
		updatedPosition = new Vector3(3, 0, 3);
	}

	public float GetResponseTime()
	{
		return responseTime ;
	}

	// Update is called once per frame
	void Update()
	{
		Debug.Log(isChosable);

		if (!stopped)
		{
			targetPosition = (1 - alpha) * targetPosition + alpha * updatedPosition ;
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime) ;
			if (!updatingPosition)
				StartCoroutine(updateRandomly()) ;
		}
	}

	public void setNewPosition(Vector3 newPosition)
	{
		updatedPosition = newPosition ;
	}

	IEnumerator updateRandomly()
	{
		updatingPosition = true ;
		setNewPosition(new Vector3(0.0f + Random.Range(-12.0f, 12.0f), Random.Range(0.5f, 2.0f), 0.0f + Random.Range(-12.0f, 12.0f))) ;
		yield return new WaitForSeconds(1.0f) ;
		updatingPosition = false ;
	}

	public void BeginAnimation()
	{
		stopped = false ;
	}

	public void StopAnimation()
	{
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition ;
		GetComponent<XRSimpleInteractable>().enabled = true;
		stopped = true ;
	}

	public void SetSpeed(float s)
	{
		speed = s ;
	}

	public void SetTarget()
	{
		isATarget = true ;
	}

	public void SetChosable(bool state)
	{
		isChosable = true ;
		startTime = Time.time ;
	}

	public void WasChosen()
	{
		Debug.Log("Was chosen by ray interactor") ;
		responseTime = Time.time - startTime ;
		if (isChosable)
		{
			wasChosen = true ;
			isChosable = false;
			StartCoroutine(DisableInteractable());
			RefreshColor();
			ShowText(true);
			audioSource.Play();
			particles.Play();
		}
	}

	public void SetOriginalColor(Color color)
	{
		originalColor = color;
		RefreshColor();

		var main = particles.main;
		main.startColor = color;
	}

	public void SetColor(Color color)
	{
		m_Capsule.GetComponent<Renderer>().materials[1].color = color;
		m_Capsule.GetComponent<Renderer>().materials[1].SetColor("_EmissionColor", color);
	}

	public void RefreshColor()
	{
		m_Capsule.GetComponent<Renderer>().materials[1].color = originalColor;
		m_Capsule.GetComponent<Renderer>().materials[1].SetColor("_EmissionColor", originalColor);	
	}

	public bool IsATarget()
	{
		return isATarget ;
	}

	public bool IsChosen()
	{
		Debug.Log("IS CHOSEN - 00");
		return wasChosen ;
	}

	public void SetNumber(int n)
	{
		number = n;
		textMesh.text = number.ToString();
	}

	public void ShowText(bool b)
	{
		textMesh.transform.gameObject.SetActive(b);
	}

	IEnumerator DisableInteractable()
	{
		yield return new WaitForSeconds(0.5f);
		GetComponent<XRSimpleInteractable>().enabled = false;
	}

	public bool IsStopped()
	{
		return stopped;
	}

	public bool IsChosable()
	{
		return isChosable;
	}
}
