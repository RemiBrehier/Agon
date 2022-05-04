using System.Collections ;
using System.Collections.Generic ;
using UnityEngine ;

using TMPro ;


public class ForceTextButton : MonoBehaviour
{
	public string forcedContent ;
    // Start is called before the first frame update
    void Update()
    {
		transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text = forcedContent ;
        
    }
}
