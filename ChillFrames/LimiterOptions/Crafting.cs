using ChillFrames.Classes;
using ChillFrames.Utilities;
using Dalamud.Plugin.Services;

namespace ChillFrames.LimiterOptions;

public class Crafting : IFrameLimiterOption {
	public string Label => "Crafting";

	public bool Active => ICondition.Get().IsCrafting;

	public ref LimiterStateTarget Target => ref System.Config.General.CraftingTarget;
}