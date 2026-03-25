using ChillFrames.Classes;
using ChillFrames.Utilities;

namespace ChillFrames.LimiterOptions;

public class BoundByDuty : IFrameLimiterOption {
    public string Label => "Duties";

    public bool Active => Services.Condition.IsBoundByDuty;

    public ref LimiterStateTarget Target => ref System.Config.General.DutyTarget;
}
