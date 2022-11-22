using System;
using System.Windows;
using System.Windows.Controls;
using BoilerSim.Models;

namespace BoilerSim;

public partial class MixingValveControl : UserControl, IUpdateable
{
    public MixingValve MixingValve { get; set; }

    public MixingValveControl(MixingValve mixingValve)
    {
        MixingValve = mixingValve;
        InitializeComponent();
    }


    public void Update()
    {
        TempSlider.Minimum = Convert.ToDouble(MixingValve.MinTemp);
        TempSlider.Maximum = Convert.ToDouble(MixingValve.MaxTemp);

        TextMinTemp.Text = TempSlider.Minimum.ToString("F0");
        TextMaxTemp.Text = TempSlider.Maximum.ToString("F0");

        MixingValve.TargetTemp = Convert.ToDecimal(TempSlider.Value);
        MixingValve.Volume = Convert.ToDecimal(VolumeSlider.Value);


        TextTargetTemp.Text = MixingValve.TargetTemp.ToString("F2");
    }
}