using ChillFrames.Controllers;
using ChillFrames.Interfaces;
using KamiLib.Game;

namespace ChillFrames.Models.LimiterOptions;

public class IslandSanctuary : IFrameLimiterOption {
    public string Label => "Island Sanctuary";
    public bool GetActive() => Condition.IsInIslandSanctuary();
    public ref bool GetSetting() => ref ChillFramesSystem.Config.General.DisableIslandSanctuarySetting;
}