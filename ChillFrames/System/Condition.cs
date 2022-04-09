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
            var inGpose = InGpose() && Settings.DisableDuringGpose;
            var inQuestEvent = InQuestEvent() && Settings.DisableDuringQuestEvent;

            return !boundByDuty && !inCombat && !inCutscene && !inBlacklistedArea && !inGpose && !inQuestEvent;
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

        private static bool InGpose()
        {
            var localPlayer = Service.ClientState.LocalPlayer;

            return localPlayer != null && localPlayer.OnlineStatus.Id == 18;
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
    }
}
