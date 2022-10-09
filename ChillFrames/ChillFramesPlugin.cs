using ChillFrames.Data;
using ChillFrames.System;
using ChillFrames.Windows;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;

namespace ChillFrames;

public sealed class ChillFramesPlugin : IDalamudPlugin
{
    public string Name => "ChillFrames";
    private const string ShorthandCommand = "/pcf";

    public static SettingsWindow SettingsWindow = null!;
    private readonly CommandSystem commandSystem;
    private readonly FrameLimiter frameLimiter;

    public ChillFramesPlugin(
        [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface)
    {
        // Create Static Services for use everywhere
        pluginInterface.Create<Service>();
            
        // If configuration json exists load it, if not make new config object
        Service.Configuration = Service.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        Service.Configuration.Initialize(Service.PluginInterface);

        // Register Slash Commands
        Service.Commands.AddHandler(ShorthandCommand, new CommandInfo(OnCommand)
        {
            HelpMessage = "shorthand command to open configuration window"
        });

        // Create Systems
        SettingsWindow = new SettingsWindow();
        commandSystem = new CommandSystem();
        frameLimiter = new FrameLimiter();

        // Register draw callbacks
        Service.PluginInterface.UiBuilder.Draw += Service.WindowSystem.Draw;
        Service.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
    }

    private void OnCommand(string command, string arguments) => commandSystem.DispatchCommands(command, arguments);

    private void DrawConfigUI() => SettingsWindow.Toggle();

    public void Dispose()
    {
        Service.PluginInterface.UiBuilder.Draw -= Service.WindowSystem.Draw;
        Service.PluginInterface.UiBuilder.OpenConfigUi -= DrawConfigUI;

        SettingsWindow.Dispose();
        frameLimiter.Dispose();

        Service.Commands.RemoveHandler(ShorthandCommand);
    }
}