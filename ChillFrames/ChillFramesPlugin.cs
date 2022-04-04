using ChillFrames.Data;
using ChillFrames.System;
using ChillFrames.Windows;
using Dalamud.Game;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using XivCommon;

namespace ChillFrames
{
    public sealed class ChillFramesPlugin : IDalamudPlugin
    {
        public string Name => "ChillFrames";
        private const string SettingsCommand = "/pchillframes";
        private const string ShorthandCommand = "/pcf";

        public static SettingsWindow SettingsWindow = null!;
        private readonly CommandSystem commandSystem;
        private readonly FrameLimiter frameLimiter;
        private readonly PerformanceTweaker performanceTweaker;

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


            // Create Systems
            SettingsWindow = new SettingsWindow();
            commandSystem = new CommandSystem();
            frameLimiter = new FrameLimiter();
            performanceTweaker = new PerformanceTweaker();

            Service.XivCommon = new XivCommonBase();

            // Register draw callbacks
            Service.PluginInterface.UiBuilder.Draw += Service.WindowSystem.Draw;
            Service.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        }

        private void OnCommand(string command, string arguments) => commandSystem.DispatchCommands(command, arguments);

        private void DrawConfigUI() => SettingsWindow.IsOpen = !SettingsWindow.IsOpen;

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