using ChillFrames.Utilities;

namespace ChillFrames.Classes;

internal static class FrameLimiterCondition {
    public static LimiterStateTarget GetTargetState() {
        if (Services.Condition.IsBetweenAreas) return LimiterStateTarget.BaseLimit;

        var result = LimiterStateTarget.LowerLimit;
        var anyActive = false;

        foreach (var option in System.LimiterOptions) {
            if (!option.Active) continue;
            anyActive = true;
            if (option.Target > result) result = option.Target;
        }

        return anyActive ? result : LimiterStateTarget.BaseLimit;
    }
}
