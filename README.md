> [!WARNING]
> If you have paid for PlayStation VR2 Toolkit, you have been scammed and you should immediately request a refund. PlayStation VR2 Toolkit is entirely free and **is intended for NON-COMMERCIAL use only**, we do not attempt to profit off of it. Additionally, eye tracking data from PlayStation VR2 Toolkit **must not** be used in commercial environments and/or for commercial purposes. \
> \
> PlayStation VR2 Toolkit is provided "as is", without warranty of any kind, express or implied, including but not limited to the warranties of merchantability, fitness for a particular purpose and noninfringement. In no event shall the authors or copyright holders be liable for any claim, damages or other liability, whether in an action of contract, tort or otherwise, arising from, out of or in connection with the software or the use or other dealings in the software.

> [!NOTE]
> Bnuuy Solutions and PlayStation VR2 Toolkit is not affiliated with Sony Interactive Entertainment, our project is not official or endorsed by Sony Interactive Entertainment in any way. \
> \
> Sony, Sony Interactive Entertainment and PS VR2/PlayStation VR2 are trademarks or registered trademarks of Sony Interactive Entertainment LLC in the United States of America and elsewhere.

<p align="center"><img src="https://github.com/BnuuySolutions/PSVR2Toolkit/blob/main/assets/Icon.png?raw=true" width="128" height="128"></p>
<h1 align="center">PlayStation VR2 Toolkit</h1>

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

Enhance your PlayStation VR2 experience on PC with better eye tracking, improved controllers, and more features.

## What You Get
- **Eye Tracking** - Works in VR games with calibration to improve accuracy
- **Better Controllers** - Smoother tracking and realistic trigger feedback
- **Haptic Effects** - Feel different sensations through your controller triggers
- **Easy Setup** - Automatic installation with one command

## Quick Start Guide

### Step 1: Download
Download the latest release from the [Releases page](https://github.com/BnuuySolutions/PSVR2Toolkit/releases).

### Step 2: Install (Choose One Method)

#### Easy Installation (Recommended)
1. Extract the downloaded files to a folder
2. Right-click on `install.bat` and select "Run as Administrator"
3. Wait for the installation to complete
4. Restart SteamVR

That's it! You're done.

#### Advanced Installation (PowerShell)
If you prefer using PowerShell commands:

**Full Install (includes in-VR settings menu):**
```powershell
.\scripts\driver-install-full.ps1 -SourceDll .\x64\Release\driver_playstation_vr2.dll
```

**Basic Install (driver only):**
```powershell
.\scripts\driver-install.ps1 -SourceDll .\x64\Release\driver_playstation_vr2.dll
```

**Uninstall (restore original driver):**
```powershell
.\scripts\driver-restore.ps1
```

### Step 3: Use the Companion App

After installation, run `PSVR2Toolkit.App.exe` to access:

**Health Check Tab**
- Checks if everything is installed correctly
- Shows connection status
- Helps diagnose problems

**Trigger Profiles Tab**
- Choose different trigger feelings (Soft, Arcade, Realistic, Rumble)
- Test effects before using them in games
- Turn triggers off if you prefer

**Gaze Debug Tab**
- See what your eyes are looking at in real-time
- Useful for checking if eye tracking is working

**Eye Calibration Tab** (New!)
- Improve eye tracking accuracy
- Takes 1-2 minutes
- See instructions below

## Calibrating Eye Tracking

Eye tracking works out of the box, but calibration makes it more accurate.

### When to Calibrate
- First time using eye tracking
- After adjusting how the headset sits on your head
- If eye tracking feels off in games

### How to Calibrate (Simple Steps)

1. **Open the App**
   - Run `PSVR2Toolkit.App.exe`
   - Click the "Eye Calibration" tab

2. **Start Calibration**
   - Click "Launch Calibration Tool"
   - Put on your headset

3. **Follow the Dots**
   - Red dots will appear in VR
   - Look directly at each dot
   - Pull the right trigger when you're looking at it
   - Repeat for all dots (about 1-2 minutes)

4. **Done!**
   - Calibration saves automatically
   - Eye tracking is now more accurate
   - No need to restart anything

### Calibration Tips
- Make sure your headset fits comfortably before calibrating
- Look at the center of each dot
- Take your time - accuracy is more important than speed
- You can recalibrate anytime if it doesn't feel right

## In-VR Settings Menu

If you used the full installation, you can change settings without taking off your headset:

1. Open SteamVR settings (in VR or on desktop)
2. Look for "PlayStation VR2" section
3. Toggle features on/off:
   - Eye tracking
   - Controller tracking
   - Pop-up messages
   - And more

## Manual Installation (If Automatic Fails)

Only follow these steps if the automatic installation doesn't work:

1. Open Steam and find the PS VR2 app
2. Right-click → Manage → Browse local files
3. Navigate to: `SteamVR_Plug-In\bin\win64`
4. Rename `driver_playstation_vr2.dll` to `driver_playstation_vr2_orig.dll`
   - **Important:** The name must be exactly `driver_playstation_vr2_orig.dll`
5. Copy the new `driver_playstation_vr2.dll` from the release into this folder
6. You should now have both files:
   - `driver_playstation_vr2.dll` (new)
   - `driver_playstation_vr2_orig.dll` (backup)
7. Restart SteamVR

## Common Problems & Solutions

**Installation says "backup already exists"**
- The toolkit is already installed
- To reinstall, run the uninstall script first, then install again

**SteamVR settings menu doesn't show PlayStation VR2 section**
- Close SteamVR completely (right-click system tray icon → Exit)
- Start SteamVR again
- If still not showing, you may have used the basic install instead of full install

**Companion app says "Connection Failed"**
- Make sure SteamVR is running
- Make sure your PS VR2 headset is connected
- Try restarting the companion app
- Check Windows Firewall isn't blocking the app

**Eye tracking not working in games**
- Open SteamVR settings → PlayStation VR2
- Make sure "Disable Eye Tracking" is turned OFF
- Try calibrating eye tracking (see above)
- Adjust your headset for a better fit

**Triggers feel weird or don't work**
- Open the companion app
- Go to "Trigger Profiles" tab
- Try different profiles or turn them off

**Need more help?**
Join our [Discord community](https://discord.gg/dPsfJhsGwb) - we're happy to help!

# Contact
Have any legal complaints or questions?
- Email: `wdh at bnuuy.solutions`
- Discord: `notahopper`
