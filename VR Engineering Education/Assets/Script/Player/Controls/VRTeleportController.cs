using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRTeleportController : MonoBehaviour
{

    private VRTeleport teleporter;
    //private VRTeleporter2 teleporter2;
    public SteamVR_Action_Boolean teleportAction = SteamVR_Actions._default.GrabPinch;
    public SteamVR_Behaviour_Pose pose;

    public OVRInput.Button teleportButton = OVRInput.Button.PrimaryIndexTrigger;
    public OVRInput.Controller controller = OVRInput.Controller.LTouch;

    
    void Start()
    {
        teleporter = this.GetComponent<VRTeleport>();
        //teleporter2 = this.GetComponent<VRTeleporter2>();
        //teleporter2.ToggleDisplay(false);
        //teleporter2.bodyTransforn = PlayerInfo.PlayerTransform;
        teleporter.SetActive(false);
        teleporter.playerTransform = PlayerInfo.PlayerTransform;
    }

    void Update()
    {
        if(PlayerInfo.VRDeviceName == PlayerInfo.OCULUS_DEVICE)
        {
            if(OVRInput.GetDown(teleportButton, controller))
            {
                StartTeleport();
            }
            if(OVRInput.GetUp(teleportButton, controller))
            {
                EndTeleport();
            }
        }
        else if(PlayerInfo.VRDeviceName == PlayerInfo.OPEN_VR_DEVICE)
        {
            if(teleportAction.GetStateDown(pose.inputSource))
            {
                StartTeleport();
            }
            if(teleportAction.GetStateUp(pose.inputSource))
            {
                EndTeleport();
            }
        }
    }

    void StartTeleport()
    {
        teleporter.SetActive(true);
        //teleporter2.ToggleDisplay(true);
    }

    void EndTeleport()
    {
        teleporter.Teleport();
        teleporter.SetActive(false);
        //teleporter2.Teleport();
        //teleporter2.ToggleDisplay(false);
    }
}
