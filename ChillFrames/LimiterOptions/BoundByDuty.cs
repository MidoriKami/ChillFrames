using ChillFrames.Classes;
using ChillFrames.Utilities;

namespace ChillFrames.LimiterOptions;

public class BoundByDuty : IFrameLimiterOption {
    public string Label => "Duties";
    
    public bool Active => Services.Condition.IsBoundByDuty;
    
    public ref bool Enabled => ref System.Config.General.DisableDuringDutySetting;
}