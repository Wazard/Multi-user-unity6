using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
/// <summary>
/// These are a few different ways to detect the current VR HMD and controller type.
/// - Unity XR api
/// - Unity Input api
/// - SteamVR plugin
/// </summary>
public static class DetectVR
{
    /// <summary>
    /// Possible HMD types
    /// </summary>
    public enum VRHMD
    {
        vive,
        vive_pro,
        vive_cosmos,
        rift,
        indexhmd,
        holographic_hmd,
        none
    }


    /// <summary>
    /// Possible controller types
    /// </summary>
    public enum VRController
    {
        vive_controller,
        vive_cosmos_controller,
        oculus_touch,
        knuckles,
        holographic_controller,
        none
    }

    

    

    /// <summary>
    /// Get the VR controller type using Unity XR/VR
    /// </summary>
    /// <returns>Controller type as enum</returns>
    public static VRController GetControllerTypeToEnum()
    {
        // check XR nodes
        List<InputDevice> inputDeviceList = new List<InputDevice>();
        InputDevices.GetDevices(inputDeviceList);
        foreach (InputDevice currInputDevice in inputDeviceList)
        {
            if (currInputDevice.name.Contains("vive_controller"))
                return VRController.vive_controller;
            if (currInputDevice.name.Contains("vive_cosmos_controller"))
                return VRController.vive_cosmos_controller;
            if (currInputDevice.name.Contains("oculus_touch"))
                return VRController.oculus_touch;
            if (currInputDevice.name.Contains("knuckles"))
                return VRController.knuckles;
            if (currInputDevice.name.Contains("holographic_controller"))
                return VRController.holographic_controller;
        }
        return VRController.none;
    }

    /// <summary>
    /// Get the VR controller type using Unity XR/VR
    /// </summary>
    /// <returns>"OpenVR Controller(vive_cosmos_controller) - Left", "OpenVR Controller(Knuckles Left) - Left"</returns>
    public static string GetControllerTypeToString()
    {
        // check XR input devices
        List<InputDevice> inputDeviceList = new List<InputDevice>();
        InputDevices.GetDevices(inputDeviceList);

        foreach (InputDevice currInputDevice in inputDeviceList)
        {
            if (currInputDevice.characteristics.HasFlag(InputDeviceCharacteristics.HeldInHand) &&
                (currInputDevice.characteristics.HasFlag(InputDeviceCharacteristics.Left) ||
                currInputDevice.characteristics.HasFlag(InputDeviceCharacteristics.Right)))
            {
                return currInputDevice.name;
            }
        }

        return null;
    }


/// <summary>
/// Get the VR controller type using Unity input joystick
/// </summary>
/// <returns>"OpenVR Controller(vive_cosmos_controller) - Left", "OpenVR Controller(Knuckles Left) - Left"</returns>
public static string GetControllerType2ToString()
    {
        if (Input.GetJoystickNames().Length == 0)
            return null;

        return Input.GetJoystickNames()[0];
    }


    
}