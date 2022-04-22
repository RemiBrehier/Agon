using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdateSlider : MonoBehaviour
{

    [SerializeField]
    private Slider M_Slider;
    [SerializeField]
    private TextMeshProUGUI m_Text;

    // Start is called before the first frame update
    void Start()
    {
        if(M_Slider !=null && m_Text != null)
        {
            m_Text.text = (M_Slider.value + 1).ToString();
        }
        else
        {
            Debug.LogError("Veuillez remplir les champs manquants");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSliderText()
    {
        m_Text.text = (M_Slider.value + 1).ToString();
    }
}
