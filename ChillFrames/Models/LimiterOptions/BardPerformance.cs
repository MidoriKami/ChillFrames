using ChillFrames.Controllers;
using ChillFrames.Interfaces;
using KamiLib.Game;

namespace ChillFrames.Models.LimiterOptions;

public class BardPerformance : IFrameLimiterOption {
    public string Label => "Bard Performance";
    public bool Active => Condition.IsInBardPerformance();
    public ref bool Enabled => ref ChillFramesSystem.Config.General.DisableDuringBardPerformance;
}