using System;
using System.Collections.Generic;

namespace BoilerSim.Models;

public class Simulator
{
    private List<WaterSimNode> _nodes = new List<WaterSimNode>();
    private ulong _ticks = 0;

    public WaterSimNode AddNode(WaterSimNode node)
    {
        _nodes.Add(node);
        return node;
    }

    public TimeSpan GetTime()
    {
        return TimeSpan.FromMilliseconds(_ticks * 100);
    }
    
    public void Step()
    {
        _ticks++;
        foreach (WaterSimNode node in _nodes)
        {
            node.InitSimStep();
        }
        foreach (WaterSimNode node in _nodes)
        {
            node.RequestStep();
        }
        foreach (WaterSimNode node in _nodes)
        {
            node.DedicateStep();
        }
        foreach (WaterSimNode node in _nodes)
        {
            node.CalcFinalStep();
        }
    }
}