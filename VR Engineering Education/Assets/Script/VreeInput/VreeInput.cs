using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using OVR;

public class VreeInput : MonoBehaviour
{

    // The string returned by VRDeviceName if an Oculus VR device is detected.
    private const string LOADED_DEVICE_OCULUS = "Oculus";

    // The string returned by VRDeviceName if a Steam VR device is detected.
    private const string LOADED_DEVICE_OPENVR = "OpenVR";

    // The string returned by VRDeviceName if no VR device is detected.
    private const string LOADED_DEVICE_NONE = "";
    private static PlayerType playerType = PlayerType.Unknown;

    /// <summary>
    /// The input devices available across VR and PC.
    /// </summary>
    public enum InputDevice
    {
        // No input device.
        None,

        // The left VR controller.
        LeftController,

        // The right VR controller.
        RightController,

        // Both left and right VR controllers.
        EitherController,

        // The Head Mounted Display.
        HMD,

        // The keyboard of a PC player.
        Keyboard,

        // The mouse of the PC player.
        Mouse

    }

    public enum VRInputType
    {
        // A default for when no input is selected.
        None,

        // A boolean button.
        Button,

        // A 0.0 - 0.1 float. 
        Axis1D,

        // A 0.0 - 0.1 Vector2
        Axis2D,

        // Anything that involves the position/orientation of a physical
        // device.
        Tracked
    }

    /// <summary>
    /// The inputs available on VR controllers that are mapped to boolean 
    /// values. These are either pressed or unpressed.
    /// </summary>
    public enum Button
    {
        // A default for when no input is selected.
        None,

        /// When the thumbstick/trackpad has been pressed.    
        ThumbStick,

        // The A / X button on oculus devices, or a right click on the Vive
        // trackpad.
        Button1,

        // The B / Y button on oculus devices, or a bottom click on the Vive
        // trackpad.
        Button2,

        // The Menu button on both oculus and Vive devices. This will always
        // return false for the right oculus controller, since it does not have
        // a menu button.
        Menu,

        // The trigger that is pressed by the index finger.
        IndexTrigger,

        // The trigger that is pressed by squeezing the grip.
        GripTrigger,

        // Up on the thumbstick/trackpad.
        Up,

        // Down on the thumbstick/trackpad.
        Down,

        // Left on the thumbstick/trackpad.
        Left,

        // Right on the thumbstick/trackpad.
        Right
    }

    /// <summary>
    /// The inputs available on VR controllers that are mapped to a float
    /// between 0.0 and 1.0.
    /// </summary>
    public enum Axis1D
    {
        // A default for when no input is selected.
        None,

        // The X (Horizontal) direction on the thumbstick/trackpad.
        ThumbstickX,

        // The Y (Vertical) direction on the thumbstick/trackpad.
        ThumbstickY,

        // The trigger that is pressed by the index finger.
        IndexTrigger,

        // The trigger that is presed by squeezing the grips.
        GripTrigger
    }

    /// <summary>
    /// The inputs available on VR controllers that are mapped to a Vector2,
    /// with the x and y values between 0.0 and 1.0.
    /// </summary>
    public enum Axis2D
    {
        // A default for when no input is selected.
        None,

        // The thumbstick/trackpad input.
        Thumbstick

    }

    /// <summary>
    /// Tracked data recieved by the VR devices.
    /// </summary>
    public enum Tracked
    {
        // A default for when no input is selected.
        None,

        // The transform of the object
        Transform,

        // The position of the tracked object, mapped to a Vector3.
        Position,

        // The rotation of the tracked object, mapped to a Quaternion
        Rotation,

        // The velocity of the tracked object, mapped to a Vector3.
        Velocity,

        // The angular velocity of the tracked object, mapped to a Vector3.
        AngularVelocity
    }

    public enum PlayerType
    {
        Unknown,
        PC,
        SteamVR,
        OculusVR,

        // This value is to be used as a parameter of the VreeInput.Get..
        // methods, but GetPlayerType() will never return this value.
        AnyVR,
        All
    }

    /// <summary>
    /// Returns the currently loaded PlayerType. Note that this will never
    /// return PlayerType.Unknown.
    /// </summary>
    /// <returns>The currently loaded PlayerType.</returns>
    public static PlayerType GetPlayerType()
    {
            PlayerType type = PlayerType.Unknown;
            if(playerType == PlayerType.Unknown)
            {
                string loadedDevice = XRSettings.loadedDeviceName;

                switch(loadedDevice)
                {
                    case LOADED_DEVICE_OCULUS:
                    type = PlayerType.OculusVR;
                    break;
                    case LOADED_DEVICE_OPENVR:
                        type = PlayerType.SteamVR;
                        break;
                    case LOADED_DEVICE_NONE:
                    default:
                        type = PlayerType.PC;
                        break;
                }
                playerType = type;
            }
        return playerType;
    }


