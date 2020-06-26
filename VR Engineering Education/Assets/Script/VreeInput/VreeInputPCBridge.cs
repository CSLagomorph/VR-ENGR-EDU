using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used for mapping VreeInput sources to PC inputs. This is used
/// to set an action to VR and Keyboard/Mouse within a single call. Often times
/// that is not possible, so use the KeyCode variant of the VreeInput.GetButton.
/// Unless you are modifying the input mappings, there is no need to change
/// anything here.
/// </summary>
public static class VreeInputPCBridge
{
    private const string PLAYER_OBJECT_NAME = "player";
    private const string HMD_OBJECT_NAME = "player";

    private static GameObject playerObject;
    private static GameObject hmdObject;
    
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
    /// The Dictionary holding the bindings for the different VreeInput.Buttons
    /// to specific keys on a keyboard.
    /// </summary>
    /// <typeparam name="VreeInput.Button">
    /// The VreeInput button that will be mapped to the keycode.
    /// </typeparam>
    /// <typeparam name="KeyCode">
    /// The KeyCode that will be mapped to the VreeInput button.
    /// </typeparam>
    /// <returns> The KeyCode mapped to the given VreeInput button.</returns>
    public static Dictionary<VreeInput.Button, KeyCode> DefaultKeyBoardButtonBindings = new Dictionary<VreeInput.Button, KeyCode>()
    {
        {VreeInput.Button.Button1, KeyCode.Q},
        {VreeInput.Button.Button2, KeyCode.E},
        {VreeInput.Button.GripTrigger, KeyCode.F},
        {VreeInput.Button.IndexTrigger, KeyCode.Space},
        {VreeInput.Button.Up, KeyCode.W},
        {VreeInput.Button.Down, KeyCode.S},
        {VreeInput.Button.Left, KeyCode.A},
        {VreeInput.Button.Right, KeyCode.D},
        {VreeInput.Button.ThumbStick, KeyCode.Z},
        {VreeInput.Button.Menu, KeyCode.Escape}
    };

    /// <summary>
    /// This dictionary is used for storing bindings between VreeInput.Axis1Ds
    /// that return a float between 0.0 and 1.0, and a KeyCode. These will be
    /// treated essentially like buttons, except instead of true/false, they
    /// will return 0.0 and 0.1, respectively.
    /// </summary>
    /// <typeparam name="VreeInput.Axis1D">The VreeInput axis that will be
    /// mapped to the keycode.</typeparam>
    /// <typeparam name="KeyCode">The KeyCode that will be mapped to the 
    /// VreeInput axis.</typeparam>
    /// <returns>The Keycode mapped to the given VreeInput axis.</returns>
    public static Dictionary<VreeInput.Axis1D, KeyCode> DefaultKeyBoardAxis1DBindings = new Dictionary<VreeInput.Axis1D, KeyCode>()
    {
        {VreeInput.Axis1D.GripTrigger, KeyCode.F},
        {VreeInput.Axis1D.IndexTrigger, KeyCode.Space}
    };

    /// <summary>
    /// This dictionary is used for storing bindings between VreeInput.Axis1Ds
    /// that return a float between -1.0 and 1.0, and an array of 2 KeyCodes. 
    /// These will be used to simulate an axis on a joystick, with the first 
    /// keycode in the array being the -1 key, and the second keycode being the
    /// 1 key.
    /// </summary>
    /// <typeparam name="VreeInput.Axis1D">
    /// The VreeInput axis mapped to the KeyCodes.
    /// </typeparam>
    /// <typeparam name="KeyCode[]">
    /// The KeyCode array that is mapped to the VreeInput axis.
    /// </typeparam>
    /// <returns>
    /// An array of 2 KeyCodes mapped to the given VreeInput axis. The first 
    /// KeyCode will be the -1 direction, and the second KeyCode will be the +1 
    /// direction.
    /// </returns>
    public static Dictionary<VreeInput.Axis1D, KeyCode[]> DefaultKeyBoardThumbstickAxis1DBindings = new Dictionary<VreeInput.Axis1D, KeyCode[]>()
    {
        {VreeInput.Axis1D.ThumbstickX, new KeyCode[2] {KeyCode.A, KeyCode.D}},
        {VreeInput.Axis1D.ThumbstickY, new KeyCode[2] {KeyCode.W, KeyCode.S}}
    };

    /// <summary>
    /// This dictionary is used for storing bindings between VreeInput.Buttons
    /// and a mouse button.
    /// </summary>
    /// <typeparam name="VreeInput.Button">
    /// The VreeInput button mapped to the mouse button.
    /// </typeparam>
    /// <typeparam name="int">
    /// The mouse button ID mapped to the VreeInput button.
    /// </typeparam>
    /// <returns>
    /// The mouse button ID mapped to the given VreeInput button. This ID is 
    /// used by Unity's Input system.
    /// </returns>
    public static Dictionary<VreeInput.Button, int> DefaultMouseButtonBindings = new Dictionary<VreeInput.Button, int>()
    {
        {VreeInput.Button.Button1, 0},
        {VreeInput.Button.Button2, 1}
    };

