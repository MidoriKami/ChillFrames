using ChillFrames.Commands;
using ChillFrames.Config;
using ChillFrames.System;
using ChillFrames.Windows;
using Dalamud.IoC;
using Dalamud.Plugin;
using KamiLib;

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
        
        KamiCommon.Initialize(pluginInterface, Name, () => Service.Configuration.Save());

        // Load migrated config if needed
        ConfigMigration.LoadConfiguration();
        
        // If configuration json exists load it, if not make new config object
        Service.Configuration = Service.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        Service.Configuration.Initialize(Service.PluginInterface);

        KamiCommon.WindowManager.AddConfigurationWindow(new SettingsWindow());      
        
        KamiCommon.CommandManager.AddHandler(ShorthandCommand, "shorthand command to open configuration window");
        KamiCommon.CommandManager.AddCommand(new GeneralCommands());
        
        // Create Systems
        frameLimiter = new FrameLimiter();
    }
    
    public void Dispose()
    {
        KamiCommon.Dispose();

        frameLimiter.Dispose();
    }
}