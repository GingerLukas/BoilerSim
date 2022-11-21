using System;
using System.Linq;

namespace BoilerSim.Models;

public class MixingValve : WaterSimNode
{
    public override decimal CurrentVolume { get; set; }
    public override decimal CurrentTemp { get; set; }

    public decimal TargetTemp { get; set; }

    public decimal MinTemp => decimal.Min(Providers[0].CurrentTemp, Providers[1].CurrentTemp);
    public decimal MaxTemp => decimal.Max(Providers[0].CurrentTemp, Providers[1].CurrentTemp);


    public override void RequestStep()
    {
        if (RequestDone)
        {
            return;
        }

        RequestDone = true;

        if (Providers.Count != 2)
        {
            throw new Exception(
                $"{nameof(MixingValve)} needs exactly 2 Providers to work! (Providers.Count = {Providers.Count})");
        }

        if (Volume == 0 || Providers.Any(x=>x.CurrentTemp <= 0))
        {
            return;
        }


        decimal resultDelta = TargetTemp - MinTemp;
        decimal delta = decimal.Abs(Providers[0].CurrentTemp - Providers[1].CurrentTemp);
        decimal multiplier = resultDelta / delta;
        multiplier = decimal.Clamp(multiplier, 0, 1);
        Providers[0].RequestVolume(this, multiplier * Volume);
        Providers[1].RequestVolume(this, (1 - multiplier) * Volume);
    }
}