using ChillFrames.Classes;
using KamiLib.Extensions;

namespace ChillFrames.LimiterOptions;

public class Combat : IFrameLimiterOption {
    public string Label => "Combat";
    
    public bool Active => Service.Condition.IsInCombat();
    
    public ref bool Enabled => ref System.Config.General.DisableDuringCombatSetting;
}