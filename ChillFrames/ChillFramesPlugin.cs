using ChillFrames.Controllers;
using Dalamud.Plugin;

namespace ChillFrames;

public sealed class ChillFramesPlugin : IDalamudPlugin {
    public static ChillFramesSystem System = null!;

    public ChillFramesPlugin(DalamudPluginInterface pluginInterface) {
        pluginInterface.Create<Service>();

        // We need to disable these, so users can monitor the config window and see what conditions are active at what times.
        pluginInterface.UiBuilder.DisableCutsceneUiHide = true;
        pluginInterface.UiBuilder.DisableAutomaticUiHide = true;
        pluginInterface.UiBuilder.DisableGposeUiHide = true;
        pluginInterface.UiBuilder.DisableUserUiHide = true;

        System = new ChillFramesSystem();
    }

    public void Dispose() {
        System.Dispose();
    }
}