using ChillFrames.Controllers;
using ChillFrames.Interfaces;
using KamiLib.Extensions;

namespace ChillFrames.Models.LimiterOptions;

public class Combat : IFrameLimiterOption {
    public string Label => "Combat";
    public bool Active => Service.Condition.IsInCombat();
    public ref bool Enabled => ref ChillFramesSystem.Config.General.DisableDuringCombatSetting;
}