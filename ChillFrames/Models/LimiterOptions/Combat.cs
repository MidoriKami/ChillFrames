using ChillFrames.Controllers;
using ChillFrames.Interfaces;
using KamiLib.Game;

namespace ChillFrames.Models.LimiterOptions;

public class Combat : IFrameLimiterOption
{
    public string Label => "Combat";
    public bool IsActive() => Condition.IsInCombat();
    public ref bool GetSetting() => ref ChillFramesSystem.Config.General.DisableDuringCombatSetting;
}