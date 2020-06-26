using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

// This script is put on the objects that represent the player's hands and allows
// the player to pick up or interact with objects containing the Interactable script.
public class ObjectInteraction : MonoBehaviour
{
    public SteamVR_Action_Boolean grabAction;
    public SteamVR_Behaviour_Pose pose;
    public OVRInput.Button grabButton = OVRInput.Button.PrimaryIndexTrigger;
    public OVRInput.Controller controller;
    
    private FixedJoint joint = null;
    private Interactable heldInteractable = null;
    private List<Interactable> touchingInteractables = new List<Interactable>();

    void Awake()
    {
        joint = this.gameObject.GetComponent<FixedJoint>();
        if(PlayerInfo.VRDeviceName == PlayerInfo.OPEN_VR_DEVICE)
        {
            pose = this.GetComponent<SteamVR_Behaviour_Pose>();
            grabAction = SteamVR_Actions._default.InteractUI;
        }
        else if(PlayerInfo.VRDeviceName == PlayerInfo.OCULUS_DEVICE)
        {
            if(this.gameObject.name == "LeftHandAnchor")
            {
                this.controller = OVRInput.Controller.LTouch;
            }
            else if(this.gameObject.name == "RightHandAnchor")
            {
                this.controller = OVRInput.Controller.RTouch;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerInfo.VRDeviceName == PlayerInfo.OPEN_VR_DEVICE)
        {
            if(grabAction.GetStateDown(pose.inputSource))
            {

                Pickup();
            }
            else if(grabAction.GetStateUp(pose.inputSource))
            {

                Drop();
            }
        }
        else if(PlayerInfo.VRDeviceName == PlayerInfo.OCULUS_DEVICE)
        {
            if(OVRInput.GetDown(grabButton, controller))
            {

                Pickup();
            }
            else if(OVRInput.GetUp(grabButton, controller))
            {

                Drop();
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Interactable interactable;
        if(other.TryGetComponent(out interactable))
        {
            touchingInteractables.Add(interactable);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        Interactable interactable;
        if(other.TryGetComponent(out interactable))
        {
            touchingInteractables.Remove(interactable);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Interactable interactable;
        if(collision.gameObject.TryGetComponent(out interactable))
        {
            if(!interactable.CanCollide)
            {
                ContactPoint contact = collision.GetContact(0);
                Physics.IgnoreCollision(contact.thisCollider, contact.otherCollider);
            }
        }
    }

    public void Pickup()
    {
        heldInteractable = GetNearestInteractable();

        if(!heldInteractable)
        {
            return;
        }

        if(heldInteractable.activeHand)
        {
            heldInteractable.activeHand.Drop();
        }

        joint.connectedBody = heldInteractable.GetComponent<Rigidbody>();
        heldInteractable.activeHand = this;
    }

    public void Drop()
    {
        if(!heldInteractable)
        {
            return;
        }

        if(PlayerInfo.VRDeviceName == PlayerInfo.OPEN_VR_DEVICE)
        {
            heldInteractable.GetComponent<Rigidbody>().velocity = pose.GetVelocity();
            heldInteractable.GetComponent<Rigidbody>().angularVelocity = pose.GetAngularVelocity();
        }
        else if(PlayerInfo.VRDeviceName == PlayerInfo.OCULUS_DEVICE)
        {
            heldInteractable.GetComponent<Rigidbody>().velocity = PlayerInfo.PlayerTransform.rotation * OVRInput.GetLocalControllerVelocity(controller);
            heldInteractable.GetComponent<Rigidbody>().angularVelocity = OVRInput.GetLocalControllerAngularVelocity(controller);
        }
        
        joint.connectedBody = null;
        heldInteractable.activeHand = null;
        heldInteractable = null;
    }

    private Interactable GetNearestInteractable()
    {
        Interactable nearest = null;
        float minDistance = float.MaxValue;
        float distance = 0.0F;

        foreach(Interactable interactable in touchingInteractables)
        {
            distance = (interactable.transform.position - this.transform.position).sqrMagnitude;
            if(distance < minDistance)
            {
                minDistance = distance;
                nearest = interactable;
            }
        }
        return nearest;
    }

}
