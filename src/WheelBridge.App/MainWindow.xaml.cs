using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;
using WheelBridge.App.Services;

namespace WheelBridge.App;

public partial class MainWindow : Window
{
    private readonly DispatcherTimer _inputTimer;
    private DirectInputWheelService? _wheelService;
    private IReadOnlyList<DetectedWheel> _detectedDevices = Array.Empty<DetectedWheel>();

    public MainWindow()
    {
        InitializeComponent();

        _inputTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(33)
        };
        _inputTimer.Tick += InputTimer_Tick;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        ScanDevices();
    }

    private void ScanDevices_Click(object sender, RoutedEventArgs e)
    {
        ScanDevices();
    }

    private void ScanDevices()
    {
        try
        {
            _wheelService ??= new DirectInputWheelService();

            IntPtr windowHandle = new WindowInteropHelper(this).Handle;
            _detectedDevices = _wheelService.RefreshDevices(windowHandle);

            DeviceSelector.ItemsSource = _detectedDevices;
            DeviceSelector.IsEnabled = _detectedDevices.Count > 0;

            if (_detectedDevices.Count > 0)
            {
                DeviceStatusText.Text = $"{_detectedDevices.Count} controller(s) detected.";
                DeviceSelector.SelectedIndex = 0;
            }
            else
            {
                DeviceSelector.SelectedItem = null;
                DeviceStatusText.Text = "No DirectInput controllers found. Connect and switch on your wheel, then scan again.";
                InputMonitorStatus.Text = "No controller selected.";
                ResetInputReadouts();
            }
        }
        catch (Exception)
        {
            DeviceStatusText.Text = "Device scanning could not start. Restart WheelBridge and try again.";
            InputMonitorStatus.Text = "DirectInput is unavailable.";
            ResetInputReadouts();
        }
    }

    private void DeviceSelector_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (DeviceSelector.SelectedItem is DetectedWheel selectedDevice)
        {
            InputMonitorStatus.Text = $"Reading live input from {selectedDevice.ProductName}.";
            _inputTimer.Start();
            ReadSelectedDevice();
        }
        else
        {
            _inputTimer.Stop();
            InputMonitorStatus.Text = "Select a controller to view live input.";
            ResetInputReadouts();
        }
    }

    private void InputTimer_Tick(object? sender, EventArgs e)
    {
        ReadSelectedDevice();
    }

    private void ReadSelectedDevice()
    {
        if (_wheelService is null || DeviceSelector.SelectedItem is not DetectedWheel selectedDevice)
        {
            return;
        }

        if (!_wheelService.TryRead(selectedDevice.InstanceGuid, out WheelInputSnapshot snapshot, out string error))
        {
            InputMonitorStatus.Text = error;
            return;
        }

        InputMonitorStatus.Text = $"Live input active • {selectedDevice.ProductName}";
        XAxisValue.Text = snapshot.X.ToString("N0");
        YAxisValue.Text = snapshot.Y.ToString("N0");
        ZAxisValue.Text = snapshot.Z.ToString("N0");
        RotationXValue.Text = snapshot.RotationX.ToString("N0");
        RotationYValue.Text = snapshot.RotationY.ToString("N0");
        RotationZValue.Text = snapshot.RotationZ.ToString("N0");
        SliderValues.Text = $"{snapshot.Slider0:N0}  /  {snapshot.Slider1:N0}";
        ButtonsValue.Text = snapshot.PressedButtons.ToString();
    }

    private void ResetInputReadouts()
    {
        XAxisValue.Text = "—";
        YAxisValue.Text = "—";
        ZAxisValue.Text = "—";
        RotationXValue.Text = "—";
        RotationYValue.Text = "—";
        RotationZValue.Text = "—";
        SliderValues.Text = "—  /  —";
        ButtonsValue.Text = "—";
    }

    private void Window_Closed(object? sender, EventArgs e)
    {
        _inputTimer.Stop();
        _wheelService?.Dispose();
        _wheelService = null;
    }
}
