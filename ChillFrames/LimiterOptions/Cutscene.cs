using ChillFrames.Classes;
using ChillFrames.Utilities;
using Dalamud.Plugin.Services;

namespace ChillFrames.LimiterOptions;

public class Cutscene : IFrameLimiterOption {
	public string Label => "Cutscenes";

	public bool Active => ICondition.Get().IsInCutscene;

	public ref LimiterStateTarget Target => ref System.Config.General.CutsceneTarget;
}