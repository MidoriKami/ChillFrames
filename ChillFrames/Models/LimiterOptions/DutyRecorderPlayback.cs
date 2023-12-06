using ChillFrames.Controllers;
using ChillFrames.Interfaces;
using KamiLib.Game;

namespace ChillFrames.Models.LimiterOptions;

public class DutyRecorderPlayback : IFrameLimiterOption {
    public string Label => "Duty Recorder Playback";
    public bool Active => Condition.IsDutyRecorderPlayback();
    public ref bool Enabled => ref ChillFramesSystem.Config.General.DisableDuringDutyRecorderPlaybackSetting;
}