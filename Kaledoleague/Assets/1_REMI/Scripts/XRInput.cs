using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XRInput : MonoBehaviour
{
    public TrackerMasterPauseManager m_trackerMasterPauseManager;
    public BeatMasterPauseManager m_beatMasterPauseManager;

    [SerializeField]
    private XRNode xrNode = XRNode.RightHand;
    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device;

    public bool rightHandLastState = false;

    void Start()
    {
        rightHandLastState = false;
    }
    void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(xrNode, devices);
        device = devices.FirstOrDefault();
    }

    void OnEnable()
    {
        if(!device.isValid)
        {
            GetDevice();
        }
    }

    void Update()
    {
        if (!device.isValid)
        {
            GetDevice();
        }

        // capturing trigger button
        bool triggerButtonAction = false;
        if (device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerButtonAction) && triggerButtonAction)
        {

           
        }

        // capturing primary button
        bool primaryButton = false;
        InputFeatureUsage<bool> usage = CommonUsages.primaryButton;

        if (device.TryGetFeatureValue(usage, out primaryButton) && primaryButton)
        {
            Debug.Log("1 - Pause !!!" );

            if (primaryButton != rightHandLastState)
            {
                rightHandLastState = primaryButton;

                if(m_trackerMasterPauseManager != null)
                {
                    m_trackerMasterPauseManager.onMenuButtonEvent(true);

                }
                if (m_beatMasterPauseManager != null)
                {
                    m_beatMasterPauseManager.onMenuButtonEvent(true);

                }
            }
        }
    }
}
