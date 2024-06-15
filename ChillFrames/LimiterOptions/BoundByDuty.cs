using ChillFrames.Controllers;
using KamiLib.Extensions;

namespace ChillFrames.LimiterOptions;

public class BoundByDuty : IFrameLimiterOption {
    public string Label => "Duties";
    public bool Active => Service.Condition.IsBoundByDuty();
    public ref bool Enabled => ref ChillFramesSystem.Config.General.DisableDuringDutySetting;
}