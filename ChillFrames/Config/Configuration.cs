using Dalamud.Configuration;

namespace ChillFrames.Config;

public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 3;

    public bool PluginEnable = true;
    public float DisableIncrementSetting = 0.025f;
    public float EnableIncrementSetting = 0.01f;

    public GeneralSettings General = new();
    public LimiterSettings Limiter = new();
    public BlacklistSettings Blacklist = new();

    public void Save() => Service.PluginInterface.SavePluginConfig(this);
}