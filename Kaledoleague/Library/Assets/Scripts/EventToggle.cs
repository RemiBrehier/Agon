using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventToggle : MonoBehaviour
{
    private UIManager uIManager;
    private GameObject m_UIManagerObject;

    private Toggle m_toggle;
    private bool m_State = false;

    [Header("MODE")]

    [SerializeField]
    private bool m_EvaluationMode = false;


    [SerializeField]
    private GameObject m_selector;

    [Header("MODULE")]
    [SerializeField]
    private int m_NumModule;

    [Header("GAME")]
    [SerializeField]
    private bool BMGenerator = false;
    [SerializeField]
    private bool TMGenerator = false;
    [SerializeField]
    private bool ZMGenerator = false;

    void Start()
    {

        m_toggle = GetComponent<Toggle>();

        m_toggle.onValueChanged.AddListener(delegate {ToggleValueChanged(m_toggle);});

        m_UIManagerObject = GameObject.FindGameObjectWithTag("UIManager");

        if (m_UIManagerObject != null)
            uIManager = m_UIManagerObject.GetComponent<UIManager>();
        else
            Debug.LogError("Objet manquant : UIManager introuvable");
    }


    public void ToggleValueChanged(Toggle change)
    {
        if (m_toggle.isOn == true)
        {
            m_selector.SetActive(true);

            if(m_EvaluationMode == false)
            {
                if (BMGenerator)
                {
                    uIManager.SetBMModule(m_NumModule);
                }
                else if (TMGenerator)
                {
                    uIManager.SetTMModule(m_NumModule);
                }
                else if (ZMGenerator)
                {
                    uIManager.SetZMModule(m_NumModule);
                }
            }
            else if(m_EvaluationMode == true)
            {
                if (BMGenerator)
                {
                    //uIManager.SetBMModule(m_NumModule);
                    uIManager.BMEvalMode(m_NumModule);
                }
                else if (TMGenerator)
                {
                    //uIManager.SetTMModule(m_NumModule);
                    uIManager.TMEvalMode(m_NumModule);
                }
                else if (ZMGenerator)
                {
                    //uIManager.SetZMModule(m_NumModule);
                    uIManager.ZMEvalMode(m_NumModule);
                }
            }
            
        }
        else if (m_toggle.isOn == false)
        {
            m_selector.SetActive(false);
        }
    }
}
