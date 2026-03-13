# PSVR2Toolkit Feature Implementation Summary

## Overview
This document summarizes the new features implemented in this session, including technical details, usage instructions, and notes for future development.

---

## ✅ Feature 1: Eye Tracking → OS Cursor Control

### Description
Allows users to control the Windows mouse cursor using their eye gaze. Useful for desktop VR applications and accessibility.

### Implementation Details

**C++ Driver (`ipc_server.cpp`, `hmd_device_hooks.cpp`):**
- Added `UpdateGazeCursor()` method with smoothing, deadzone, and sensitivity controls
- Integrated into gaze update pipeline to move cursor in real-time
- Uses Windows `SetCursorPos()` API for cursor movement
- Exponential moving average smoothing to reduce jitter
- Deadzone filtering (0.05 default) to ignore micro-movements

**IPC Protocol (`ipc_protocol.h`):**
- Command 16: `Command_ClientEnableGazeCursor` with `CommandDataClientGazeCursorControl_t`
- Command 17: `Command_ClientDisableGazeCursor`
- Parameters: sensitivity (0.1-5.0), smoothing (0.0-1.0), deadzone (0.0-0.5)

**C# Client (`IpcClient.cs`):**
- `EnableGazeCursor(float sensitivity, float smoothing, float deadzone)` method
- `DisableGazeCursor()` method

**WPF UI (`MainWindow.xaml`):**
- Toggle checkbox in Eye Calibration tab
- Sensitivity slider (0.1 - 5.0, default 1.0)
- Smoothing slider (0.0 - 1.0, default 0.3)
- Real-time settings updates

### Usage
1. Open PSVR2Toolkit companion app
2. Navigate to "Eye Calibration" tab
3. Check "Enable Gaze Cursor"
4. Adjust sensitivity/smoothing as needed
5. Your gaze now controls the mouse cursor

### Build Status
✅ Compiles successfully with 0 errors

---

## ✅ Feature 2: Auto-Recalibration Prompt with Session Ignore

### Description
Automatically detects eye tracking calibration drift and prompts the user to recalibrate, with an option to suppress notifications for the current session.

### Implementation Details

**C++ Driver (`ipc_server.cpp`, `hmd_device_hooks.cpp`):**
- `CheckGazeDrift()` method monitors gaze variance every 5 seconds
- Drift threshold: variance > 0.15 triggers notification
- 5-minute cooldown between notifications to prevent spam
- Sends `Command_ServerRecalibrationNeeded` IPC notification to client

**Drift Detection Algorithm:**
```cpp
float variance = gazeX * gazeX + gazeY * gazeY;
avgVariance = accumulator / sampleCount;
if (avgVariance > 0.15 && timeSinceLastNotification >= 300s) {
    SendNotification();
}
```

**IPC Protocol:**
- Command 18: `Command_ServerRecalibrationNeeded` (server → client notification)

**C# Client (`IpcClient.cs`):**
- `RecalibrationNeeded` event fires when drift detected
- Event handler in `MainWindow.xaml.cs` shows custom dialog

**WPF UI (`MainWindow.xaml.cs`):**
- Custom `RecalibrationDialog` class with 3 options:
  - **"Calibrate Now"** - Launches calibration tool immediately
  - **"Remind Me Later"** - Dismisses dialog, will notify again later
  - **"Ignore This Session"** - Suppresses all notifications until app restart
- Session flag `ignoreRecalibrationThisSession` prevents repeated prompts

### Usage
1. Drift detection runs automatically in background
2. When drift exceeds threshold, dialog appears
3. Choose action based on preference
4. "Ignore This Session" prevents notification spam

### Build Status
✅ Compiles successfully with 0 errors

---

## ⚠️ Feature 3: Controller Battery Monitor (Experimental)

### Description
Queries controller battery levels via IPC. **Note: Function offset is estimated and requires hardware testing to verify.**

### Implementation Details

**C++ Driver (`trigger_effect_manager.cpp`):**
- Added `GetControllerBatteryLevel()` method
- Reverse-engineered `ScePadControllerInformation` structure:
  ```cpp
  struct ScePadControllerInformation {
      uint8_t connected;
      uint8_t batteryLevel;  // 0-100 percentage
      uint8_t padding[30];
  };
  ```
- Function pointer: `scePadGetControllerInformation` at offset `0x1BF200`
  - **⚠️ WARNING:** This offset is estimated based on libpad API patterns
  - Actual offset may differ - needs hardware testing to verify
  - Common offsets are +0x100 to +0x500 from `scePadSetTriggerEffect` (0x1BF060)

**IPC Protocol:**
- Command 19: `Command_ClientRequestBatteryLevel` with controller type
- Command 20: `Command_ServerBatteryLevelResult` with battery level (0-100 or -1)

**C# Client (`IpcClient.cs`):**
- `RequestBatteryLevel(EVRControllerType)` method
- Returns -1 for unavailable/async operation

### Hardware Testing Required
To verify/adjust the battery function offset:

1. **Enable driver logging** to see if function calls succeed
2. **Test with hardware** - check if battery levels are returned correctly
3. **If function fails**, try nearby offsets:
   - Try: `0x1BF100`, `0x1BF150`, `0x1BF200`, `0x1BF250`, `0x1BF300`
   - Look for patterns in Sony's libpad exports
4. **Verify structure layout** - battery level field position may vary

