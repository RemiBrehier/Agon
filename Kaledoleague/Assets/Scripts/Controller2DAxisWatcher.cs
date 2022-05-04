using System.Collections.Generic ;
using UnityEngine ;
using UnityEngine.Events ;
using UnityEngine.XR ;

[System.Serializable]
public class Axis2DEvent : UnityEvent<Vector2> { }

[System.Serializable]
public class PrimaryButtonEvent : UnityEvent<bool> { }


 public enum XRDevice
 {
    leftHand, rightHand, head
 }

public class Controller2DAxisWatcher : MonoBehaviour
{
    public Axis2DEvent axis2DChangeValue ;
    public PrimaryButtonEvent primaryButtonChangedValue ;

    public XRDevice currentDevice ;
    private List<InputDevice> devicesWith2DAxis ;

	private bool oldState = false ;

    private void Awake()
    {
        if (axis2DChangeValue == null)
        {
            axis2DChangeValue = new Axis2DEvent() ;
        }
		if (primaryButtonChangedValue == null)
		{
            primaryButtonChangedValue = new PrimaryButtonEvent() ;
		}

        devicesWith2DAxis = new List<InputDevice>() ;
    }

    void OnEnable()
    {
        List<InputDevice> allDevices = new List<InputDevice>();
        InputDevices.GetDevices(allDevices);
        foreach(InputDevice device in allDevices)
        {
            if (   (currentDevice == XRDevice.leftHand && device.name.Contains("Controller - Left") )
                || (currentDevice == XRDevice.rightHand && device.name.Contains("Controller - Right") )
                || (currentDevice == XRDevice.head && device.name.Contains("Quest") ) )
            {
                InputDevices_deviceConnected(device) ;
                Debug.Log("DEBUG VERBOSE Added device " + device.name) ;
            }
        }

        InputDevices.deviceConnected += InputDevices_deviceConnected ;
        InputDevices.deviceDisconnected += InputDevices_deviceDisconnected ;
    }

    private void OnDisable()
    {
        InputDevices.deviceConnected -= InputDevices_deviceConnected ;
        InputDevices.deviceDisconnected -= InputDevices_deviceDisconnected ;
        devicesWith2DAxis.Clear() ;
    }

    private void InputDevices_deviceConnected(InputDevice device)
    {
        Vector2 discardedValue ;
        if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out discardedValue))
        {
            devicesWith2DAxis.Add(device); // Add any devices that have a 2D Axis.
        }
    }

    private void InputDevices_deviceDisconnected(InputDevice device)
    {
        if (devicesWith2DAxis.Contains(device))
            devicesWith2DAxis.Remove(device) ;
    }

    void Update()
    {
        foreach (var device in devicesWith2DAxis)
        {
            Vector2 Axis2DState = Vector2.zero ;
            device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Axis2DState) ;
            axis2DChangeValue.Invoke(Axis2DState) ;

			bool buttonState = false ;
			device.TryGetFeatureValue(CommonUsages.primaryButton, out buttonState) ;
			if (buttonState == true && oldState != true)
			{
				oldState = true ;
				primaryButtonChangedValue.Invoke(buttonState) ;
			}
			else if (buttonState == false)
			{
				oldState = false ;
			}
        }
    }
}