using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChillFrames.Data.Enums;
using ChillFrames.Data.SettingsObjects;
using ChillFrames.Data.SettingsObjects.Components;
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

            return !boundByDuty && !inCombat && !inCutscene && !inBlacklistedArea;
        }

        private static bool InCutscene()
        {
            return Service.Condition[ConditionFlag.OccupiedInCutSceneEvent];
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
    }
}
