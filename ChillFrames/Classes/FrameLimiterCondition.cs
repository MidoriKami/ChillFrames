using System.Linq;
using ChillFrames.Utilities;

namespace ChillFrames.Classes;

internal static class FrameLimiterCondition {
    public static bool DisableFramerateLimit() {
        if (System.LimiterOptions.Any(option => option is { Enabled: true, Active: true })) return true;
        if (Services.Condition.IsBetweenAreas) return true;

        return false;
    }
}