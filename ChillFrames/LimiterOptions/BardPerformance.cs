using ChillFrames.Controllers;
using KamiLib.Extensions;

namespace ChillFrames.LimiterOptions;

public class BardPerformance : IFrameLimiterOption {
    public string Label => "Bard Performance";
    public bool Active => Service.Condition.IsInBardPerformance();
    public ref bool Enabled => ref ChillFramesSystem.Config.General.DisableDuringBardPerformance;
}