### Potential Issues
- Function offset may be incorrect → returns garbage data or crashes
- Structure layout may differ → battery level in wrong field
- Function may not exist in this driver version → always returns -1

### Next Steps
1. Test on actual hardware
2. Adjust offset if needed in `trigger_effect_manager.cpp:61`
3. Add UI display in Health Check or separate battery tab
4. Consider using reverse engineering tools (IDA Pro, Ghidra) to find exact offset

### Build Status
✅ Compiles successfully with 0 errors (runtime behavior untested)

---

## 🔴 Feature 4: Headset Haptics (Not Implemented - Backlog)

### Status
**Deferred** - Requires extensive reverse engineering

### Why Not Implemented
- Headset vibration motor uses different interface than controller haptics
- No documented API for headset haptics in Sony's driver
- Would require:
  1. USB packet sniffing to find haptic commands
  2. Reverse engineering driver binary for function offsets
  3. Hardware testing to verify commands
  4. Risk of incorrect commands causing hardware issues

### Recommendation
Implement after battery monitor is verified working, or when Sony provides documentation.

---

## Build Summary

**Final Build Status:** ✅ **SUCCESS**
- 0 Errors
- 4 Warnings (nullable reference type annotations - non-critical)
- All projects compile successfully

**Modified Files:**
- `projects/shared/ipc_protocol.h` - Added IPC commands and structures
- `projects/psvr2_openvr_driver_ex/ipc_server.h/cpp` - Cursor control, drift detection, battery query
- `projects/psvr2_openvr_driver_ex/hmd_device_hooks.cpp` - Integrated cursor and drift checks
- `projects/psvr2_openvr_driver_ex/trigger_effect_manager.h/cpp` - Battery query implementation
- `projects/PSVR2Toolkit.IPC/IpcClient.cs` - Client methods for new features
- `projects/PSVR2Toolkit.IPC/IpcProtocol.cs` - C# protocol definitions
- `projects/PSVR2Toolkit.App/MainWindow.xaml` - UI for cursor control
- `projects/PSVR2Toolkit.App/MainWindow.xaml.cs` - Event handlers and recalibration dialog

---

## Testing Checklist

### Eye Tracking Cursor Control
- [ ] Enable cursor control in UI
- [ ] Verify cursor moves with gaze
- [ ] Test sensitivity adjustment (should affect speed)
- [ ] Test smoothing adjustment (should affect jitter)
- [ ] Disable cursor control - cursor should stop following gaze

### Auto-Recalibration Prompt
- [ ] Wait for drift to accumulate (may take time)
- [ ] Verify dialog appears when drift detected
- [ ] Test "Calibrate Now" - should launch calibration tool
- [ ] Test "Remind Me Later" - dialog should close
- [ ] Test "Ignore This Session" - no more prompts until restart
- [ ] Verify 5-minute cooldown between notifications

### Battery Monitor (Experimental)
- [ ] Call `RequestBatteryLevel()` for left controller
- [ ] Call `RequestBatteryLevel()` for right controller
- [ ] Check driver logs for battery query results
- [ ] If returns -1, adjust function offset and retry
- [ ] Verify battery percentage is reasonable (0-100)

---

## Known Issues & Limitations

1. **Battery Monitor Offset Unverified**
   - Function offset `0x1BF200` is estimated
   - May need adjustment based on hardware testing
   - Structure layout assumptions may be incorrect

2. **Cursor Control Coordinate Mapping**
   - Current implementation uses simple delta scaling
   - May need calibration for different screen sizes/resolutions
   - Multi-monitor setups not tested

3. **Drift Detection Sensitivity**
   - Threshold of 0.15 may be too sensitive or not sensitive enough
   - May need user-configurable threshold in future

4. **No Headset Haptics**
   - Deferred due to reverse engineering complexity
   - Would be valuable feature for immersion

---

## Future Enhancements

1. **Battery Monitor UI**
   - Add battery icons to Health Check tab
   - Show charging status if available
   - Low battery warnings

2. **Cursor Control Improvements**
   - Multi-monitor support
   - Per-monitor sensitivity scaling
   - Configurable deadzone in UI

3. **Drift Detection Tuning**
   - User-configurable drift threshold
   - Drift visualization/graph
   - Manual drift check trigger

4. **Headset Haptics**
   - Reverse engineer headset haptic interface
   - Add IPC commands for headset vibration
   - UI controls for testing

---

## Developer Notes

### IPC Protocol Version
Current version: **4**
- Version bumped from 3 to 4 for calibration commands
- All new commands added to version 4
- Maintain backward compatibility where possible

### Code Style
- C++: 2-space indentation, K&R braces
- C#: 4-space indentation, K&R braces
- Follow existing patterns in codebase

### Debugging Tips
- Enable driver logging: Check `Util::DriverLog()` calls
- IPC traffic: Monitor console output from IpcClient
- Battery queries: Check driver logs for function call results
- Drift detection: Logs show variance calculations every 5 seconds

---

## Contact & Support

For issues or questions about these features:
- GitHub: @sanudatta11
- Repository: https://github.com/sanudatta11/PSVR2Toolkit
- Discord: https://discord.gg/dPsfJhsGwb

---

**Implementation Date:** March 13, 2026  
**Build Status:** ✅ All features compile successfully  
**Hardware Testing Status:** ⚠️ Battery monitor requires verification
