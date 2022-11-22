using System.Windows.Controls;
using BoilerSim.Models;

namespace BoilerSim;

public partial class PumpControl : UserControl, IUpdateable
{
    public Pump Pump { get; set; }
    public PumpControl(Pump pump)
    {
        Pump = pump;
        InitializeComponent();
    }

    public void Update()
    {
    }
}