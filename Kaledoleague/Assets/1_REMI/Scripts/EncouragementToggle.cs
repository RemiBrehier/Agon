using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EncouragementToggle : MonoBehaviour
{

    private Toggle m_toggle;
    private EncouragementManager m_encouragementManager;

    // Start is called before the first frame update
    void Start()
    {
        m_toggle = GetComponent<Toggle>();
        m_toggle.onValueChanged.AddListener(delegate { ToggleValueChanged(m_toggle); });
        m_encouragementManager = GameObject.FindGameObjectWithTag("MANAGER").GetComponent<EncouragementManager>();
        m_toggle.isOn = m_encouragementManager.m_Encouragement;

        if (m_encouragementManager.m_Encouragement == true)
        {
            m_toggle.isOn = true;
        }
        else if (m_encouragementManager.m_Encouragement == false)
        {
            m_toggle.isOn = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleValueChanged(Toggle change)
    {
        m_encouragementManager.m_Encouragement = !m_encouragementManager.m_Encouragement;

        if (m_encouragementManager.m_Encouragement == true)
        {
            m_toggle.isOn = true;
            Debug.Log("ENCOURAGEMENT Activé : "+ m_encouragementManager.m_Encouragement);
        }
        else if(m_encouragementManager.m_Encouragement == false)
        {
            m_toggle.isOn = false;
            Debug.Log("ENCOURAGEMENT Désactivé : " + m_encouragementManager.m_Encouragement);

        }
    }
}
