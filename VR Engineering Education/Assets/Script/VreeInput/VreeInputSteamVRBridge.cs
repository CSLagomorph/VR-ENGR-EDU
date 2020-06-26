using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

/// <summary>
/// A class that is meant to be a way to connect VreeInput to the OpenVR API,
/// converting the VreeInput calls into SteamVR calls.
/// </summary>
public static class VreeInputSteamVRBridge
{
    
    public const string HMD_OBJECT_NAME = "FollowHead";
    public const string LEFT_HAND_OBJECT_NAME = "LeftHand";
    public const string RIGHT_HAND_OBJECT_NAME = "RightHand";
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
        if(device == VreeInput.InputDevice.None || button == VreeInput.Button.None)
        {
            return false;
        }

        // A check to see if the device is the headset.
        if(device == VreeInput.InputDevice.HMD)
        {
            return false;
        }

        // SteamVR uses action sets to map actions to actual inputs. The action
        // set must be active for the actions to actually register any input.
        if(!SteamVR_Actions.VreeInput.IsActive(SteamVR_Input_Sources.Any))
        {
            SteamVR_Actions.VreeInput.Activate(SteamVR_Input_Sources.Any, 0, false);
        }

        bool result = false;
        SteamVR_Input_Sources inputSource = GetController(device);
        SteamVR_Action_Boolean action = GetButton(button);

        if(action == SteamVR_Actions.vreeInput_NoButton)
        {
            result = GetAxisButton(device, button);
        }
        else
        {
            result = action.GetState(inputSource);
        }
        
