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

Version 0.2.0 adds:
- DirectInput discovery of connected game controllers.
- Live polling of raw joystick axes (X, Y, Z, Rotation X/Y/Z).
- Live display of additional slider values and the number of pressed buttons.
- A selectable controller list and a manual rescan button.

The input monitor shows raw DirectInput values. Axis names and ranges depend on the wheel and its driver, so this build does not automatically identify which axis is steering, throttle, brake or clutch yet.

It does not yet:
- Map wheel axes to steering, throttle, brake, clutch, paddles or an H-shifter.
- Create a virtual controller.
- Connect to Roblox.
- Send force feedback to a physical wheel.
- Save configurable control profiles.

These features must be implemented and tested before this can be considered a working wheel bridge.

## Manufacturer software

WheelBridge does not bundle Logitech G HUB, MOZA Pit House, Thrustmaster software, Fanatec software or other vendor drivers. Get those only from the relevant manufacturer's official website.