    /// <summary>
    /// This dictionary is used for storing bindings between VreeInput.Axis2Ds
    /// that return a Vector2, with x and y values between <c>-1.0</c>, and 
    /// <c>1.0</c>. The Array of KeyCodes should only ever be 4 elements long, 
    /// and follow this order: <c>{+Y, -Y, +X, -X}</c>
    /// </summary>
    /// <typeparam name="VreeInput.Axis2D">
    /// The VreeInput axis mapped to the KeyCode array.
    /// </typeparam>
    /// <typeparam name="KeyCode[]">
    /// The KeyCode Array mapped to the VreeInput axis. This should only ever be
    /// 4 elements long, since it represents a 2D axis with 4 directions.
    /// </typeparam>
    /// <returns>
    /// An array of 4 KeyCodes mapped to the 4 directions of a 2D axis, with the
    /// first 2 being the +1/-1 y direction, and the last 2 being the +1/-1 x
    /// direction.
    /// </returns>
    public static Dictionary<VreeInput.Axis2D, KeyCode[]> DefaultKeyBoardAxis2DBindings = new Dictionary<VreeInput.Axis2D, KeyCode[]>()
    {
        {VreeInput.Axis2D.Thumbstick, new KeyCode[4] {KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D}}
    };

    /// <summary>
    /// Checks and returns the state of the Key or mouse button mapped to the 
    /// given button on the given input device.
    /// </summary>
    /// <param name="device">
    /// The device to check the state of the button on.
    /// </param>
    /// <param name="button">The button to check the state of.</param>
    /// <returns>
    /// The state of the given button on the given device. Returns true if the 
    /// button is down, false otherwise.
    /// </returns>
    public static bool GetButton(VreeInput.InputDevice device, VreeInput.Button button)
    {

        if(device == VreeInput.InputDevice.None)
        {
            return false;
        }

        bool result = false;
        bool buttonFound = false;
        switch(device)
        {
            case VreeInput.InputDevice.Keyboard:
                KeyCode key;
                
                buttonFound = DefaultKeyBoardButtonBindings.TryGetValue(button, out key);
                if(!buttonFound)
                {
                    result = false;
                }
                else
                {
                    result = Input.GetKey(key);
                }
                break;
            case VreeInput.InputDevice.Mouse:
                int mouseButton = 0;
                buttonFound = DefaultMouseButtonBindings.TryGetValue(button, out mouseButton);
                if(!buttonFound)
                {
                    result = false;
                }
                else
                {
                    result = Input.GetMouseButton(mouseButton);
                }
                break;
            default:
                result = false;
                break;
        }

        return result;
    }

    /// <summary>
    /// Returns true during the frame the user starts pressing down the given
    /// button on the given device.
    /// </summary>
    /// <param name="device">
    /// The device to check the state of the button on.
    /// </param>
    /// <param name="button">The button to check the state of.</param>
    /// <returns>
    /// Returns true during the frame the user starts pressing down the given
    /// button on the given device, false otherwise.
    /// </returns>
    public static bool GetButtonDown(VreeInput.InputDevice device, VreeInput.Button button)
    {

        if(device == VreeInput.InputDevice.None)
        {
            return false;
        }

        bool result = false;
        bool buttonFound = false;
        switch(device)
        {
            case VreeInput.InputDevice.Keyboard:
                KeyCode key;
                
                buttonFound = DefaultKeyBoardButtonBindings.TryGetValue(button, out key);
                if(!buttonFound)
                {
                    result = false;
                }
                else
                {
                    result = Input.GetKeyDown(key);
                }
                break;
            case VreeInput.InputDevice.Mouse:
                int mouseButton = 0;
                buttonFound = DefaultMouseButtonBindings.TryGetValue(button, out mouseButton);
                if(!buttonFound)
                {
                    result = false;
                }
                else
                {
                    result = Input.GetMouseButtonDown(mouseButton);
                }
                break;
            default:
                result = false;
                break;
        }

        return result;
    }

