using Vortice.DirectInput;

namespace WheelBridge.App.Services;

public sealed record DetectedWheel(Guid InstanceGuid, string ProductName);

public readonly record struct WheelInputSnapshot(
    int X,
    int Y,
    int Z,
    int RotationX,
    int RotationY,
    int RotationZ,
    int Slider0,
    int Slider1,
    int PressedButtons);

public sealed class DirectInputWheelService : IDisposable
{
    private readonly IDirectInput8 _directInput;
    private readonly Dictionary<Guid, IDirectInputDevice8> _devices = new();
    private bool _disposed;

    public DirectInputWheelService()
    {
        _directInput = DInput.DirectInput8Create();
    }

    public IReadOnlyList<DetectedWheel> RefreshDevices(IntPtr windowHandle)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ReleaseDevices();

        var detected = new List<DetectedWheel>();
        IList<DeviceInstance> instances = _directInput.GetDevices(
            DeviceClass.GameControl,
            DeviceEnumerationFlags.AttachedOnly);

        foreach (DeviceInstance instance in instances)
        {
            IDirectInputDevice8? device = null;

            try
            {
                device = _directInput.CreateDevice(instance.InstanceGuid);

                // Background + non-exclusive lets WheelBridge read the wheel
                // while another app (such as a Roblox game) is in the foreground.
                device.SetCooperativeLevel(
                    windowHandle,
                    CooperativeLevel.NonExclusive | CooperativeLevel.Background);

                var formatResult = device.SetDataFormat<RawJoystickState>();
                if (formatResult.Failure)
                {
                    device.Dispose();
                    continue;
                }

                // Acquisition may need to be retried when the device first reports input.
                _ = device.Acquire();

                _devices[instance.InstanceGuid] = device;
                detected.Add(new DetectedWheel(
                    instance.InstanceGuid,
                    string.IsNullOrWhiteSpace(instance.ProductName)
                        ? "Unknown controller"
                        : instance.ProductName));

                device = null;
            }
            catch
            {
                // One unavailable or incompatible controller must not block other devices.
                device?.Dispose();
            }
        }

        return detected
            .OrderBy(item => item.ProductName, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    public bool TryRead(
        Guid instanceGuid,
        out WheelInputSnapshot snapshot,
        out string error)
    {
        snapshot = default;
        error = string.Empty;

        if (_disposed || !_devices.TryGetValue(instanceGuid, out IDirectInputDevice8? device))
        {
            error = "Select a detected controller first.";
            return false;
        }

        try
        {
            var pollResult = device.Poll();
            if (pollResult.Failure)
            {
                var acquireResult = device.Acquire();
                if (acquireResult.Failure)
                {
                    error = "Windows could not acquire this controller. Try scanning again.";
                    return false;
                }

                pollResult = device.Poll();
                if (pollResult.Failure)
                {
                    error = "The controller is not providing input. Check its connection.";
                    return false;
                }
            }

            JoystickState state = device.GetCurrentJoystickState();
            snapshot = new WheelInputSnapshot(
                state.X,
                state.Y,
                state.Z,
                state.RotationX,
                state.RotationY,
                state.RotationZ,
                state.Sliders.Length > 0 ? state.Sliders[0] : 0,
                state.Sliders.Length > 1 ? state.Sliders[1] : 0,
                state.Buttons.Count(pressed => pressed));

            return true;
        }
        catch
        {
            error = "The controller stopped responding. Scan again to reconnect it.";
            return false;
        }
    }

    private void ReleaseDevices()
    {
        foreach (IDirectInputDevice8 device in _devices.Values)
        {
            try
            {
                _ = device.Unacquire();
            }
            catch
            {
                // Continue disposing the remaining devices.
            }

            try
            {
                device.Dispose();
            }
            catch
            {
                // Continue disposing the remaining devices.
            }
        }

        _devices.Clear();
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        ReleaseDevices();
        _directInput.Dispose();
        _disposed = true;
    }
}
