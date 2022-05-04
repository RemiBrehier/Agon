using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveButtonStart : MonoBehaviour
{
    private ToggleGroup m_toggleGroup;
    private Button m_StartButton;

    void Start()
    {
        m_toggleGroup = GetComponent<ToggleGroup>();

    }

    public void CheckToggleGroup(Button m_StartButton)
    {
        if(m_toggleGroup != null)
        {
            if(m_toggleGroup.AnyTogglesOn() == true)
            {
                m_StartButton.interactable = true;
            }
        }
        else
        {
            m_StartButton.interactable = false;
        }
    }
}
