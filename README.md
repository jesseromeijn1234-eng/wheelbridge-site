# WheelBridge

WheelBridge is an early-development Windows app project.

## Windows installer

The repository includes a classic Windows Setup Wizard built with Inno Setup.

To create the installer:
1. Open the **Actions** tab.
2. Select **Build WheelBridge Windows installer**.
3. Click **Run workflow**.
4. When the workflow finishes, open the run and download the **WheelBridge-Windows-Setup** artifact.

The artifact contains the setup EXE. It is currently a development build, not a finished wheel-to-Roblox bridge.

## What the installer does
- Installs the app under Program Files.
- Adds a WheelBridge entry to the Windows Start menu.
- Offers an optional desktop shortcut.
- Adds WheelBridge to Windows Installed Apps so it can be uninstalled normally.
- Bundles the .NET runtime so users do not need to install the .NET SDK.

## Current limitations
The current app foundation provides a WPF interface, DirectInput device discovery, local profile settings and simulated input controls. Live wheel-axis reading, virtual controller output, Roblox integration and physical force feedback are not implemented yet.

Manufacturer software is not bundled. Use official vendor downloads only.
