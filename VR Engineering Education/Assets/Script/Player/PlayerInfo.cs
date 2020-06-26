using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR.InteractionSystem;

public class PlayerInfo : MonoBehaviour
{
    // The string returned by VRDeviceName if an Oculus VR device is detected.
    public const string OCULUS_DEVICE = "Oculus";

    // The string returned by VRDeviceName if a Steam VR device is detected.
    public const string OPEN_VR_DEVICE = "OpenVR";

    // The string returned by VRDeviceName if no VR device is detected.
    public const string NO_DETECTED_DEVICE = "";

    private static string _VRDeviceName = null;
    private static Transform _PlayerTransform = null;
    private static Transform _LaserPointer = null;
    private static Hand _LeftHand;
    private static Hand _RightHand;

    // A struct used to hold helpful information about VR hands, since they are
    // sometimes seperate within the player prefab or just confusing to find.
    public struct Hand
    {
        public Transform Transform;
        public Transform ObjectAttachmentPoint;
        public Collider Collider;
    }

    // This can be used to check if a VR device is connected and which
    // it is. It can be checked against PlayerInfo.OCULUS_DEVICE, PlayerInfo.OPEN_VR_DEVICE,
    // and PlayerInfo.NO_DETECTED_DEVICE.
    public static string VRDeviceName
    {
        get
        {
            if(_VRDeviceName == null)
            {
                _VRDeviceName = XRSettings.loadedDeviceName;
            }
            return _VRDeviceName;
        }
    }

    // This is used to get a reference to the current player. Since this is a static call, and works
    // regardless of which VR device is connected, this should be the go-to for referencing the player
    // from a different object.
    public static Transform PlayerTransform
    {
        get
        {
            if(_PlayerTransform == null)
            {
                _PlayerTransform = Object.FindObjectOfType<PlayerBase>().transform;
            }
            return _PlayerTransform;
        }
    }

    public static Transform LaserPointer
    {
        get
        {
            if((VRDeviceName == OCULUS_DEVICE || VRDeviceName == OPEN_VR_DEVICE) && _LaserPointer == null)
            {
                _LaserPointer = GameObject.Find("LaserPointer").transform;
            }
            return _LaserPointer;
        }
    }

    public static Hand LeftHand
    {
        get
        {
            if(_LeftHand.Transform == null)
            {
                if(VRDeviceName == OCULUS_DEVICE)
                {
                    _LeftHand = new Hand();
                    //_LeftHand.Trans = GameObject.Find("LeftHandAnchor").transform;
                }
                else if(VRDeviceName == OPEN_VR_DEVICE)
                {
                    _LeftHand = new Hand();
                    _LeftHand.Transform = GameObject.Find("LeftHand").transform;
                    _LeftHand.ObjectAttachmentPoint = _LeftHand.Transform.Find("ObjectAttachmentPoint");
                    _LeftHand.Collider = _LeftHand.Transform.gameObject.GetComponent<SphereCollider>();
                }
            }
            return _LeftHand;
        }
    }

    public static Hand RightHand
    {
        get
        {
            if(_RightHand.Transform == null)
            {
                if(VRDeviceName == OCULUS_DEVICE)
                {
                    _RightHand = new Hand();//GameObject.Find("RightHandAnchor").transform;
                }
                else if(VRDeviceName == OPEN_VR_DEVICE)
                {
                    _RightHand = new Hand();
                    _RightHand.Transform = GameObject.Find("RightHand").transform;
                    _RightHand.ObjectAttachmentPoint = _RightHand.Transform.Find("ObjectAttachmentPoint");
                    _RightHand.Collider = _RightHand.Transform.gameObject.GetComponent<SphereCollider>();
                }
            }
            return _RightHand;
        }
    }
}
