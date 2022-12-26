using ChillFrames.Commands;
using ChillFrames.Data;
using ChillFrames.System;
using ChillFrames.Windows;
using Dalamud.IoC;
using Dalamud.Plugin;

namespace ChillFrames;

public sealed class ChillFramesPlugin : IDalamudPlugin
{
    public string Name => "ChillFrames";
    private const string ShorthandCommand = "/pcf";

    public static SettingsWindow SettingsWindow = null!;
    private readonly FrameLimiter frameLimiter;

    public ChillFramesPlugin(
        [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface)
    {
        // Create Static Services for use everywhere
        pluginInterface.Create<Service>();
            
        KamiLib.KamiLib.Initialize(pluginInterface, Name);
        KamiLib.KamiLib.CommandManager.AddHandler(ShorthandCommand, "shorthand command to open configuration window");
        KamiLib.KamiLib.CommandManager.AddCommand(new GeneralCommands());

        // If configuration json exists load it, if not make new config object
        Service.Configuration = Service.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        Service.Configuration.Initialize(Service.PluginInterface);

        // Create Systems
        SettingsWindow = new SettingsWindow();
        frameLimiter = new FrameLimiter();

        // Register draw callbacks
        Service.PluginInterface.UiBuilder.Draw += Service.WindowSystem.Draw;
        Service.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
    }
    
    private void DrawConfigUI() => SettingsWindow.Toggle();

    public void Dispose()
    {
        KamiLib.KamiLib.Dispose();
        
        Service.PluginInterface.UiBuilder.Draw -= Service.WindowSystem.Draw;
        Service.PluginInterface.UiBuilder.OpenConfigUi -= DrawConfigUI;

        SettingsWindow.Dispose();
        frameLimiter.Dispose();
    }
}