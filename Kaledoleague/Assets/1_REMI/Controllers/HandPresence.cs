using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    public bool showController = false;
    public InputDeviceCharacteristics controllerCharacteristics;
    public List<GameObject> controllerPrefabs;
    public GameObject handmodelPrefab;

    private InputDevice targetDevice;
    private GameObject spawnedController;
    private GameObject spawnedHandModel;

    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }

        if(devices.Count > 0)
        {
            targetDevice = devices[0];
            GameObject prefab = controllerPrefabs.Find(ControllerColliderHit => ControllerColliderHit.name ==  targetDevice.name);
            if(prefab)
            {
                spawnedController =  Instantiate(prefab, transform);
            }
            else
            {
                Debug.Log("Did not ind corresponding controller model");
                spawnedController = Instantiate(controllerPrefabs[0], transform);
            }

            spawnedHandModel = Instantiate(handmodelPrefab, transform);
        }
    }

    void Update()
    {
        
        if(showController)
        {
            spawnedHandModel.SetActive(false);
            spawnedController.SetActive(true);
        }
        else
        {
            spawnedHandModel.SetActive(true);
            spawnedController.SetActive(false);
        }

        if(targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primayButtonValue) && primayButtonValue)
        {
            Debug.Log("Pressing Primary Button");
        }

        if(targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) && triggerValue >0.1f)
        {
            Debug.Log("Trigger pressed " + triggerValue);
        }

        if(targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue) && primary2DAxisValue != Vector2.zero)
        {
            Debug.Log("Primary Touchpad " + primary2DAxisValue);

        }
    }
}
