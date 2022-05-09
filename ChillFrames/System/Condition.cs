using System;
using System.Linq;
using ChillFrames.Data.Enums;
using ChillFrames.Data.SettingsObjects;
using Dalamud.Game.ClientState.Conditions;

namespace ChillFrames.System
{
    internal static class Condition
    {
        private static GeneralSettings Settings => Service.Configuration.General;
        private static BlacklistSettings Blacklist => Service.Configuration.Blacklist;

        public static bool EnableFramerateLimit()
        {
            var boundByDuty = BoundByDuty() && Settings.DisableDuringDuty;
            var inCombat = InCombat() && Settings.DisableDuringCombat;
            var inCutscene = InCutscene() && Settings.DisableDuringCutscene;
            var inBlacklistedArea = InBlacklistedZone() && Blacklist.Enabled;
            var inQuestEvent = InQuestEvent() && Settings.DisableDuringQuestEvent;
            var isBetweenAreas = IsBetweenAreas();

            return !boundByDuty && !inCombat && !inCutscene && !inBlacklistedArea && !inQuestEvent && !isBetweenAreas;
        }

        private static bool InCutscene()
        {
            return Service.Condition[ConditionFlag.OccupiedInCutSceneEvent] ||
                   Service.Condition[ConditionFlag.WatchingCutscene] ||
                   Service.Condition[ConditionFlag.WatchingCutscene78];
        }

        private static bool BoundByDuty()
        {
            return Service.Condition[ConditionFlag.BoundByDuty] ||
                   Service.Condition[ConditionFlag.BoundByDuty56] ||
                   Service.Condition[ConditionFlag.BoundByDuty95];
        }

        private static bool InCombat()
        {
            return Service.Condition[ConditionFlag.InCombat];
        }

        private static bool InBlacklistedZone()
        {
            var inTaggedZone = Blacklist.Territories.Any(territory => territory.TerritoryID == Service.ClientState.TerritoryType);

            return Blacklist.Mode switch
            {
                BlacklistMode.Exclusion => inTaggedZone,
                BlacklistMode.Inclusion => !inTaggedZone,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static bool InQuestEvent()
        {
            return Service.Condition[ConditionFlag.OccupiedInQuestEvent];
        }

        private static bool IsBetweenAreas()
        {
            return Service.Condition[ConditionFlag.BetweenAreas] ||
                   Service.Condition[ConditionFlag.BetweenAreas51];
        }
    }
}
