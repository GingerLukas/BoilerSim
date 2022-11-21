using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace BoilerSim.Models;

public enum EHeatingMode
{
    Automatic,
    Manual
}

public class Boiler : WaterSimNode
{
    private decimal _diameter;
    private decimal _height;

    public EHeatingMode HeatingMode { get; set; }
    public bool Enabled { get; set; }


    public decimal Diameter
    {
        get => _diameter;
        set
        {
            _diameter = value;
            RecalculateVolume();
        }
    }

    public decimal Height
    {
        get => _height;
        set
        {
            _height = value;
            RecalculateVolume();
        }
    }

    public decimal HeatingPower { get; set; }

    public override decimal CurrentVolume { get; set; }

    public override decimal CurrentTemp { get; set; }


    public override void CalcFinalStep()
    {
        base.CalcFinalStep();

        decimal heating = Enabled ? HeatingPower : 0;
        heating = CurrentTemp > 20 ? heating - 100 : heating;
        CurrentTemp += (TimeStep * heating) / (CurrentVolume * 4180);
        
        ControlStep();
    }

    private void ControlStep()
    {
        if (HeatingMode == EHeatingMode.Automatic)
        {
            if (CurrentTemp > 70)
            {
                Enabled = false;
            }
            else if (CurrentTemp < 50)
            {
                Enabled = true;
            }
        }
        else
        {
            if (CurrentTemp > 80)
            {
                Enabled = false;
            }
        }
    }


    private void RecalculateVolume()
    {
        Volume = 2 * (decimal)Math.PI * (_diameter / 2) * (_diameter / 2) * _height;
    }
}