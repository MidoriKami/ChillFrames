using System;
using System.Collections.Generic;
using ChillFrames.Data.SettingsObjects;
using Dalamud.Configuration;
using Dalamud.Plugin;

namespace ChillFrames.Data
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 1;

        public bool devMode = false;

        public GeneralSettings General = new();
        public BlacklistSettings Blacklist = new();
        public SystemSettings System = new();
        public PerformanceSettings Performance = new();

        [NonSerialized]
        private DalamudPluginInterface? pluginInterface;
        public void Initialize(DalamudPluginInterface inputPluginInterface) => pluginInterface = inputPluginInterface;
        public void Save() => pluginInterface!.SavePluginConfig(this);
    }
}