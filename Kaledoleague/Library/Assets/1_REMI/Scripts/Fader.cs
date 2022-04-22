using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{

    [Header("FADER")]
    [SerializeField, Tooltip("UI de fondu au noir")]
    private Image m_BlackFadeTransition;
    [SerializeField, Tooltip("Valeur de fin de fondu")]
    private Color opaqueColor = Color.black;
    [SerializeField, Tooltip("Valeur de fin de fondu")]
    private Color transparentColor = Color.clear;
    [SerializeField, Tooltip("Durée du fondu")]
    private float m_FadeDuration;
    public  bool m_EndFaderOpen = false;
    public  bool m_EndFaderClose = false;

    // Start is called before the first frame update
    void Start()
    {
            StartCoroutine(c_OpenFader());
        
    }

    public void OpenFader()
    {
        StartCoroutine(c_OpenFader());
    }

    public void CloseFader()
    {
        StartCoroutine(c_CloseFader());
    }

    //Ouverture du fondu au noir de l'image en enfant de la camera sous le nom "Fader"
    public IEnumerator c_OpenFader()
    {
        yield return new WaitForSeconds(1.5f);

        float counter = 0;

        while (counter < m_FadeDuration)
        {
            Debug.Log("OPEN FADER");
            counter += Time.deltaTime;
            float colorTime = counter / m_FadeDuration;
            m_BlackFadeTransition.color = Color.Lerp(opaqueColor, transparentColor, counter / m_FadeDuration);

            yield return null;
        }
    }

    //Fermeture du fondu au noir de l'image en enfant de la camera sous le nom "Fader"
    public IEnumerator c_CloseFader()
    {
        float counter = 0;

        while (counter < m_FadeDuration)
        {
            counter += Time.deltaTime;
            float colorTime = counter / m_FadeDuration;
            m_BlackFadeTransition.color = Color.Lerp(transparentColor, opaqueColor, counter / m_FadeDuration);
            Debug.Log(m_BlackFadeTransition.color.a);

            if (m_BlackFadeTransition.color.a >= 0.9f)
            {
                m_BlackFadeTransition.color = opaqueColor;
                m_EndFaderClose = true;
            }

            yield return null;
            m_EndFaderClose = true;

        }
    }

}
