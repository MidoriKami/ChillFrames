using System.Linq;
using ChillFrames.Config;
using KamiLib.GameState;

namespace ChillFrames.System;

internal static class FrameLimiterCondition
{
    private static GeneralSettings Settings => ChillFramesSystem.Config.General;
    private static BlacklistSettings Blacklist => ChillFramesSystem.Config.Blacklist;

    public static bool DisableFramerateLimit()
    {
        if (Blacklist is { Enabled: true } && InFilteredZone()) return true;
        
        if (Condition.IsDutyRecorderPlayback() && Settings.DisableDuringDutyRecorderPlaybackSetting) return true;
        if (Condition.IsBoundByDuty() && Settings.DisableDuringDutySetting) return true;
        if (Condition.IsInCombat() && Settings.DisableDuringCombatSetting) return true;
        if (Condition.IsInCutscene() && Settings.DisableDuringCutsceneSetting) return true;
        if (Condition.IsInQuestEvent() && Settings.DisableDuringQuestEventSetting) return true;
        if (Condition.IsCrafting() && Settings.DisableDuringCraftingSetting) return true;
        if (Condition.IsInIslandSanctuary() && Settings.DisableIslandSanctuarySetting) return true;
        if (Condition.IsInBardPerformance() && Settings.DisableDuringBardPerformance) return true;
        if (Condition.IsBetweenAreas()) return true;

        return false;
    }

    private static bool InFilteredZone() => Blacklist.BlacklistedZones.Any(territory => territory == Service.ClientState.TerritoryType);
}