using ChillFrames.Controllers;
using ChillFrames.Interfaces;
using KamiLib.Game;

namespace ChillFrames.Models.LimiterOptions;

public class IslandSanctuary : IFrameLimiterOption {
    public string Label => "Island Sanctuary";
    public bool Active => Condition.IsInIslandSanctuary();
    public ref bool Enabled => ref ChillFramesSystem.Config.General.DisableIslandSanctuarySetting;
}