using System;
using System.Collections.Generic;
using System.Linq;
using Accessibility;

namespace BoilerSim.Models;

public abstract class WaterSimNode
{
    protected static decimal TimeStep => Convert.ToDecimal(0.1d);

    protected readonly List<WaterSimNode> Providers = new List<WaterSimNode>();
    protected readonly List<WaterSimNode> Consumers = new List<WaterSimNode>();

    protected readonly Dictionary<WaterSimNode, decimal> VolumeRequests = new Dictionary<WaterSimNode, decimal>();
    protected readonly List<(decimal Temp, decimal Volume)> VolumeSupplies = new List<(decimal Temp, decimal Volume)>();
    protected decimal VolumeRequestTotal = 0;
    protected decimal DedicatedVolume = 0;
    protected decimal LeftOverVolume = 0;

    protected bool RequestDone;
    protected bool DedicateDone;
    protected bool FinalDone;


    public abstract decimal CurrentVolume { get; set; }

    public abstract decimal CurrentTemp { get; set; }


    public decimal Volume { get; set; }

    public void AddProvider(WaterSimNode provider)
    {
        Providers.Add(provider);
        provider.Consumers.Add(this);
    }

    public void AddConsumer(WaterSimNode consumer)
    {
        Consumers.Add(consumer);
        consumer.Providers.Add(this);
    }

    //reset all counters
    public virtual void InitSimStep()
    {
        //reset request
        VolumeRequestTotal = 0;
        VolumeRequests.Clear();
        VolumeSupplies.Clear();
        //reset leftover
        LeftOverVolume = 0;
        //set available volume to current colume
        DedicatedVolume = CurrentVolume;

        RequestDone = false;
        DedicateDone = false;
        FinalDone = false;
    }

//request volume for this step
    public virtual void RequestStep()
    {
        if (RequestDone)
        {
            return;
        }

        RequestDone = true;

        foreach (WaterSimNode consumer in Consumers)
        {
            consumer.RequestStep();
        }
        

        if (Providers.Count != 0)
        {
            decimal volumeQuantum = (VolumeRequestTotal + (Volume - CurrentVolume)) / Providers.Count;
            foreach (WaterSimNode waterProvider in Providers)
            {
                waterProvider.RequestVolume(this, volumeQuantum);
            }
        }
    }

    //dedicate volume to all consumers
    public virtual void DedicateStep()
    {
        if (DedicateDone)
        {
            return;
        }

        DedicateDone = true;

        foreach (WaterSimNode provider in Providers)
        {
            provider.DedicateStep();
        }

        //check for left over
        if (VolumeRequestTotal < DedicatedVolume)
        {
            LeftOverVolume = DedicatedVolume - VolumeRequestTotal;
        }

        if (Consumers.Count != 0)
        {
            decimal multiplier = VolumeRequestTotal == 0 ? 0 : DedicatedVolume / VolumeRequestTotal;
            multiplier = Math.Min(multiplier, 1);

            //dedicate to all consumers
            foreach (KeyValuePair<WaterSimNode,decimal> request in VolumeRequests)
            {
                request.Key.DedicateVolume(multiplier * request.Value, CurrentTemp);
            }
        }
    }

    //calculate final state
    public virtual void CalcFinalStep()
    {
        if (FinalDone)
        {
            return;
        }

        FinalDone = true;

        if (VolumeSupplies.Count != 0)
        {
            decimal volume = CurrentVolume;
            decimal sum = CurrentTemp * CurrentVolume;
            foreach ((decimal Temp, decimal Volume) in VolumeSupplies)
            {
                volume += Volume;
                sum += (Temp * Volume);
            }

            CurrentTemp = volume == 0 ? 0 : sum / volume;
        }

        CurrentVolume = decimal.Min(Volume, LeftOverVolume);
    }

    public virtual void DedicateVolume(decimal volume, decimal temp)
    {
        if (volume == 0)
        {
            return;
        }
        VolumeSupplies.Add((temp, decimal.Min(volume, Volume)));
        DedicatedVolume += volume;
    }

    public virtual void RequestVolume(WaterSimNode consumer, decimal volume)
    {
        if (volume == 0)
        {
            return;
        }
        VolumeRequests.Add(consumer, volume);
        VolumeRequestTotal += volume;
    }
}