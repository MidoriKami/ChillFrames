using ChillFrames.Data;
using ChillFrames.System;
using ChillFrames.Utilities;
using ChillFrames.Windows;
using Dalamud.Game;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using XivCommon;
// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace ChillFrames
{
    public sealed class ChillFramesPlugin : IDalamudPlugin
    {
        public string Name => "ChillFrames";
        private const string SettingsCommand = "/pchillframes";
        private const string ShorthandCommand = "/pcf";

        public static SettingsWindow SettingsWindow = null!;
        private CommandSystem commandSystem;
        private FrameLimiter frameLimiter;
        private PerformanceTweaker performanceTweaker;

        public ChillFramesPlugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface)
        {
            // Create Static Services for use everywhere
            pluginInterface.Create<Service>();
            
            // If configuration json exists load it, if not make new config object
            Service.Configuration = Service.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            Service.Configuration.Initialize(Service.PluginInterface);

            // Register Slash Commands
            Service.Commands.AddHandler(SettingsCommand, new CommandInfo(OnCommand)
            {
                HelpMessage = "open configuration window"
            });

            Service.Commands.AddHandler(ShorthandCommand, new CommandInfo(OnCommand)
            {
                HelpMessage = "shorthand command to open configuration window"
            });

            // Initialize XivCommon
            Service.XivCommon = new XivCommonBase();

            // Create Systems
            SettingsWindow = new SettingsWindow();
            commandSystem = new CommandSystem();
            frameLimiter = new FrameLimiter();
            performanceTweaker = new PerformanceTweaker();

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

            Service.XivCommon.Dispose();

            SettingsWindow.Dispose();
            frameLimiter.Dispose();
            performanceTweaker.Dispose();

            Service.Commands.RemoveHandler(SettingsCommand);
            Service.Commands.RemoveHandler(ShorthandCommand);
        }
    }
}