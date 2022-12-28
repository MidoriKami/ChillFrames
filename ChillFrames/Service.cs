using ChillFrames.Config;
using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.IoC;
using Dalamud.Plugin;

namespace ChillFrames;

public class Service
{
    [PluginService] public static DalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] public static ClientState ClientState { get; private set; } = null!;
    [PluginService] public static Framework Framework { get; private set; } = null!;

    public static Configuration Configuration { get; set; } = null!;
}