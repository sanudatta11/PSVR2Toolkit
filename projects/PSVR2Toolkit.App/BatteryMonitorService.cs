using System;
using Valve.VR;

namespace PSVR2Toolkit.App;

/// <summary>
/// Battery monitoring service for PSVR2 controllers using native OpenVR API.
/// Queries battery percentage and charging status via OpenVR properties.
/// </summary>
public class BatteryMonitorService
{
    private CVRSystem? vrSystem;

    public class ControllerBatteryInfo
    {
        public int BatteryLevel { get; set; } = -1; // -1 = unavailable, 0-100 = percentage
        public bool IsCharging { get; set; } = false;
        public bool IsConnected { get; set; } = false;
    }

    public BatteryMonitorService()
    {
        TryInitializeOpenVR();
        Logger.Info("Battery monitor service created");
    }

    private void TryInitializeOpenVR()
    {
        try
        {
            var error = EVRInitError.None;
            vrSystem = OpenVR.Init(ref error, EVRApplicationType.VRApplication_Background);

            if (error != EVRInitError.None)
            {
                Logger.Warning($"OpenVR initialization failed: {OpenVR.GetStringForHmdError(error)}");
                vrSystem = null;
                return;
            }

            Logger.Info("OpenVR initialized successfully for battery monitoring");
        }
        catch (Exception ex)
        {
            Logger.Error($"Failed to initialize OpenVR: {ex.Message}", ex);
            vrSystem = null;
        }
    }

    public ControllerBatteryInfo GetLeftControllerBattery()
    {
        return GetControllerBattery(ETrackedControllerRole.LeftHand);
    }

    public ControllerBatteryInfo GetRightControllerBattery()
    {
        return GetControllerBattery(ETrackedControllerRole.RightHand);
    }

    private ControllerBatteryInfo GetControllerBattery(ETrackedControllerRole role)
    {
        var info = new ControllerBatteryInfo();

        if (vrSystem == null)
        {
            TryInitializeOpenVR();
            if (vrSystem == null)
                return info;
        }

        try
        {
            // Find the device index for the specified controller role
            uint deviceIndex = vrSystem.GetTrackedDeviceIndexForControllerRole(role);

            // Check if device index is valid
            if (deviceIndex == OpenVR.k_unTrackedDeviceIndexInvalid)
            {
                return info; // Controller not found
            }

            // Check if device is connected
            if (!vrSystem.IsTrackedDeviceConnected(deviceIndex))
            {
                return info;
            }

            info.IsConnected = true;

            // Query battery percentage
            ETrackedPropertyError error = ETrackedPropertyError.TrackedProp_Success;
            float batteryFloat = vrSystem.GetFloatTrackedDeviceProperty(
                deviceIndex,
                ETrackedDeviceProperty.Prop_DeviceBatteryPercentage_Float,
                ref error
            );

            if (error == ETrackedPropertyError.TrackedProp_Success)
            {
                info.BatteryLevel = (int)(batteryFloat * 100.0f);
            }

            // Query charging status
            bool isCharging = vrSystem.GetBoolTrackedDeviceProperty(
                deviceIndex,
                ETrackedDeviceProperty.Prop_DeviceIsCharging_Bool,
                ref error
            );

            if (error == ETrackedPropertyError.TrackedProp_Success)
            {
                info.IsCharging = isCharging;
            }
        }
        catch (Exception ex)
        {
            Logger.Error($"Failed to query controller battery: {ex.Message}", ex);
        }

        return info;
    }

    public bool IsOpenVRAvailable()
    {
        return vrSystem != null;
    }

    public void Shutdown()
    {
        if (vrSystem != null)
        {
            OpenVR.Shutdown();
            vrSystem = null;
            Logger.Info("OpenVR shut down");
        }
    }
}
