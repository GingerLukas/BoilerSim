using System.Windows.Controls;
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
        TemperatureBLock.Text = Boiler.CurrentTemp.ToString();
        VolumeBlock.Text = Boiler.CurrentVolume.ToString();
    }
}