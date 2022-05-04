using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RebuildLayoutManager : MonoBehaviour
{
    [SerializeField]
    private List<RectTransform> m_RebuildLayout;

    // Start is called before the first frame update
    void Start()
    {
        if (m_RebuildLayout.Count > 0)
        {
            RebuildLayoutRectTransform();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /* private void OnEnable()
     {
         if (m_RebuildLayout.Count > 0)
         {
             RebuildLayoutRectTransform();
         }
     }*/



    void RebuildLayoutRectTransform()
    {
        for (int i = 0; i < m_RebuildLayout.Count; i++)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_RebuildLayout[i]);
        }
    }
}
