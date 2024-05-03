using ChillFrames.Controllers;
using ChillFrames.Interfaces;
using KamiLib.Classes;

namespace ChillFrames.Models.LimiterOptions;

public class Crafting : IFrameLimiterOption {
    public string Label => "Crafting";
    public bool Active => Service.Condition.IsCrafting();
    public ref bool Enabled => ref ChillFramesSystem.Config.General.DisableDuringCraftingSetting;
}