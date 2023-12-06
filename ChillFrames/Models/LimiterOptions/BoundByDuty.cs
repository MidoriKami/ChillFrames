using ChillFrames.Controllers;
using ChillFrames.Interfaces;
using KamiLib.Game;

namespace ChillFrames.Models.LimiterOptions;

public class BoundByDuty : IFrameLimiterOption {
    public string Label => "Duties";
    public bool GetActive() => Condition.IsBoundByDuty();
    public ref bool GetSetting() => ref ChillFramesSystem.Config.General.DisableDuringDutySetting;
}