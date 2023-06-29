using Dalamud.Configuration;

namespace ChillFrames.Config;

public class ConfigVersion : IPluginConfiguration
{
    public int Version { get; set; }
}