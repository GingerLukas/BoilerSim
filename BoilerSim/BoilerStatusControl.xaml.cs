using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BoilerSim.Models;

namespace BoilerSim;

public partial class BoilerStatusControl : UserControl, IUpdateable
{
    private readonly Boiler _boiler;

    public BoilerStatusControl(Boiler boiler)
    {
        _boiler = boiler;
        InitializeComponent();

        ComboControlMode.ItemsSource = Enum.GetValues(typeof(EHeatingMode)).Cast<EHeatingMode>();
        ComboControlMode.SelectedItem = _boiler.HeatingMode;
    }


    public void Update()
    {
        if (ComboControlMode.SelectionBoxItem is EHeatingMode mode)
        {
            _boiler.HeatingMode = mode;
        }

        CheckHeatingEnabled.IsEnabled = _boiler.HeatingMode == EHeatingMode.Manual;
        CheckHeatingEnabled.IsChecked = _boiler.Enabled;
    }

    private void CheckHeatingEnabled_OnChecked(object sender, RoutedEventArgs e)
    {
        _boiler.Enabled = true;
    }

    private void CheckHeatingEnabled_OnUnchecked(object sender, RoutedEventArgs e)
    {
        _boiler.Enabled = false;
    }
}