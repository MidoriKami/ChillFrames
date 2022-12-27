using System;
using Dalamud.Configuration;
using Dalamud.Plugin;
using KamiLib.Configuration;

namespace ChillFrames.Config;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 1;

    public bool DevMode = false;
    public Setting<float> DisableIncrementSetting = new(0.025f);
    public Setting<float> EnableIncrementSetting = new(0.01f);

    public GeneralSettings General = new();
    public BlacklistSettings Blacklist = new();

    [NonSerialized]
    private DalamudPluginInterface? pluginInterface;
    public void Initialize(DalamudPluginInterface inputPluginInterface) => pluginInterface = inputPluginInterface;
    public void Save() => pluginInterface!.SavePluginConfig(this);
}