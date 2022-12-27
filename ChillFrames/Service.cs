using ChillFrames.Config;
using Dalamud.Game.ClientState;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;

namespace ChillFrames;

public class Service
{
    [PluginService] public static DalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] public static ClientState ClientState { get; private set; } = null!;

    public static WindowSystem WindowSystem { get; } = new("ChillFrames");
    public static Configuration Configuration { get; set; } = null!;
}