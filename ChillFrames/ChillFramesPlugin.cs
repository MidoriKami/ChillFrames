using ChillFrames.Commands;
using ChillFrames.Config;
using ChillFrames.System;
using ChillFrames.Windows;
using Dalamud.IoC;
using Dalamud.Plugin;

namespace ChillFrames;

public sealed class ChillFramesPlugin : IDalamudPlugin
{
    public string Name => "ChillFrames";
    private const string ShorthandCommand = "/pcf";

    private readonly FrameLimiter frameLimiter;

    public ChillFramesPlugin(
        [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface)
    {
        // Create Static Services for use everywhere
        pluginInterface.Create<Service>();
        
        KamiLib.KamiLib.Initialize(pluginInterface, Name, () => Service.Configuration.Save());

        // If configuration json exists load it, if not make new config object
        Service.Configuration = Service.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        Service.Configuration.Initialize(Service.PluginInterface);

        KamiLib.KamiLib.CommandManager.AddHandler(ShorthandCommand, "shorthand command to open configuration window");
        KamiLib.KamiLib.CommandManager.AddCommand(new GeneralCommands());
        KamiLib.KamiLib.WindowManager.AddWindow(new SettingsWindow());        
        
        // Create Systems
        frameLimiter = new FrameLimiter();
    }
    
    public void Dispose()
    {
        KamiLib.KamiLib.Dispose();

        frameLimiter.Dispose();
    }
}