using ChillFrames.Classes;
using ChillFrames.Utilities;

namespace ChillFrames.LimiterOptions;

public class BardPerformance : IFrameLimiterOption {
    public string Label => "Bard Performance";
    
    public bool Active => Services.Condition.IsInBardPerformance;
    
    public ref bool Enabled => ref System.Config.General.DisableDuringBardPerformance;
}