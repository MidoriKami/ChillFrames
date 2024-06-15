using ChillFrames.Controllers;
using KamiLib.Extensions;

namespace ChillFrames.LimiterOptions;

public class DutyRecorderPlayback : IFrameLimiterOption {
    public string Label => "Duty Recorder Playback";
    public bool Active => Service.Condition.IsDutyRecorderPlayback();
    public ref bool Enabled => ref ChillFramesSystem.Config.General.DisableDuringDutyRecorderPlaybackSetting;
}