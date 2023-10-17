using ChillFrames.Controllers;
using ChillFrames.Views.ConfigWindow;
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

        // We need to disable these, so users can monitor the config window and see what conditions are active at what times.
        pluginInterface.UiBuilder.DisableCutsceneUiHide = true;
        pluginInterface.UiBuilder.DisableAutomaticUiHide = true;
        pluginInterface.UiBuilder.DisableGposeUiHide = true;
        pluginInterface.UiBuilder.DisableUserUiHide = true;

        KamiCommon.Initialize(pluginInterface, "ChillFrames");

        System = new ChillFramesSystem();

        KamiCommon.WindowManager.AddConfigurationWindow(new SettingsWindow(), true);

        CommandController.RegisterMainCommand("/chillframes", "/pcf");
    }

    public void Dispose()
    {
        KamiCommon.Dispose();

        System.Dispose();
    }
}