using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.XR;
using UnityEngine.EventSystems;

public class PlayerMovementController : MonoBehaviour
{
    public SteamVR_Action_Vector2 input;
    public SteamVR_Action_Boolean grip;
    public SteamVR_Input_Sources rightHand = SteamVR_Input_Sources.RightHand;
    public OVRInput.Controller rightController = OVRInput.Controller.RTouch;
    public OVRInput.Controller leftController = OVRInput.Controller.LTouch;
    public Vector2 joyInput;
    public float speed = 2.0f;

    public GameObject laserPointer;

    public bool isRotating = false;

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.zero;
        if(PlayerInfo.VRDeviceName == PlayerInfo.OPEN_VR_DEVICE)
        {
            direction = Player.instance.hmdTransform.TransformDirection(new Vector3(input.axis.x, 0, input.axis.y));
        }
        else if(PlayerInfo.VRDeviceName == PlayerInfo.OCULUS_DEVICE)
        {
            joyInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, leftController);
            direction = PlayerInfo.PlayerTransform.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor").TransformDirection(new Vector3(joyInput.x, 0, joyInput.y));
        }
        transform.position += speed * Vector3.ProjectOnPlane(direction, Vector3.up) * Time.deltaTime;
        ToggleLaserPointer();
        SnapRotation();
    }

    void ToggleLaserPointer()
    {
        if(laserPointer == null)
        {
            laserPointer = PlayerInfo.PlayerTransform.gameObject.GetComponentInChildren<UI_Pointer>().gameObject;
        }

        bool toggle = false;
        if(PlayerInfo.VRDeviceName == PlayerInfo.OPEN_VR_DEVICE)
        {
            toggle = grip.GetStateDown(rightHand);
        }
        else if(PlayerInfo.VRDeviceName == PlayerInfo.OCULUS_DEVICE)
        {
            toggle = OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, rightController);
        }
        if(toggle)
        {
            laserPointer.SetActive(!laserPointer.activeInHierarchy);
            if(laserPointer.activeInHierarchy)
            {
                laserPointer.transform.localPosition = Vector3.zero;
            }
            else
            {
                laserPointer.transform.localPosition = new Vector3(0.0F, -10000, 0.0F);
            }
        
        }
    }

    void SnapRotation()
    {
        if(PlayerInfo.VRDeviceName == PlayerInfo.OCULUS_DEVICE)
        {
            float xAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, rightController).x;
            if(Mathf.Abs(xAxis) >= 0.9F)
            {
                if(!isRotating)
                {
                    int direction = 0;
                    isRotating = true;
                    if(xAxis > 0)
                    {
                        direction = 1;
                    }
                    else
                    {
                        direction = -1;
                    }
                    PlayerInfo.PlayerTransform.rotation = PlayerInfo.PlayerTransform.rotation * Quaternion.Euler(0, 45.0F * direction, 0);
                }
            }
            else
            {
                isRotating = false;
            }
        }
    }
}
