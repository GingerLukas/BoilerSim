namespace BoilerSim.Models;

public abstract class WaterSimNode : IWaterConsumer, IWaterProvider
{
    //reset all counters
    public abstract void InitSimStep();
    //request volume for this step
    public abstract void RequestStep();
    
    //dedicate volume to all consumers
    public abstract void DedicateStep();
    
    //calculate final state
    public abstract void CalcFinalStep();
    public abstract void DedicateVolume(decimal volume, decimal temp);

    public abstract void RequestVolume(IWaterConsumer consumer, decimal volume);
}