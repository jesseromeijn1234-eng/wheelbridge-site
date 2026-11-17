# WheelBridge

WheelBridge is an early-development Windows app for a future racing-wheel-to-Roblox bridge.

## Build the Windows installer

1. Open the **Actions** tab in this repository.
2. Select **Build WheelBridge Windows installer**.
3. Select **Run workflow** and confirm.
4. Open the completed run and download the **WheelBridge-Windows-Setup** artifact.

The artifact contains a Windows setup EXE. It installs WheelBridge under Program Files, adds a Start menu shortcut, offers an optional desktop shortcut and registers an uninstaller in Windows Installed Apps.

The installer is self-contained: the .NET runtime is included, so the end user does not need to install the .NET SDK.

## Current app status

This build is an interface preview only. The sliders in the Input Lab are simulated. It does not yet:
- Detect or read a physical racing wheel.
- Read steering, throttle, brake, clutch, paddles, buttons or an H-shifter.
- Create a virtual controller.
- Connect to Roblox.
- Send force feedback to a physical wheel.
- Save configurable control profiles.

These features must be implemented and tested before this can be considered a working wheel bridge.

## Manufacturer software

WheelBridge does not bundle Logitech G HUB, MOZA Pit House, Thrustmaster software, Fanatec software or other vendor drivers. Get those only from the relevant manufacturer's official website.
