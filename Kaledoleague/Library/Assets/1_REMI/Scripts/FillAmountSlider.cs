using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FillAmountSlider : MonoBehaviour
{

    private Slider m_slider;
    [SerializeField]
    private Image ImageFillAmount;
    [SerializeField]
    private TextMeshProUGUI m_Level;

    void Start()
    {
        m_slider = GetComponent<Slider>();
    }

    public void UpdateFillAmount()
    {
        ImageFillAmount.fillAmount = m_slider.value / m_slider.maxValue;
        m_Level.text = " Levels : " + (m_slider.value+1).ToString();

    }

}
