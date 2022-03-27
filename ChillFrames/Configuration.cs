using System;
using System.Collections.Generic;
using Dalamud.Configuration;
using Dalamud.Plugin;

namespace ChillFrames
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 1;
        public List<uint> TerritoryBlacklist = new();

        public bool DisableInBlacklistedTerritories = false;
        public bool DisableDuringCutscene = true;
        public bool DisableDuringCombat = true;
        public bool DisableDuringDuty = true;
        public bool EnableLimiter = true;
        public int FrameRateLimit = 60;

        [NonSerialized]
        private DalamudPluginInterface? pluginInterface;
        public void Initialize(DalamudPluginInterface inputPluginInterface) => pluginInterface = inputPluginInterface;
        public void Save() => pluginInterface!.SavePluginConfig(this);
    }
}