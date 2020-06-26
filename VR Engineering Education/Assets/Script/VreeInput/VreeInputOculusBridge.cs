using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class that is meant to be a way to connect VreeInput to the OculusVR API,
/// converting the VreeInput calls into OculusVR calls.
/// </summary>
public static class VreeInputOculusBridge
{

    public const string HMD_OBJECT_NAME = "CenterEyeAnchor";
    public const string LEFT_HAND_OBJECT_NAME = "LeftHandAnchor";
    public const string RIGHT_HAND_OBJECT_NAME = "RightHandAnchor";
    public const string PLAYER_OBJECT_NAME = "player";

    private static GameObject playerObject;
    private static GameObject hmdObject;
    private static GameObject leftHandObject;
    private static GameObject rightHandObject;

    public static GameObject PlayerObject
    {
        get
        {
            if(playerObject == null)
            {
                playerObject = GameObject.Find(PLAYER_OBJECT_NAME);
            }
            return playerObject;
        }
    }

    public static GameObject LeftHandObject
    {
        get
        {
            if(leftHandObject == null)
            {
                leftHandObject = GameObject.Find(LEFT_HAND_OBJECT_NAME);
            }
            return leftHandObject;
        }
    }

    public static GameObject RightHandObject
    {
        get
        {
            if(rightHandObject == null)
            {
                rightHandObject = GameObject.Find(RIGHT_HAND_OBJECT_NAME);
            }
            return rightHandObject;
        }
    }

    public static GameObject HMDObject
    {
        get
        {
            if(hmdObject == null)
            {
                hmdObject = GameObject.Find(HMD_OBJECT_NAME);
            }
            return hmdObject;
        }
    }

    /// <summary>
    /// Gets the current state of the given button on the given device.
    /// </summary>
    /// <param name="device">The device to check for the input on.</param>
    /// <param name="button">The button whose state is returned.</param>
    /// <returns>
    /// True if the given button is held down on the given device. Returns false 
    /// if the button doesn't exist on the given device, or if either the device
    /// or button is not set.
    /// </returns>
    public static bool GetButton(VreeInput.InputDevice device, VreeInput.Button button)
    {
        // A check for None in either the device or button, which means it can't
        // have a value.
        if(button == VreeInput.Button.None || device == VreeInput.InputDevice.None)
        {
            return false;
        }

        // A check to see if trying to get the menu button on the right
        // controller. Oculus controllers don't have a menu button on the right.
        if(button == VreeInput.Button.Menu && device == VreeInput.InputDevice.RightController)
        {
            return false;
        }

        // A check to see if the device is the headset.
        if(device == VreeInput.InputDevice.HMD)
        {
            return false;
        }

        OVRInput.Controller ovrController = GetController(device);
        OVRInput.Button ovrButton = GetButton(button);

        return OVRInput.Get(ovrButton, ovrController);
    }

    /// <summary>
    /// Checks if the given button was pressed on the given controller on this 
    /// frame.
    /// </summary>
    /// <param name="device">The device to check for the input on.</param>
    /// <param name="button">The button whose state is returned.</param>
    /// <returns>
    /// Returns true if the given button on the given controller was unpressed
    /// last frame, and pressed this frame. Returns false if the button doesn't
    /// exist on the given device, or if either the device or button is not set.
    /// </returns>
    public static bool GetButtonDown(VreeInput.InputDevice device, VreeInput.Button button)
    {
        // A check for None in either the device or button, which means it can't
        // have a value.
        if(button == VreeInput.Button.None || device == VreeInput.InputDevice.None)
        {
            return false;
        }

        // A check to see if trying to get the menu button on the right
        // controller. Oculus controllers don't have a menu button on the right.
        if(button == VreeInput.Button.Menu && device == VreeInput.InputDevice.RightController)
        {
            return false;
        }

        // A check to see if the device is the headset.
        if(device == VreeInput.InputDevice.HMD)
        {
            return false;
        }

        OVRInput.Controller ovrController = GetController(device);
        OVRInput.Button ovrButton = GetButton(button);

        return OVRInput.GetDown(ovrButton, ovrController);
    }

    /// <summary>
    /// Checks if the given button was unpressed on the given controller on this 
    /// frame.
    /// </summary>
    /// <param name="device">The device to check for the input on.</param>
    /// <param name="button">The button whose state is returned.</param>
    /// <returns>
    /// Returns true if the given button on the given controller was pressed
    /// last frame, and unpressed this frame. Returns false if the button 
    /// doesn't exist on the given device, or if either the device or button is
    /// not set.
    /// </returns>
    public static bool GetButtonUp(VreeInput.InputDevice device, VreeInput.Button button)
    {
        // A check for None in either the device or button, which means it can't
        // have a value.
        if(button == VreeInput.Button.None || device == VreeInput.InputDevice.None)
        {
            return false;
        }

        // A check to see if trying to get the menu button on the right
        // controller. Oculus controllers don't have a menu button on the right.
        if(button == VreeInput.Button.Menu && device == VreeInput.InputDevice.RightController)
        {
            return false;
        }

        // A check to see if the device is the headset.
        if(device == VreeInput.InputDevice.HMD)
        {
            return false;
        }

        OVRInput.Controller ovrController = GetController(device);
        OVRInput.Button ovrButton = GetButton(button);

        return OVRInput.GetUp(ovrButton, ovrController);
    }

