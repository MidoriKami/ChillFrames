using System.Linq;
using KamiLib.Classes;

namespace ChillFrames.Controllers;

internal static class FrameLimiterCondition {
    public static bool IsBlacklisted 
        => ChillFramesSystem.Config.Blacklist.BlacklistedZones
            .Any(territory => territory == Service.ClientState.TerritoryType);

    public static bool DisableFramerateLimit() {
        if (ChillFramesPlugin.System.LimiterOptions.Any(option => option is { Enabled: true, Active: true })) return true;
        if (Service.Condition.IsBetweenAreas()) return true;

        return false;
    }
}