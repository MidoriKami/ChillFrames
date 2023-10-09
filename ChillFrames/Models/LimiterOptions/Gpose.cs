using ChillFrames.Controllers;
using ChillFrames.Interfaces;
using FFXIVClientStructs.FFXIV.Client.Game;

namespace ChillFrames.Models.LimiterOptions;

public class Gpose : IFrameLimiterOption
{
    public string Label => "GPose";
    public bool IsActive() => GameMain.IsInGPose();
    public ref bool GetSetting() => ref ChillFramesSystem.Config.General.DisableDuringGpose;
}