using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBLEManager : MonoBehaviour
{
/*	List < BLEDevice > devices = new List < BLEDevice >() ;
	public BLEScan bleScanner ;

	public GameObject itemPrefab ;
	public GameObject viewport ;

	bool isInCoroutine = false ;

	// Start is called before the first frame update
	void Start()
	{    
		string lastDeviceName = PlayerPrefs.GetString("lasDeviceName","");
		string lastDeviceAddress = PlayerPrefs.GetString("lastDeviceAddress", "");

		//We add the last used device
		if(lastDeviceName != "" && lastDeviceAddress != "")
		{
			GameObject go = Instantiate(itemPrefab, new Vector3(0, 0, 0), Quaternion.identity) ;
			go.GetComponent<ItemBehaviourManager>().InitValues(0, lastDeviceName, lastDeviceAddress) ;
			go.transform.SetParent(viewport.transform) ;
			go.transform.localPosition = new Vector3(135.5f, -97.5f - (0), 0) ;
			go.transform.localRotation = Quaternion.Euler(0, 0, 0) ;
			go.transform.localScale = new Vector3(0.6f, 0.6f , 0.6f) ;
		}
	}

	private void Update()
	{
		if (!isInCoroutine)
			StartCoroutine(GetNewScannedObjects()) ;   
	}

	IEnumerator GetNewScannedObjects()
	{
		isInCoroutine = true ;
		List < BLEDevice > newDevices = bleScanner.GetScannedDevices() ;
		int index = 0 ;	
		Debug.Log("Devices count : " + newDevices.Count) ;
		if (newDevices.Count != devices.Count)
		{
			devices = newDevices ;
			//int y = 0 ;
			foreach (BLEDevice device in devices)
			{
				GameObject go = Instantiate(itemPrefab, new Vector3(0, 0, 0), Quaternion.identity) ;
				go.GetComponent<ItemBehaviourManager>().InitValues(index, device.name, device.address) ;
				go.transform.SetParent(viewport.transform) ;
				go.transform.localPosition = new Vector3(135.5f, -97.5f - (index * 60), 0) ;
				go.transform.localRotation = Quaternion.Euler(0, 0, 0) ;
				go.transform.localScale = new Vector3(0.6f, 0.6f , 0.6f) ;
			}
		}

		yield return new WaitForSeconds(2.0f) ;

		isInCoroutine = false ;
	}

	public void ChooseDevice()
	{
		//Debug.Log("Chose device " + index + " - mac = " + devices[index].address) ;
	}*/
}
