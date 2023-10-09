using ChillFrames.Controllers;
using ChillFrames.Interfaces;
using KamiLib.Game;

namespace ChillFrames.Models.LimiterOptions;

public class Crafting : IFrameLimiterOption
{
    public string Label => "Crafting";
    public bool IsActive() => Condition.IsCrafting();
    public ref bool GetSetting() => ref ChillFramesSystem.Config.General.DisableDuringCraftingSetting;
}