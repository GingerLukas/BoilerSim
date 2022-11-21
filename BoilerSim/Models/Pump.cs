using System.Collections.Generic;

namespace BoilerSim.Models;

public class Pump : WaterSimNode
{
    public override decimal CurrentVolume
    {
        get { return 1000000; }
        set { }
    }

    public override decimal CurrentTemp
    {
        get { return 10; }
        set { }
    }

    public override void DedicateStep()
    {
        base.DedicateStep();
    }
}