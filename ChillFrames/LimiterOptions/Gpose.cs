using ChillFrames.Classes;
using FFXIVClientStructs.FFXIV.Client.Game;

namespace ChillFrames.LimiterOptions;

public class Gpose : IFrameLimiterOption {
    public string Label => "GPose";

    public bool Active => GameMain.IsInGPose();

    public ref LimiterStateTarget Target => ref System.Config.General.GposeTarget;
}
