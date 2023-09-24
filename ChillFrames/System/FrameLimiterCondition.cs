using System.Linq;
using ChillFrames.Config;
using FFXIVClientStructs.FFXIV.Client.Game;
using KamiLib.GameState;

namespace ChillFrames.System;

internal static class FrameLimiterCondition
{
    private static GeneralSettings Settings => ChillFramesSystem.Config.General;
    private static BlacklistSettings Blacklist => ChillFramesSystem.Config.Blacklist;

    public static bool DisableFramerateLimit()
    {
        if (Condition.IsDutyRecorderPlayback() && Settings.DisableDuringDutyRecorderPlaybackSetting) return true;
        if (Condition.IsBoundByDuty() && Settings.DisableDuringDutySetting) return true;
        if (Condition.IsInCombat() && Settings.DisableDuringCombatSetting) return true;
        if (Condition.IsInCutscene() && Settings.DisableDuringCutsceneSetting) return true;
        if (Condition.IsInQuestEvent() && Settings.DisableDuringQuestEventSetting) return true;
        if (Condition.IsCrafting() && Settings.DisableDuringCraftingSetting) return true;
        if (Condition.IsInIslandSanctuary() && Settings.DisableIslandSanctuarySetting) return true;
        if (Condition.IsInBardPerformance() && Settings.DisableDuringBardPerformance) return true;
        if (GameMain.IsInGPose() && Settings.DisableDuringGpose) return true;
        if (Condition.IsBetweenAreas()) return true;

        return false;
    }

    public static bool IsBlacklisted => Blacklist is { Enabled: true } && InFilteredZone();

    private static bool InFilteredZone() => Blacklist.BlacklistedZones.Any(territory => territory == Service.ClientState.TerritoryType);
}