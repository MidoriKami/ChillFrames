using System.Linq;
using KamiLib.Extensions;

namespace ChillFrames.Classes;

internal static class FrameLimiterCondition {
    public static bool IsBlacklisted 
        => System.Config.Blacklist.BlacklistedZones
            .Any(territory => territory == Service.ClientState.TerritoryType);

    public static bool DisableFramerateLimit() {
        if (System.LimiterOptions.Any(option => option is { Enabled: true, Active: true })) return true;
        if (Service.Condition.IsBetweenAreas()) return true;

        return false;
    }
}