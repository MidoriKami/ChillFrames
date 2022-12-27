using System.Linq;
using ChillFrames.Config;
using Condition = KamiLib.Utilities.Condition;

namespace ChillFrames.System;

internal static class FrameLimiterCondition
{
    private static GeneralSettings Settings => Service.Configuration.General;
    private static BlacklistSettings Blacklist => Service.Configuration.Blacklist;

    public static bool DisableFramerateLimit()
    {
        if (Blacklist is {EnabledSetting.Value: true, ModeSetting.Value: BlacklistMode.Exclusion} && InFilteredZone()) return true;
        if (Blacklist is {EnabledSetting.Value: true, ModeSetting.Value: BlacklistMode.Inclusion} && InFilteredZone()) return false;
        if (Blacklist is {EnabledSetting.Value: true, ModeSetting.Value: BlacklistMode.Inclusion} && !InFilteredZone()) return true;

        if (Condition.IsDutyRecorderPlayback() && Settings.DisableDuringDutyRecorderPlaybackSetting.Value) return true;
        if (Condition.IsBoundByDuty() && Settings.DisableDuringDutySetting.Value) return true;
        if (Condition.IsInCombat() && Settings.DisableDuringCombatSetting.Value) return true;
        if (Condition.IsInCutscene() && Settings.DisableDuringCutsceneSetting.Value) return true;
        if (Condition.IsInQuestEvent() && Settings.DisableDuringQuestEventSetting.Value) return true;
        if (Condition.IsCrafting() && Settings.DisableDuringCraftingSetting.Value) return true;
        if (Condition.IsInIslandSanctuary() && Settings.DisableIslandSanctuarySetting.Value) return true;
        if (Condition.IsBetweenAreas()) return true;

        return false;
    }

    private static bool InFilteredZone()
    {
        return Blacklist.BlacklistedZones.Value.Any(territory => territory == Service.ClientState.TerritoryType);
    }
}