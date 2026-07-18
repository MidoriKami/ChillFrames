using ChillFrames.Classes;
using ChillFrames.Utilities;
using Dalamud.Plugin.Services;

namespace ChillFrames.LimiterOptions;

public class Combat : IFrameLimiterOption {
	public string Label => "Combat";

	public bool Active => ICondition.Get().IsInCombat;

	public ref LimiterStateTarget Target => ref System.Config.General.CombatTarget;
}