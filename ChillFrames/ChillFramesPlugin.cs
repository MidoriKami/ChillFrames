using ChillFrames.System;
using ChillFrames.Windows;
using Dalamud.Plugin;
using KamiLib;
using KamiLib.System;

namespace ChillFrames;

public sealed class ChillFramesPlugin : IDalamudPlugin
{
    public static ChillFramesSystem System = null!;
    
    public ChillFramesPlugin(DalamudPluginInterface pluginInterface)
    {
        pluginInterface.Create<Service>();
        
        KamiCommon.Initialize(pluginInterface, "ChillFrames");

        System = new ChillFramesSystem();

        KamiCommon.WindowManager.AddConfigurationWindow(new SettingsWindow());      
        
        CommandController.RegisterMainCommand("/chillframes", "/pcf");
    }
    
    public void Dispose()
    {
        KamiCommon.Dispose();

        System.Dispose();
    }
}