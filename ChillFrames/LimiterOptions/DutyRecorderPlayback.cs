using ChillFrames.Classes;
using ChillFrames.Utilities;
using Dalamud.Plugin.Services;

namespace ChillFrames.LimiterOptions;

public class DutyRecorderPlayback : IFrameLimiterOption {
	public string Label => "Duty Recorder Playback";

	public bool Active => ICondition.Get().IsDutyRecorderPlayback;

	public ref LimiterStateTarget Target => ref System.Config.General.DutyRecorderPlaybackTarget;
}