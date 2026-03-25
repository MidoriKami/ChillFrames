using ChillFrames.Classes;
using ChillFrames.Utilities;

namespace ChillFrames.LimiterOptions;

public class Cutscene : IFrameLimiterOption {
    public string Label => "Cutscenes";

    public bool Active => Services.Condition.IsInCutscene;

    public ref LimiterStateTarget Target => ref System.Config.General.CutsceneTarget;
}
