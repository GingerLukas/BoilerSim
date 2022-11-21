using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BoilerSim.Models;

namespace BoilerSim
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Pump pump = new Pump();

            Boiler boiler = new Boiler(){HeatingMode = EHeatingMode.Automatic};
            boiler.Diameter = 5;
            boiler.Height = 20;
            boiler.HeatingPower = 4000;
            boiler.AddProvider(pump);

            MixingValve valve = new MixingValve() { Volume = 0, TargetTemp = 15 };
            valve.AddProvider(boiler);
            valve.AddProvider(pump);

            Simulator simulator = new Simulator();
            simulator.AddNode(pump);
            simulator.AddNode(boiler);
            simulator.AddNode(valve);

        }
    }
}