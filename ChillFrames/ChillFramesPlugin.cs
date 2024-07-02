using System.Linq;
using ChillFrames.Classes;
using ChillFrames.Controllers;
using ChillFrames.Windows;
using Dalamud.Plugin;
using KamiLib.Classes;
using KamiLib.CommandManager;
using KamiLib.Window;

namespace ChillFrames;

public sealed class ChillFramesPlugin : IDalamudPlugin {
    public ChillFramesPlugin(IDalamudPluginInterface pluginInterface) {
        pluginInterface.Create<Service>();

        // We need to disable these, so users can monitor the config window and see what conditions are active at what times.
        pluginInterface.UiBuilder.DisableCutsceneUiHide = true;
        pluginInterface.UiBuilder.DisableAutomaticUiHide = true;
        pluginInterface.UiBuilder.DisableGposeUiHide = true;
        pluginInterface.UiBuilder.DisableUserUiHide = true;

        System.LimiterOptions = Reflection.ActivateOfInterface<IFrameLimiterOption>().ToList();

        System.Config = Configuration.Load();

        System.frameLimiterController = new FrameLimiterController();
        System.ipcController = new IpcController();
        System.CommandManager = new CommandManager(Service.PluginInterface, "chillframes", "pcf");
        System.WindowManager = new WindowManager(Service.PluginInterface);
        System.DtrController = new DtrController(); 

        System.WindowManager.AddWindow(new SettingsWindow(), WindowFlags.IsConfigWindow | WindowFlags.OpenImmediately);

        System.CommandManager.RegisterCommand(new ToggleCommandHandler {
            EnableDelegate = EnableLimiter,
            DisableDelegate = DisableLimiter,
            ToggleDelegate = ToggleLimiter,
            BaseActivationPath = string.Empty,
        });
        
        System.CommandManager.RegisterCommand(new CommandHandler {
            Delegate = SetIdleLimit,
            ActivationPath = "/fps/setlower",
        });
        
        System.CommandManager.RegisterCommand(new CommandHandler {
            Delegate = SetActiveLimit,
            ActivationPath = "/fps/setupper",
        });
    }

    public void Dispose() {
        System.frameLimiterController.Dispose();
        System.ipcController.Dispose();
        System.WindowManager.Dispose();
        System.CommandManager.Dispose();
    }
    
    private void EnableLimiter(params string[] args) {
        System.Config.PluginEnable = true;
        System.Config.Save();
    }

    private void DisableLimiter(params string[] args) {
        System.Config.PluginEnable = false;
        System.Config.Save();
    }
    
    private void ToggleLimiter(params string[] args) {
        if (args.Length != 0) {
            if (bool.TryParse(args[0], out var value)) {
                System.Config.PluginEnable = value;
                System.Config.Save();
            }
        }
        else {
            System.Config.PluginEnable = !System.Config.PluginEnable;
            System.Config.Save();
        }
    }
    
    private void SetIdleLimit(params string[] args) {
        if (args.Length < 1) return;
        if (!int.TryParse(args[0], out var newTarget) || newTarget <= 0) return;
    
        System.Config.Limiter.IdleFramerateTarget = newTarget;
        System.Config.Save();
    }
    
    private void SetActiveLimit(params string[] args) {
        if (args.Length < 1) return;
        if (!int.TryParse(args[0], out var newTarget) || newTarget <= 0) return;
    
        System.Config.Limiter.ActiveFramerateTarget = newTarget;
        System.Config.Save();
    }
}