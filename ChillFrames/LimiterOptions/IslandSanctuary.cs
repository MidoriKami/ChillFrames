using ChillFrames.Controllers;
using FFXIVClientStructs.FFXIV.Client.Game.MJI;

namespace ChillFrames.LimiterOptions;

public unsafe class IslandSanctuary : IFrameLimiterOption {
    public string Label => "Island Sanctuary";
    public bool Active => MJIManager.Instance() is not null && MJIManager.Instance()->IsPlayerInSanctuary == 1;
    public ref bool Enabled => ref ChillFramesSystem.Config.General.DisableIslandSanctuarySetting;
}