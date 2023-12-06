using ChillFrames.Controllers;
using ChillFrames.Interfaces;
using KamiLib.Game;

namespace ChillFrames.Models.LimiterOptions;

public class DutyRecorderPlayback : IFrameLimiterOption {
    public string Label => "Duty Recorder Playback";
    public bool GetActive() => Condition.IsDutyRecorderPlayback();
    public ref bool GetSetting() => ref ChillFramesSystem.Config.General.DisableDuringDutyRecorderPlaybackSetting;
}