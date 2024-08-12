using ChillFrames.Classes;
using KamiLib.Extensions;

namespace ChillFrames.LimiterOptions;

public class Crafting : IFrameLimiterOption {
    public string Label => "Crafting";
    
    public bool Active => Service.Condition.IsCrafting();
    
    public ref bool Enabled => ref System.Config.General.DisableDuringCraftingSetting;
}