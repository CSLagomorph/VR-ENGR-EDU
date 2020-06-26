using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using Valve.VR;

public class PointerInput : BaseInput
{
    private Camera eventCamera = null;
    public Camera EventCamera
    {
        get
        {
            if(eventCamera == null)
            {
                eventCamera = ((Canvas)GameObject.FindObjectOfType<Canvas>()).worldCamera;
            }
            return eventCamera;
        }
    }
    
    public SteamVR_Action_Boolean defaultSelectAction = SteamVR_Actions._default.InteractUI;
    public SteamVR_Input_Sources defaultInputSource = SteamVR_Input_Sources.RightHand;

    public OVRInput.Button defaultSelectButton = OVRInput.Button.PrimaryIndexTrigger;
    public OVRInput.Controller defaultController = OVRInput.Controller.RTouch;

    public SteamVR_Action_Boolean selectAction; 
    public SteamVR_Input_Sources handType;

    public OVRInput.Button selectButton;
    public OVRInput.Controller controller;

    protected override void Awake()
    {

        if(PlayerInfo.VRDeviceName != PlayerInfo.NO_DETECTED_DEVICE)
        {
            EventSystem.current.GetComponent<BaseInputModule>().inputOverride = this;

        }
        if(PlayerInfo.VRDeviceName == PlayerInfo.OPEN_VR_DEVICE)
        {
            selectAction = defaultSelectAction;
            handType = defaultInputSource;
        }
        else if(PlayerInfo.VRDeviceName == PlayerInfo.OCULUS_DEVICE)
        {
            selectButton = defaultSelectButton;
            controller = defaultController;
        }
    }

    public override bool GetMouseButton(int button)
    {
        if(PlayerInfo.VRDeviceName == PlayerInfo.OPEN_VR_DEVICE)
        {   
            return SteamVR_Actions._default.InteractUI.GetState(handType);
        }
        else if(PlayerInfo.VRDeviceName == PlayerInfo.OCULUS_DEVICE)
        {
            return OVRInput.Get(selectButton, controller);
        }
        return false;
    }

    public override bool GetMouseButtonDown(int button)
    {
        if(PlayerInfo.VRDeviceName == PlayerInfo.OPEN_VR_DEVICE)
        {   
            return SteamVR_Actions._default.InteractUI.GetStateDown(handType);
        }
        else if(PlayerInfo.VRDeviceName == PlayerInfo.OCULUS_DEVICE)
        {
            return OVRInput.GetDown(selectButton, controller);
        }
        return false;
    }

    public override bool GetMouseButtonUp(int button)
    {
        if(PlayerInfo.VRDeviceName == PlayerInfo.OPEN_VR_DEVICE)
        {   
            return SteamVR_Actions._default.InteractUI.GetStateUp(handType);
        }
        else if(PlayerInfo.VRDeviceName == PlayerInfo.OCULUS_DEVICE)
        {
            return OVRInput.GetUp(selectButton, controller);
        }
        return false;
    }

    public override Vector2 mousePosition
    {
        get
        {
            return new Vector2(EventCamera.pixelWidth / 2, EventCamera.pixelHeight / 2);
        }
    }
}
