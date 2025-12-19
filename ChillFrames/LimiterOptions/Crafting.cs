using ChillFrames.Classes;
using ChillFrames.Utilities;

namespace ChillFrames.LimiterOptions;

public class Crafting : IFrameLimiterOption {
    public string Label => "Crafting";
    
    public bool Active => Services.Condition.IsCrafting;
    
    public ref bool Enabled => ref System.Config.General.DisableDuringCraftingSetting;
}