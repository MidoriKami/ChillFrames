using ChillFrames.Classes;
using ChillFrames.Utilities;

namespace ChillFrames.LimiterOptions;

public class DutyRecorderPlayback : IFrameLimiterOption {
    public string Label => "Duty Recorder Playback";
    
    public bool Active => Services.Condition.IsDutyRecorderPlayback;
    
    public ref bool Enabled => ref System.Config.General.DisableDuringDutyRecorderPlaybackSetting;
}