    /// <summary>
    /// Returns true during the frame the user releases the given button on the
    /// given controller.
    /// </summary>
    /// <param name="device">
    /// The device to check the state of the button on.
    /// </param>
    /// <param name="button">The button to check the state of.</param>
    /// <returns>
    /// Returns true during the frame the user releases the given button on the
    /// given controller, false otherwise.
    /// </returns>
    public static bool GetButtonUp(VreeInput.InputDevice device, VreeInput.Button button)
    {
        if(device == VreeInput.InputDevice.None)
        {
            return false;
        }

        bool result = false;
        bool buttonFound = false;
        switch(device)
        {
            case VreeInput.InputDevice.Keyboard:
                KeyCode key;
                
                buttonFound = DefaultKeyBoardButtonBindings.TryGetValue(button, out key);
                if(!buttonFound)
                {
                    result = false;
                }
                else
                {
                    result = Input.GetKeyUp(key);
                }
                break;
            case VreeInput.InputDevice.Mouse:
                int mouseButton = 0;
                buttonFound = DefaultMouseButtonBindings.TryGetValue(button, out mouseButton);
                if(!buttonFound)
                {
                    result = false;
                }
                else
                {
                    result = Input.GetMouseButtonUp(mouseButton);
                }
                break;
            default:
                result = false;
                break;
        }

        return result;
    }

    /// <summary>
    /// Returns the state of the given axis on the given device. This will be
    /// between 0.0 and 1.0, or -1.0 and 1.0 depending on the axis.
    /// </summary>
    /// <param name="device">The device to check the axis on.</param>
    /// <param name="axis">The axis to check the state on.</param>
    /// <returns>
    /// Returns a float between 0.0 and 1.0 or -1.0 and 1.0 that represents the
    /// state of the given axis on the given device. If the axis does not exist
    /// on the device, this will always return 0.0.
    /// </returns>
    public static float GetAxis1D(VreeInput.InputDevice device, VreeInput.Axis1D axis)
    {
        if(device == VreeInput.InputDevice.Mouse || device == VreeInput.InputDevice.None)
        {
            return 0.0F;
        }
        float result = 0.0F;
        bool axisFound = false;
        if(!axisFound)
        {
            result = 0.0F;
        }
        else
        {
            switch(axis)
            {
                case VreeInput.Axis1D.GripTrigger:
                case VreeInput.Axis1D.IndexTrigger:
                    KeyCode key;
                    axisFound = DefaultKeyBoardAxis1DBindings.TryGetValue(axis, out key);
                    if(!axisFound)
                    {
                        result = 0.0F;
                    }
                    else
                    {
                        result = Input.GetKey(key) ? 1.0F : 0.0F;
                    }
                    break;
                case VreeInput.Axis1D.ThumbstickX:
                case VreeInput.Axis1D.ThumbstickY:
                    KeyCode[] keys;
                    axisFound = DefaultKeyBoardThumbstickAxis1DBindings.TryGetValue(axis, out keys);
                    if(!axisFound)
                    {
                        result = 0.0F;
                    }
                    else
                    {
                        result = Input.GetKey(keys[0]) ? -1.0F : 0.0F;
                        result += Input.GetKey(keys[1]) ? 1.0F : 0.0F;
                    }
                break;
                case VreeInput.Axis1D.None:
                default:
                    result = 0.0F;
                    break;
            }
            
        }
        return result;
    }

    /// <summary>
    /// Returns the state of the given axis on the given device. This will be
    /// a Vector2, with x and y values between -1.0 and 1.0.
    /// </summary>
    /// <param name="device">The device to check the axis on.</param>
    /// <param name="axis">The axis to check the state on.</param>
    /// <returns>
    /// A Vector2 with x and y values between -1.0 and 1.0 that represents the
    /// state of the given axis on the given device.
    /// </returns>
    public static Vector2 GetAxis2D(VreeInput.InputDevice device, VreeInput.Axis2D axis)
    {
        if(device == VreeInput.InputDevice.Mouse || device == VreeInput.InputDevice.None)
        {
            return Vector2.zero;
        }
        Vector2 result = Vector2.zero;
        KeyCode[] keys;
        bool axisFound = false;
        axisFound = DefaultKeyBoardAxis2DBindings.TryGetValue(axis, out keys);

        if(!axisFound)
        {
            result = Vector2.zero;
        }
        else
        {
            float x = 0.0F;
            float y = 0.0F;
            y = Input.GetKey(keys[0]) ? 1.0F : 0.0F;
            y += Input.GetKey(keys[1]) ? -1.0F : 0.0F;

            x = Input.GetKey(keys[2]) ? -1.0F : 0.0F;
            x += Input.GetKey(keys[3]) ? 1.0F : 0.0F;
            result = new Vector2(x, y);
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
            case VreeInput.InputDevice.RightController:
            default:
                result = null;
                break;
        }
        return result;
    }

    public static Vector3 GetPosition(VreeInput.InputDevice device)
    {
        Vector3 result = Vector3.zero;
        switch(device)
        {
            case VreeInput.InputDevice.HMD:
                result = HMDObject.transform.position;
                break;
            case VreeInput.InputDevice.Mouse:
                result = Input.mousePosition;
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
        switch(device)
        {
            case VreeInput.InputDevice.HMD:
                result = HMDObject.transform.rotation;
                break;
            default:
                result = Quaternion.identity;
                break;
        }
        return result;
    }

    public static Transform GetPlayer()
    {
        return PlayerObject.transform;
    }

    
}
