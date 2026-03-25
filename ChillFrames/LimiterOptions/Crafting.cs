using ChillFrames.Classes;
using ChillFrames.Utilities;

namespace ChillFrames.LimiterOptions;

public class Crafting : IFrameLimiterOption {
    public string Label => "Crafting";

    public bool Active => Services.Condition.IsCrafting;

    public ref LimiterStateTarget Target => ref System.Config.General.CraftingTarget;
}