    /// <summary>
    /// Gets the state of the given Input Axis on the given device.
    /// </summary>
    /// <param name="device">The device to check for the input on.</param>
    /// <param name="axis"> The axis whose state is returned.</param>
    /// <returns>
    /// Returns a float between 0.0 and 1.0 for trigger and grip axes, that
    /// represent how far the asis is pressed, with 1.0 being fully pressed and
    /// 0.0 being fully unpressed.
    /// For the Thumbstick axis, returns a float between -1.0 and 1.0, with
    /// 0.0 being the stick is in the center resting position, and the 1.0s
    /// being the stick is at the edge.
    /// </returns>
    public static float GetAxis1D(VreeInput.InputDevice device, VreeInput.Axis1D axis)
    {
        // A check for None in either the device or axis, which means it can't
        // have a value.
        if(axis == VreeInput.Axis1D.None || device == VreeInput.InputDevice.None)
        {
            return 0;
        }

        // A check to see if the device is the headset.
        if(device == VreeInput.InputDevice.HMD)
        {
            return 0;
        }
        float result;

        OVRInput.Controller ovrController = GetController(device);
        if(axis == VreeInput.Axis1D.ThumbstickX)
        {
            result = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, ovrController).x;
        }
        else if(axis == VreeInput.Axis1D.ThumbstickY)
        {
            result = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, ovrController).y;
        }
        else
        {
            OVRInput.Axis1D ovrAxis = GetAxis1D(axis);
            result = OVRInput.Get(ovrAxis, ovrController);
        }
        return result;
        
    }

    /// <summary>
    /// Gets the state of the given Input Axis on the given device.
    /// </summary>
    /// <param name="device">The device to check for the input on.</param>
    /// <param name="axis"> The axis whose state is returned.</param>
    /// <returns>
    /// Returns a Vector2, whose x and y values are floats between -1.0 and 1.0,
    /// with 0.0 being the stick is in the center resting position, and the 1.0s
    /// being the stick is at the edge of the axis.
    /// </returns>
    public static Vector2 GetAxis2D(VreeInput.InputDevice device, VreeInput.Axis2D axis)
    {
        // A check for None in either the device or axis, which means it can't
        // have a value.
        if(axis == VreeInput.Axis2D.None || device == VreeInput.InputDevice.None)
        {
            return Vector2.zero;
        }

        // A check to see if the device is the headset.
        if(device == VreeInput.InputDevice.HMD)
        {
            return Vector2.zero;
        }

        OVRInput.Controller ovrController = GetController(device);
        OVRInput.Axis2D ovrAxis = GetAxis2D(axis);
        return OVRInput.Get(ovrAxis, ovrController);
    }

    /// <summary>
    /// Returns the Oculus API's representation of the given VreeInput device.
    /// This is used to relay the input checks requested using Vree to the
    /// Oculus API.
    /// </summary>
    /// <param name="device"> The device to get from the OVR API.</param>
    /// <returns>
    /// Returns the Oculus API's representation of the given VreeInput device.
    /// This is used to relay the input checks requested using Vree to the
    /// Oculus API. The given device isn't supported, or can't be found in an
    /// Oculus setup, this returns <c>OVRInput.Controller.None</c>.
    /// </returns>
    private static OVRInput.Controller GetController(VreeInput.InputDevice device)
    {
        OVRInput.Controller controller = OVRInput.Controller.None;
        switch(device)
        {
            case VreeInput.InputDevice.LeftController:
                controller = OVRInput.Controller.LTouch;
                break;
            case VreeInput.InputDevice.RightController:
                controller = OVRInput.Controller.RTouch;
                break;
            case VreeInput.InputDevice.EitherController:
                controller = OVRInput.Controller.All;
                break;
            case VreeInput.InputDevice.None:
            default: 
                controller = OVRInput.Controller.None;
                break;
        }
        return controller;
    }

    /// <summary>
    /// Returns the Oculus API's representation of the given VreeInput button.
    /// This is used to relay the input checks requested using Vree to the
    /// Oculus API.
    /// </summary>
    /// <param name="button"> The button to get from the OVR API.</param>
    /// <returns>
    /// Returns the Oculus API's representation of the given VreeInput button.
    /// This is used to relay the input checks requested using Vree to the
    /// Oculus API. The given button isn't supported, or can't be found in an
    /// Oculus setup, this returns <c>OVRInput.Button.None</c>.
    /// </returns>
    private static OVRInput.Button GetButton(VreeInput.Button button)
    {
        OVRInput.Button result = OVRInput.Button.None;
        switch(button)
        {
            case VreeInput.Button.Button1:
                result = OVRInput.Button.One;
                break;
            case VreeInput.Button.Button2:
                result = OVRInput.Button.Two;
                break;
            case VreeInput.Button.ThumbStick:
                result = OVRInput.Button.PrimaryThumbstick;
                break;
            case VreeInput.Button.GripTrigger:
                result = OVRInput.Button.PrimaryHandTrigger;
                break;
            case VreeInput.Button.IndexTrigger:
                result = OVRInput.Button.PrimaryIndexTrigger;
                break;
            case VreeInput.Button.Menu:
                result = OVRInput.Button.Start;
                break;
            case VreeInput.Button.Up:
                result = OVRInput.Button.PrimaryThumbstickUp;
                break;
            case VreeInput.Button.Down:
                result = OVRInput.Button.PrimaryThumbstickDown;
                break;
            case VreeInput.Button.Left:
                result = OVRInput.Button.PrimaryThumbstickLeft;
                break;
            case VreeInput.Button.Right:
                result = OVRInput.Button.PrimaryThumbstickRight;
                break;
            case VreeInput.Button.None:
            default: 
                result = OVRInput.Button.None;
                break;
        }
        return result;
    }

    /// <summary>
    /// Returns the Oculus API's representation of the given VreeInput axis.
    /// This is used to relay the input checks requested using Vree to the
    /// Oculus API.
    /// </summary>
    /// <param name="axis">The axis to get from the OVR API.</param>
    /// <returns>
    /// Returns the Oculus API's representation of the given VreeInput axis.
    /// This is used to relay the input checks requested using Vree to the
    /// Oculus API. The given axis isn't supported, or can't be found in an
    /// Oculus setup, this returns <c>OVRInput.Axis1D.None</c>.
    /// </returns>
    private static OVRInput.Axis1D GetAxis1D(VreeInput.Axis1D axis)
    {
        OVRInput.Axis1D result = OVRInput.Axis1D.None;
        switch(axis)
        {
            case VreeInput.Axis1D.GripTrigger:
                result = OVRInput.Axis1D.PrimaryHandTrigger;
                break;
            case VreeInput.Axis1D.IndexTrigger:
                result = OVRInput.Axis1D.PrimaryIndexTrigger;
                break;
            case VreeInput.Axis1D.None:
            default:
                result = OVRInput.Axis1D.None;
                break;
        }
        return result;
    }

    /// <summary>
    /// Returns the Oculus API's representation of the given VreeInput axis.
    /// This is used to relay the input checks requested using Vree to the
    /// Oculus API.
    /// </summary>
    /// <param name="axis">The axis to get from the OVR API.</param>
    /// <returns>
    /// Returns the Oculus API's representation of the given VreeInput axis.
    /// This is used to relay the input checks requested using Vree to the
    /// Oculus API. The given axis isn't supported, or can't be found in an
    /// Oculus setup, this returns <c>OVRInput.Axis2D.None</c>.
    /// </returns>
    private static OVRInput.Axis2D GetAxis2D(VreeInput.Axis2D axis)
    {
        OVRInput.Axis2D result = OVRInput.Axis2D.None;
        switch(axis)
        {
            case VreeInput.Axis2D.Thumbstick:
                result = OVRInput.Axis2D.PrimaryThumbstick;
                break;
            case VreeInput.Axis2D.None:
            default:
                result = OVRInput.Axis2D.None;
                break;
        }
        return result;
    }

    public static Transform GetTransform(VreeInput.InputDevice device)
    {
        Transform result = null;
        switch(device)
        {
            case VreeInput.InputDevice.HMD:
                result = HMDObject.transform;
                break;
            case VreeInput.InputDevice.LeftController:
                result = LeftHandObject.transform;
                break;
            case VreeInput.InputDevice.RightController:
                result = RightHandObject.transform;
                break;
            default:
                result = null;
                break;
        }
        return result;
    }

    public static Vector3 GetPosition(VreeInput.InputDevice device)
    {
        Transform transform = GetTransform(device);
        Vector3 result = Vector3.zero;
        if(transform != null)
        {
            result = transform.position;
        }
        return result;
    }

    public static Quaternion GetRotation(VreeInput.InputDevice device)
    {
        Transform transform = GetTransform(device);
        Quaternion result = Quaternion.identity;
        if(transform != null)
        {
            result = transform.rotation;
        }
        return result;
    }

    public static Transform GetPlayer()
    {
        return PlayerObject.transform;
    }
}
