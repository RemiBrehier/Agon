using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class NBackMaker
{
	private string m_alphabet;
	public char CorrectChar{get; private set;}
	public int CorrectIndex{get; private set;}

	public NBackMaker(string alphabet)
	{
		m_alphabet = alphabet;
	}

	public string GenerateString(int length, int nth)
	{
		StringBuilder sb = new StringBuilder(), nBack = new StringBuilder(); 
		char correctChar = m_alphabet[Random.Range(0, m_alphabet.Length)];

		//we create a guaranteed n-back
		nBack.Append(correctChar);
		for(int i = 0; i < nth - 1; i++)
		{
			nBack.Append(m_alphabet[Random.Range(0, m_alphabet.Length)]);
		}
		nBack.Append(correctChar);

		//we generate a random rest
		for(int i = 0; i < length - (nth + 1); i++)
		{
			sb.Append(m_alphabet[Random.Range(0, m_alphabet.Length)]);
		}

		//we put the n-back inside the string
		CorrectIndex = Random.Range(0, sb.Length);
		sb.Insert(CorrectIndex, nBack);
		CorrectIndex += nth;

		//we correct any mistakes
		for(int i = nth; i < sb.Length; i++)
		{
			if(sb[i] == sb[i - nth])
			{
				if(i == CorrectIndex)
				{
					sb[CorrectIndex] = correctChar;
					sb[CorrectIndex - nth] = correctChar;
				}
				else if(i == CorrectIndex - nth)
				{
					while(sb[i] == sb[i - nth]) sb[i - nth] = m_alphabet[Random.Range(0, m_alphabet.Length)];
				}
				else
				{
					while(sb[i] == sb[i - nth]) sb[i] = m_alphabet[Random.Range(0, m_alphabet.Length)];
					//Debug.Log("Replaced number at " + i + " with " + sb[i]);
				}
			}
		}

		/*
		for(int i = 0; i < length - (nth + 1); i++)
		{
			char c = m_alphabet[Random.Range(0, m_alphabet.Length)];
			if(i >= nth) while(c == sb[i - nth]) c = m_alphabet[Random.Range(0, m_alphabet.Length)]; // We ensure to not make another n-back of the same type
			sb.Append(c);
		}

		int r = Random.Range(0, sb.Length);
		if(r >= nth && r <= sb.Length - nth) while(correctChar == sb[r - nth] && correctChar == sb[r + nth]) r = Random.Range(0, sb.Length);
		sb.Insert(r, correctChar);

		for(int i = 1; i <= nth; i++)
		{
			char c = m_alphabet[Random.Range(0, m_alphabet.Length)];
			if(i + r >= nth && i + r <= sb.Length - nth) while(c == sb[i + r - nth] && c == sb[i + r + nth]) c = m_alphabet[Random.Range(0, m_alphabet.Length)];
			sb.Insert(i + r, c);
		}
		sb.Insert(r, correctChar);
		*/

		Debug.Log("Chosen char: " + correctChar);
		Debug.Log("Phrase generated: " + sb);
		Debug.Log("Correct nBack at: " + (CorrectIndex + 1));

		//sb.Insert(Random.Range(0, sb.Length), nBack);

		//Debug.Log("Generated: " + sb.ToString());

		return sb.ToString();
	}
}
