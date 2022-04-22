using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class TextWriter : MonoBehaviour
{
	public TextMeshProUGUI TextMeshProComponent;
	public float ScrollSpeed = 2;
	public bool isScrolling;

	private RectTransform m_rectTransform;
	private Vector3 startPosition;
	private NBackMaker generator = new NBackMaker("ABCD");
	private Keyboard kb;

	private void Start()
	{
		m_rectTransform = TextMeshProComponent.rectTransform;
		startPosition = m_rectTransform.position;
		kb = InputSystem.GetDevice<Keyboard>();

		StartCoroutine(InitSession());
		//generator.GenerateString(20, 3);

	}

	private IEnumerator ScrollOnce(char c)
	{
		TextMeshProComponent.text = c + string.Empty;
		float scrollPosition = 0, width = 15;
		isScrolling = true;

		while(scrollPosition <= width)
		{
			m_rectTransform.position = new Vector3(startPosition.x - scrollPosition % width, startPosition.y, startPosition.z);
			scrollPosition += ScrollSpeed * Time.deltaTime;

			yield return null;
		}

		isScrolling = false;
		yield return null;
	}

	private IEnumerator InitSession()
	{
		string phrase = generator.GenerateString(8, 2);
		char key = generator.CorrectChar;
		int i = 0;

		foreach(char c in phrase)
		{
			TextMeshProComponent.text = c + string.Empty;
			float scrollPosition = 0, width = 15;
			bool hasAnswered = false;
			isScrolling = true;

			while(scrollPosition <= width && !hasAnswered)
			{
				m_rectTransform.position = new Vector3(startPosition.x - scrollPosition % width, startPosition.y, startPosition.z);
				scrollPosition += ScrollSpeed * Time.deltaTime;

				if(kb.spaceKey.wasPressedThisFrame)
				{
					if(i == generator.CorrectIndex)
					{
						Debug.Log("Correct !");
						TextMeshProComponent.color = Color.green;
					}
					else
					{
						Debug.Log("Wrong !");
						TextMeshProComponent.color = Color.red;
					}
					hasAnswered = true;

					yield return new WaitForSeconds(2);

					scrollPosition = width;
					m_rectTransform.position = new Vector3(startPosition.x - scrollPosition, startPosition.y, startPosition.z);
				}

				yield return null;
			}

			isScrolling = false;

			if(hasAnswered)
			{
				break;
			}
			
			i++;
			yield return null;
		}
	}
}
