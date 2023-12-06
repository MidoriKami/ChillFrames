using ChillFrames.Controllers;
using ChillFrames.Interfaces;
using KamiLib.Game;

namespace ChillFrames.Models.LimiterOptions;

public class Crafting : IFrameLimiterOption {
    public string Label => "Crafting";
    public bool Active => Condition.IsCrafting();
    public ref bool Enabled => ref ChillFramesSystem.Config.General.DisableDuringCraftingSetting;
}