    public static bool GetButton(VreeInput.InputDevice device, VreeInput.Button button, VreeInput.InputDevice secondDevice = VreeInput.InputDevice.None, VreeInput.Button secondDeviceButton = VreeInput.Button.None)
    {
        bool result1 = false;
        bool result2 = false;
        switch(GetPlayerType())
        {
            case PlayerType.OculusVR:
                result1 = VreeInputOculusBridge.GetButton(device, button);
                result2 = VreeInputOculusBridge.GetButton(secondDevice, secondDeviceButton);
                break;
            case PlayerType.SteamVR:
                result1 = VreeInputSteamVRBridge.GetButton(device, button);
                break;
            case PlayerType.PC:
            default:
                result1 = false;
                break;
        }
        return result1;
    }

    public static bool GetButtonDown(VreeInput.InputDevice device, VreeInput.Button button)
    {
        bool result = false;
        switch(GetPlayerType())
        {
            case PlayerType.OculusVR:
                result = VreeInputOculusBridge.GetButtonDown(device, button);
                break;
            case PlayerType.SteamVR:
                result = VreeInputSteamVRBridge.GetButtonDown(device, button);
                break;
            case PlayerType.PC:
                result = VreeInputPCBridge.GetButtonDown(device, button);
                break;
            default:
                result = false;
                break;
        }
        return result;
    }

    public static bool GetButtonUp(VreeInput.InputDevice device, VreeInput.Button button)
    {
        bool result = false;
        switch(GetPlayerType())
        {
            case PlayerType.SteamVR:
                result = VreeInputSteamVRBridge.GetButtonUp(device, button);
                break;
            case PlayerType.OculusVR:
                result = VreeInputOculusBridge.GetButtonUp(device, button);
                break;
            case PlayerType.PC:
                result = VreeInputPCBridge.GetButtonUp(device, button);
                break;
            default:
                result = false;
                break;
        }
        return result;
    }

    public static float GetAxis1D(VreeInput.InputDevice device, VreeInput.Axis1D axis)
    {
        float result = 0.0F;

        switch(GetPlayerType())
        {
            case PlayerType.SteamVR:
                result = VreeInputSteamVRBridge.GetAxis1D(device, axis);
                break;
            case PlayerType.OculusVR:
                result = VreeInputOculusBridge.GetAxis1D(device, axis);
                break;
            case PlayerType.PC:
                result = VreeInputPCBridge.GetAxis1D(device, axis);
                break;
            default:
                result = 0.0F;
                break;
        }
        return result;
    }

    public static Vector2 GetAxis2D(VreeInput.InputDevice device, VreeInput.Axis2D axis)
    {
        Vector2 result = Vector2.zero;

        switch(GetPlayerType())
        {
            case PlayerType.SteamVR:
                result = VreeInputSteamVRBridge.GetAxis2D(device, axis);
                break;
            case PlayerType.OculusVR:
                result = VreeInputOculusBridge.GetAxis2D(device, axis);
                break;
            case PlayerType.PC:
                result = VreeInputPCBridge.GetAxis2D(device, axis);
                break;
            default:
                result = Vector2.zero;
                break;
        }
        return result;
    }

    public static Transform GetTransform(VreeInput.InputDevice device)
    {
        Transform result = null;
        switch(GetPlayerType())
        {
            case PlayerType.SteamVR:
                result = VreeInputSteamVRBridge.GetTransform(device);
                break;
            case PlayerType.OculusVR:
                result = VreeInputOculusBridge.GetTransform(device);
                break;
            case PlayerType.PC:
                result = VreeInputPCBridge.GetTransform(device);
                break;
            default:
                result = null;
                break;
        }
        return result;
    }

    public static Vector3 GetPosition(VreeInput.InputDevice device)
    {
        Vector3 result = Vector3.zero;
        switch(GetPlayerType())
        {
            case PlayerType.SteamVR:
                result = VreeInputSteamVRBridge.GetPosition(device);
                break;
            case PlayerType.OculusVR:
                result = VreeInputOculusBridge.GetPosition(device);
                break;
            case PlayerType.PC:
                result = VreeInputPCBridge.GetPosition(device);
                break;
            default:
                result = Vector3.zero;
                break;
        }
        return result;
    }

    public static Quaternion GetRotation(VreeInput.InputDevice device)
    {
        Quaternion result = Quaternion.identity;
        switch(GetPlayerType())
        {
            case PlayerType.SteamVR:
                result = VreeInputSteamVRBridge.GetRotation(device);
                break;
            case PlayerType.OculusVR:
                result = VreeInputOculusBridge.GetRotation(device);
                break;
            case PlayerType.PC:
                result = VreeInputPCBridge.GetRotation(device);
                break;
            default:
                result = Quaternion.identity;
                break;
        }
        return result;
    }

    public static float GetVelocity(VreeInput.InputDevice device)
    {
        return 0.0F;
    }

    public static float GetAngularVelocity(VreeInput.InputDevice device)
    {
        return 0.0F;
    }
}