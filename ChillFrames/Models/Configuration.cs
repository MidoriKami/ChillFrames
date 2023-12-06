using Dalamud.Configuration;

namespace ChillFrames.Models;

public class Configuration : IPluginConfiguration {
    public BlacklistSettings Blacklist = new();
    public float DisableIncrementSetting = 0.025f;
    public float EnableIncrementSetting = 0.01f;

    public GeneralSettings General = new();
    public LimiterSettings Limiter = new();

    public bool PluginEnable = true;
    public int Version { get; set; } = 3;

    public void Save() => Service.PluginInterface.SavePluginConfig(this);
}