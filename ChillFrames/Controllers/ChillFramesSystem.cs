using System;
using System.Collections.Generic;
using System.Linq;
using ChillFrames.ConfigWindow;
using ChillFrames.LimiterOptions;
using KamiLib.Classes;
using KamiLib.CommandManager;
using KamiLib.Window;
using Configuration = ChillFrames.Configuration;

namespace ChillFrames.Controllers;

public class ChillFramesSystem : IDisposable {
    public static Configuration Config = null!;
    public static WindowManager WindowManager = null!;
    public static CommandManager CommandManager = null!;
    public static DtrController DtrController = null!;

    public static HashSet<string> BlockList = [];

    private readonly FrameLimiterController frameLimiterController;
    private readonly ChillFramesIpcController ipcController;
    
    public List<IFrameLimiterOption> LimiterOptions;

    public ChillFramesSystem() {
        LimiterOptions = Reflection.ActivateOfInterface<IFrameLimiterOption>().ToList();
        
        Config = Service.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();

        frameLimiterController = new FrameLimiterController();
        ipcController = new ChillFramesIpcController();
        CommandManager = new CommandManager(Service.PluginInterface, "chillframes", "pcf");
        WindowManager = new WindowManager(Service.PluginInterface);
        DtrController = new DtrController(); 

        WindowManager.AddWindow(new SettingsWindow(), WindowFlags.IsConfigWindow);

        CommandManager.RegisterCommand(new ToggleCommandHandler {
            EnableDelegate = EnableLimiter,
            DisableDelegate = DisableLimiter,
            ToggleDelegate = ToggleLimiter,
            BaseActivationPath = string.Empty,
        });
        
        CommandManager.RegisterCommand(new CommandHandler {
            Delegate = SetIdleLimit,
            ActivationPath = "/fps/setlower",
        });
        
        CommandManager.RegisterCommand(new CommandHandler {
            Delegate = SetActiveLimit,
            ActivationPath = "/fps/setupper",
        });
    }

    public void Dispose() {
        frameLimiterController.Dispose();
        ipcController.Dispose();
        WindowManager.Dispose();
        CommandManager.Dispose();
    }

    private void EnableLimiter(params string[] args) {
        Config.PluginEnable = true;
        Config.Save();
    }

    private void DisableLimiter(params string[] args) {
        Config.PluginEnable = false;
        Config.Save();
    }
    
    private void ToggleLimiter(params string[] args) {
        if (args.Any()) {
            if (bool.TryParse(args[0], out var value)) {
                Config.PluginEnable = value;
                Config.Save();
            }
        }
        else {
            Config.PluginEnable = !Config.PluginEnable;
            Config.Save();
        }
    }
    
    private void SetIdleLimit(params string[] args) {
        if (args.Length < 1) return;
        if (!int.TryParse(args[0], out var newTarget) || newTarget <= 0) return;
    
        Config.Limiter.IdleFramerateTarget = newTarget;
        Config.Save();
    }
    
    private void SetActiveLimit(params string[] args) {
        if (args.Length < 1) return;
        if (!int.TryParse(args[0], out var newTarget) || newTarget <= 0) return;
    
        Config.Limiter.ActiveFramerateTarget = newTarget;
        Config.Save();
    }
}