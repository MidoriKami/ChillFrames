using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;

namespace ChillFrames
{
    public sealed class ChillFramesPlugin : IDalamudPlugin
    {
        public string Name => "ChillFrames";
        private const string SettingsCommand = "/pchillframes";
        private const string ShorthandCommand = "/pcf";

        private readonly SettingsWindow settingsWindow;
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
            Service.Commands.AddHandler(SettingsCommand, new CommandInfo(OnCommand)
            {
                HelpMessage = "open configuration window"
            });

            Service.Commands.AddHandler(ShorthandCommand, new CommandInfo(OnCommand)
            {
                HelpMessage = "shorthand command to open configuration window"
            });

            // Create Systems
            settingsWindow = new();
            frameLimiter = new();

            // Register draw callbacks
            Service.PluginInterface.UiBuilder.Draw += Service.WindowSystem.Draw;
            Service.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        }

        private void OnCommand(string command, string arguments) => settingsWindow.IsOpen = !settingsWindow.IsOpen;

        private void DrawConfigUI() => settingsWindow.IsOpen = !settingsWindow.IsOpen;

        public void Dispose()
        {
            Service.PluginInterface.UiBuilder.Draw -= Service.WindowSystem.Draw;
            Service.PluginInterface.UiBuilder.OpenConfigUi -= DrawConfigUI;

            settingsWindow.Dispose();
            frameLimiter.Dispose();

            Service.Commands.RemoveHandler(SettingsCommand);
            Service.Commands.RemoveHandler(ShorthandCommand);
        }
    }
}