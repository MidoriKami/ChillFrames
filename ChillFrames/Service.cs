using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace ChillFrames;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public class Service {
    [PluginService] public static IDalamudPluginInterface PluginInterface { get; set; }
    [PluginService] public static IClientState ClientState { get; set; }
    [PluginService] public static IFramework Framework { get; set; }
    [PluginService] public static ICondition Condition { get; set; }
    [PluginService] public static IDtrBar DtrBar { get; set; }
    [PluginService] public static IDataManager DataManager { get; set; }
    [PluginService] public static ITextureProvider TextureProvider { get; set; }
}