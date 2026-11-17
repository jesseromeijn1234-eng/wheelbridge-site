using System.Windows;
using System.Windows.Controls;

namespace WheelBridge.App;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        UpdateReadouts();
    }

    private void InputChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (!IsLoaded)
        {
            return;
        }

        UpdateReadouts();
    }

    private void UpdateReadouts()
    {
        SteeringValue.Text = $"{SteeringSlider.Value:0}%";
        ThrottleValue.Text = $"{ThrottleSlider.Value:0}%";
        BrakeValue.Text = $"{BrakeSlider.Value:0}%";
        ClutchValue.Text = $"{ClutchSlider.Value:0}%";
    }

    private void Reset_Click(object sender, RoutedEventArgs e)
    {
        SteeringSlider.Value = 0;
        ThrottleSlider.Value = 0;
        BrakeSlider.Value = 0;
        ClutchSlider.Value = 0;
    }
}
