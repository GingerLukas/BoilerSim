using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
        private Simulator _simulator;
        private PumpControl _pumpControl;
        private BoilerControl _boilerControl;
        private MixingValveControl _valveControlShower;
        private MixingValveControl _valveControlSink;
        public Timer UpdateTimer { get; set; }

        private bool _simEnabled = false;
        private int _stepsPerUpdate = 0;

        public MainWindow()
        {
            UpdateTimer = new Timer(TimeSpan.FromMilliseconds(100));

            UpdateTimer.Elapsed += UpdateTimerOnElapsed;

            InitializeComponent();

            Pump pump = new Pump();

            Boiler boiler = new Boiler() { HeatingMode = EHeatingMode.Automatic };
            boiler.Diameter = 5;
            boiler.Height = 20;
            boiler.HeatingPower = 4000;
            boiler.AddProvider(pump);

            MixingValve shower = new MixingValve();
            shower.AddProvider(boiler);
            shower.AddProvider(pump);

            MixingValve sink = new MixingValve();
            sink.AddProvider(boiler);
            sink.AddProvider(pump);


            _simulator = new Simulator();
            _simulator.AddNode(pump);
            _simulator.AddNode(boiler);
            _simulator.AddNode(shower);
            _simulator.AddNode(sink);

            _pumpControl = new PumpControl(pump);
            PumpsContainer.Children.Add(_pumpControl);
            _boilerControl = new BoilerControl(boiler);
            BoilerContainer.Children.Add(_boilerControl);
            _valveControlShower = new MixingValveControl(shower);
            ValvesContainer.Children.Add(_valveControlShower);
            _valveControlSink = new MixingValveControl(sink);
            ValvesContainer.Children.Add(_valveControlSink);


            UpdateTimer.Start();
        }


        private void UpdateTimerOnElapsed(object? sender, ElapsedEventArgs e)
        {
            if (_simEnabled)
            {
                lock (_simulator)
                {
                    for (int i = 0; i < _stepsPerUpdate; i++)
                    {
                        _simulator.Step();
                    }
                }

                Dispatcher.Invoke(() =>
                {
                    _pumpControl.Update();
                    _boilerControl.Update();
                    _valveControlShower.Update();
                    _valveControlSink.Update();

                    SimElapsed.Text = _simulator.GetTime().ToString();
                    TextSimSpeed.Text = $"{_stepsPerUpdate}x";
                });
            }
        }

        private void SliderSimSpeed_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _stepsPerUpdate = Convert.ToInt32(SliderSimSpeed.Value);
        }

        private void ButtonSimPlay_OnClick(object sender, RoutedEventArgs e)
        {
            _simEnabled = true;
            ButtonSimPlay.IsEnabled = false;
            ButtonSimStop.IsEnabled = true;
        }

        private void ButtonSimStop_OnClick(object sender, RoutedEventArgs e)
        {
            _simEnabled = false;
            ButtonSimStop.IsEnabled = false;
            ButtonSimPlay.IsEnabled = true;
        }
    }
}