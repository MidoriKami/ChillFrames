using ChillFrames.Classes;
using ChillFrames.Utilities;
using Dalamud.Plugin.Services;

namespace ChillFrames.LimiterOptions;

public class BoundByDuty : IFrameLimiterOption {
	public string Label => "Duties - Any";

	public bool Active => ICondition.Get().IsBoundByDuty;

	public ref LimiterStateTarget Target => ref System.Config.General.DutyTarget;
}