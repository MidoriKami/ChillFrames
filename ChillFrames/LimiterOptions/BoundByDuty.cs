using ChillFrames.Controllers;
using ChillFrames.Interfaces;
using KamiLib.Extensions;

namespace ChillFrames.Models.LimiterOptions;

public class BoundByDuty : IFrameLimiterOption {
    public string Label => "Duties";
    public bool Active => Service.Condition.IsBoundByDuty();
    public ref bool Enabled => ref ChillFramesSystem.Config.General.DisableDuringDutySetting;
}