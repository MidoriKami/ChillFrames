using ChillFrames.Classes;
using ChillFrames.Utilities;
using Dalamud.Plugin.Services;

namespace ChillFrames.LimiterOptions;

public class BardPerformance : IFrameLimiterOption {
	public string Label => "Bard Performance";

	public bool Active => ICondition.Get().IsInBardPerformance;

	public ref LimiterStateTarget Target => ref System.Config.General.BardPerformanceTarget;
}