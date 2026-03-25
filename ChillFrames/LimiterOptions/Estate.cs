using ChillFrames.Classes;
using FFXIVClientStructs.FFXIV.Client.Game;

namespace ChillFrames.LimiterOptions;

public unsafe class Estate : IFrameLimiterOption {
    public string Label => "Inside Estate";

    public bool Active => HousingManager.Instance()->IsInside();

    public ref LimiterStateTarget Target => ref System.Config.General.EstateTarget;
}
