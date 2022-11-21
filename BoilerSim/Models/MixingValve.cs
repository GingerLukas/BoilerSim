namespace BoilerSim.Models;

public class Valve : WaterSimNode
{
    public override decimal CurrentVolume { get; set; }
    public override decimal CurrentTemp { get; set; }

    public decimal TargetVolume { get; set; }
    public decimal TargetTemp { get; set; }

    public override void RequestStep()
    {
        VolumeRequestTotal = TargetVolume;
        base.RequestStep();
    }
}