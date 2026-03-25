using ChillFrames.Classes;
using ChillFrames.Utilities;

namespace ChillFrames.LimiterOptions;

public class Combat : IFrameLimiterOption {
    public string Label => "Combat";

    public bool Active => Services.Condition.IsInCombat;

    public ref LimiterStateTarget Target => ref System.Config.General.CombatTarget;
}