        return result;
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
    public static bool GetButtonDown(VreeInput.InputDevice device, VreeInput.Button button)
    {
        // A check for None in either the device or button, which means it can't
        // have a value.
        if(device == VreeInput.InputDevice.None || button == VreeInput.Button.None)
        {
            return false;
        }

        // A check to see if the device is the headset.
        if(device == VreeInput.InputDevice.HMD)
        {
            return false;
        }

        // SteamVR uses action sets to map actions to actual inputs. The action
        // set must be active for the actions to actually register any input.
        if(!SteamVR_Actions.VreeInput.IsActive(SteamVR_Input_Sources.Any))
        {
            SteamVR_Actions.VreeInput.Activate(SteamVR_Input_Sources.Any, 0, false);
        }
        bool result = false;
        SteamVR_Input_Sources inputSource = GetController(device);
        SteamVR_Action_Boolean action = GetButton(button);

        // A real button wasn't found, so it is likely a directional axis
        // button, and since they aren't really buttons, but combinations of
        // the thumbpad position and press, they are handled differently.
        if(action == SteamVR_Actions.vreeInput_NoButton)
        {
            result = GetAxisButtonDown(device, button);
        }
        else // Button found, check it directly
        {
            result = action.GetStateDown(inputSource);
        }
        
        return result;
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
        if(device == VreeInput.InputDevice.None || button == VreeInput.Button.None)
        {
            return false;
        }

        // A check to see if the device is the headset.
        if(device == VreeInput.InputDevice.HMD)
        {
            return false;
        }

        // SteamVR uses action sets to map actions to actual inputs. The action
        // set must be active for the actions to actually register any input.
        if(!SteamVR_Actions.VreeInput.IsActive(SteamVR_Input_Sources.Any))
        {
            SteamVR_Actions.VreeInput.Activate(SteamVR_Input_Sources.Any, 0, false);
        }
        bool result = false;
        SteamVR_Input_Sources inputSource = GetController(device);
        SteamVR_Action_Boolean action = GetButton(button);

        if(action == SteamVR_Actions.vreeInput_NoButton)
        {
            result = GetAxisButtonUp(device, button);
        }
        else
        {
            result = action.GetStateUp(inputSource);
        }
        
        return result;
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
        if(device == VreeInput.InputDevice.None || axis == VreeInput.Axis1D.None)
        {
            return 0;
        }

        // A check to see if the device is the headset.
        if(device == VreeInput.InputDevice.HMD)
        {
            return 0;
        }

        // SteamVR uses action sets to map actions to actual inputs. The action
        // set must be active for the actions to actually register any input.
        if(!SteamVR_Actions.VreeInput.IsActive(SteamVR_Input_Sources.Any))
        {
            SteamVR_Actions.VreeInput.Activate(SteamVR_Input_Sources.Any, 0, false);
        }

        float result = 0;
        SteamVR_Input_Sources inputSource = GetController(device);
        if(axis == VreeInput.Axis1D.ThumbstickX)
        {
            result = SteamVR_Actions.vreeInput_Thumbpad.GetAxis(inputSource).x;
        }
        else if(axis == VreeInput.Axis1D.ThumbstickY)
        {
            result = SteamVR_Actions.vreeInput_Thumbpad.GetAxis(inputSource).y;
        }
        else
        {
            SteamVR_Action_Single action = GetAxis1D(axis);
            result = action.GetAxis(inputSource);
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
        if(device == VreeInput.InputDevice.None || axis == VreeInput.Axis2D.None)
        {
            return Vector2.zero;
        }

        // A check to see if the device is the headset.
        if(device == VreeInput.InputDevice.HMD)
        {
            return Vector2.zero;
        }

        // SteamVR uses action sets to map actions to actual inputs. The action
        // set must be active for the actions to actually register any input.
        if(!SteamVR_Actions.VreeInput.IsActive(SteamVR_Input_Sources.Any))
        {
            SteamVR_Actions.VreeInput.Activate(SteamVR_Input_Sources.Any, 0, false);
        }

        SteamVR_Input_Sources inputSource = GetController(device);
        SteamVR_Action_Vector2 action = GetAxis2D(axis);
        return action.GetAxis(inputSource);
    }

    /// <summary>
    /// Returns the OpenVR API's representation of the given device. This is 
    /// used to relay the input checks requested using Vree to the OpenVR API.
    /// </summary>
    /// <param name="device">The device to get from the OpenVR API.</param>
    /// <returns>
    /// Returns the OpenVR API's representation of the given device. This is 
    /// used to relay the input checks requested using Vree to the OpenVR API.
    /// If the given device is not supported or is not found on that OpenVR 
    /// device, it returns a SteamVR_Input_Sources with the value -1.
    /// </returns>
    private static SteamVR_Input_Sources GetController(VreeInput.InputDevice device)
    {
        // -1 is not defined within SteamVR_Input_Sources, and is only used here
        // to represent VreeInput.VRInputDevice.None
        SteamVR_Input_Sources controller = (SteamVR_Input_Sources)(-1);

        switch(device)
        {
            case VreeInput.InputDevice.LeftController:
                controller = SteamVR_Input_Sources.LeftHand;
                break;
            case VreeInput.InputDevice.RightController:
                controller = SteamVR_Input_Sources.RightHand;
                break;
            case VreeInput.InputDevice.EitherController:
                controller = SteamVR_Input_Sources.Any;
                break;
            case VreeInput.InputDevice.None:
            default:
                controller = (SteamVR_Input_Sources)(-1);
                break;
        }

        return controller;
    }

    /// <summary>
    /// Returns the OpenVR API's representation of the given button. This is 
    /// used to relay the input checks requested using Vree to the OpenVR API.
    /// </summary>
    /// <param name="button">The button to get from the OpenVR API.</param>
    /// <returns>
    /// Returns the OpenVR API's representation of the given button. This is 
    /// used to relay the input checks requested using Vree to the OpenVR API.
    /// If the given button is not supported or is not found on that OpenVR 
    /// device, it returns <c>SteamVR_Actions.vreeInput_NoButton</c>.
    /// </returns>
    private static SteamVR_Action_Boolean GetButton(VreeInput.Button button)
    {
        SteamVR_Action_Boolean action = SteamVR_Actions.vreeInput_Menu;

        switch(button)
        {
            case VreeInput.Button.ThumbStick:
                action = SteamVR_Actions.vreeInput_ThumbpadClick;
                break;
            case VreeInput.Button.IndexTrigger:
                action = SteamVR_Actions.vreeInput_IndexTriggerPress;
                break;
            case VreeInput.Button.GripTrigger:
                action = SteamVR_Actions.vreeInput_HandTriggerPress;
                break;
            case VreeInput.Button.Menu:
            default:
                action = SteamVR_Actions.vreeInput_NoButton;
                break;
        }

        return action;
    }

    /// <summary>
    /// Returns the OpenVR API's representation of the given axis. This is 
    /// used to relay the input checks requested using Vree to the OpenVR API.
    /// </summary>
    /// <param name="axis">The axis to get from the OpenVR API.</param>
    /// <returns>
    /// Returns the OpenVR API's representation of the given axis. This is 
    /// used to relay the input checks requested using Vree to the OpenVR API.
    /// </returns>
    private static SteamVR_Action_Single GetAxis1D(VreeInput.Axis1D axis)
    {
        SteamVR_Action_Single action = SteamVR_Actions.vreeInput_HandTrigger;

        switch(axis)
        {
            case VreeInput.Axis1D.GripTrigger:
                action = SteamVR_Actions.vreeInput_HandTrigger;
                break;
            case VreeInput.Axis1D.IndexTrigger:
            default:
                action = SteamVR_Actions.vreeInput_IndexTrigger;
                break;
            
        }
        return action;
    }

    /// <summary>
    /// Returns the OpenVR API's representation of the given axis. This is 
    /// used to relay the input checks requested using Vree to the OpenVR API.
    /// </summary>
    /// <param name="axis">The axis to get from the OpenVR API.</param>
    /// <returns>
    /// Returns the OpenVR API's representation of the given axis. This is 
    /// used to relay the input checks requested using Vree to the OpenVR API.
    /// </returns>
    private static SteamVR_Action_Vector2 GetAxis2D(VreeInput.Axis2D axis)
    {
        SteamVR_Action_Vector2 action = SteamVR_Actions.vreeInput_Thumbpad;
        return action;
    }

    /// <summary>
    /// Returns the value of the button that is mapped to the thumbpad being at
    /// specific positions. Since Oculus has multiple buttons that are not 
    /// present on many OpenVR controllers, this uses a combination of the 
    /// thumbpad position and thumbpad clicks to determine these values.
    /// </summary>
    /// <param name="button">The state of the given button.</param>
    /// <returns>
    /// Returns true if the given button is currently pressed, false otherwise.
    /// </returns>
    private static bool GetAxisButton(VreeInput.InputDevice device, VreeInput.Button button)
    {
        Vector2 axis = GetAxis2D(device, VreeInput.Axis2D.Thumbstick);
        bool result = false;
        
        switch(button)
        {
            case VreeInput.Button.Button1:
                result = GetButton(device, VreeInput.Button.Right) && 
                         GetButton(device, VreeInput.Button.ThumbStick);
                break;
            case VreeInput.Button.Button2:
                result = GetButton(device, VreeInput.Button.Down) &&
                         GetButton(device, VreeInput.Button.ThumbStick);
                break;
            case VreeInput.Button.Up:
                result = axis.y >= 0.5F;
                break;
            case VreeInput.Button.Down:
                result = axis.y <= -0.5F;
                break;
            case VreeInput.Button.Left:
                result = axis.x <= -0.5F;
                break;
            case VreeInput.Button.Right:
                result = axis.x >= 0.5F;
                
                break;
            default:
                result = false;
                break;
        }
        return result;
    }

    /// <summary>
    /// Returns true if the given button was pressed this frame. Since Oculus 
    /// has multiple buttons that are not present on many OpenVR controllers, 
    /// this uses a combination of the thumbpad position and thumbpad clicks to 
    /// determine these values.
    /// </summary>
    /// <param name="device">
    /// The device to get the state of the given button on.
    /// </param>
    /// <param name="button">The button to get the state of.</param>
    /// <returns>
    /// Returns true if the given button on the given controller was pressed
    /// this frame, false otherwise.
    /// </returns>
    private static bool GetAxisButtonDown(VreeInput.InputDevice device, VreeInput.Button button)
    {
        SteamVR_Input_Sources inputSource = GetController(device);
        SteamVR_Action_Vector2 action = GetAxis2D(VreeInput.Axis2D.Thumbstick);
        Vector2 currentAxis = action.GetAxis(inputSource);
        Vector2 lastAxis = action.GetLastAxis(inputSource);
        bool result = false;

        switch(button)
        {
            case VreeInput.Button.Button1:
                result = GetButton(device, VreeInput.Button.Right) && 
                         GetButtonDown(device, VreeInput.Button.ThumbStick);
                break;
            case VreeInput.Button.Button2:
                result = GetButton(device, VreeInput.Button.Down) &&
                         GetButtonDown(device, VreeInput.Button.ThumbStick);
                break;
            case VreeInput.Button.Up:
                result = lastAxis.y >= 0.5F && currentAxis.y < 0.5F;
                break;
            case VreeInput.Button.Down:
                result = lastAxis.y > -0.5F && currentAxis.y <= -0.5F;
                break;
            case VreeInput.Button.Left:
                result = lastAxis.x <= -0.5F && currentAxis.x > -0.5F;
                break;
            case VreeInput.Button.Right:
                result = lastAxis.x < 0.5F && currentAxis.x >= 0.5F;
                break;
            default:
                result = false;
                break;
        }
        return result;
    }

    /// <summary>
    /// Returns true if the given button was released this frame. Since Oculus 
    /// has multiple buttons that are not present on many OpenVR controllers, 
    /// this uses a combination of the thumbpad position and thumbpad clicks to 
    /// determine these values.
    /// </summary>
    /// <param name="device">
    /// The device to get the state of the given button on.
    /// </param>
    /// <param name="button">The button to get the state of.</param>
    /// <returns>
    /// Returns true if the given button on the given controller was released
    /// this frame, false otherwise.
    /// </returns>
    private static bool GetAxisButtonUp(VreeInput.InputDevice device, VreeInput.Button button)
    {
        SteamVR_Input_Sources inputSource = GetController(device);
        SteamVR_Action_Vector2 action = GetAxis2D(VreeInput.Axis2D.Thumbstick);
        Vector2 currentAxis = action.GetAxis(inputSource);
        Vector2 lastAxis = action.GetLastAxis(inputSource);
        bool result = false;

        switch(button)
        {
            case VreeInput.Button.Button1:
                result = GetButton(device, VreeInput.Button.Right) && 
                         GetButtonUp(device, VreeInput.Button.ThumbStick);
                break;
            case VreeInput.Button.Button2:
                result = GetButton(device, VreeInput.Button.Down) &&
                         GetButtonUp(device, VreeInput.Button.ThumbStick);
                break;
            case VreeInput.Button.Up:
                result = lastAxis.y < 0.5F && currentAxis.y >= 0.5F;
                break;
            case VreeInput.Button.Down:
                result = lastAxis.y <= -0.5F && currentAxis.y > -0.5F;
                break;
            case VreeInput.Button.Left:
                result = lastAxis.x > -0.5F && currentAxis.x <= -0.5F;
                break;
            case VreeInput.Button.Right:
                result = lastAxis.x >= 0.5F && currentAxis.x < 0.5F;
                break;
            default:
                result = false;
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
