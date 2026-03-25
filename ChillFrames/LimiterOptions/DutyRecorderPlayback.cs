using ChillFrames.Classes;
using ChillFrames.Utilities;

namespace ChillFrames.LimiterOptions;

public class DutyRecorderPlayback : IFrameLimiterOption {
    public string Label => "Duty Recorder Playback";

    public bool Active => Services.Condition.IsDutyRecorderPlayback;

    public ref LimiterStateTarget Target => ref System.Config.General.DutyRecorderPlaybackTarget;
}
