using System.Windows.Controls;
using System.Windows.Media;
using BoilerSim.Models;

namespace BoilerSim;

public partial class BoilerControl : UserControl, IUpdateable
{
    public Boiler Boiler { get; set; }

    public BoilerControl(Boiler boiler)
    {
        Boiler = boiler;
        InitializeComponent();
    }

    public void Update()
    {
        TemperatureBlock.Text = $"{Boiler.CurrentTemp:F4}℃";
        VolumeBlock.Text = $"{Boiler.CurrentVolume:F}L";
        decimal value = (Boiler.CurrentTemp) / 100;
        WaterLevel.Fill =
            new SolidColorBrush(Color.FromRgb((byte)(value * byte.MaxValue), 0, (byte)((1 - value) * byte.MaxValue)));
    }
}