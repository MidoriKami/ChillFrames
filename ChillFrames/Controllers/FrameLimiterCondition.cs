using System.Linq;
using ChillFrames.Models;
using KamiLib.Game;

namespace ChillFrames.Controllers;

internal static class FrameLimiterCondition
{
    private static BlacklistSettings Blacklist => ChillFramesSystem.Config.Blacklist;

    public static bool IsBlacklisted => Blacklist is { Enabled: true } && InFilteredZone();

    public static bool DisableFramerateLimit()
    {
        if (ChillFramesPlugin.System.LimiterOptions.Any(option => option.IsEnabled() && option.IsActive())) return true;
        if (Condition.IsBetweenAreas()) return true;

        return false;
    }

    private static bool InFilteredZone()
    {
        return Blacklist.BlacklistedZones.Any(territory => territory == Service.ClientState.TerritoryType);
    }
}