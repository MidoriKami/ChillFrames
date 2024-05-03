// ReSharper disable UnusedMember.Local

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ChillFrames.Interfaces;
using ChillFrames.Views.ConfigWindow;
using KamiLib.Classes;
using KamiLib.CommandManager;
using KamiLib.Window;
using Newtonsoft.Json.Linq;
using Configuration = ChillFrames.Models.Configuration;

namespace ChillFrames.Controllers;

public class ChillFramesSystem : IDisposable {
    public static Configuration Config = null!;
    public static HashSet<string> BlockList = [];
    private readonly FrameLimiterController frameLimiterController;
    private readonly ChillFramesIpcController ipcController;
    public List<IFrameLimiterOption> LimiterOptions;
    public static WindowManager WindowManager = null!;
    public static CommandManager CommandManager = null!;

    public ChillFramesSystem() {
        LimiterOptions = Reflection.ActivateOfInterface<IFrameLimiterOption>().ToList();
        
        Config = Service.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();

        frameLimiterController = new FrameLimiterController();
        ipcController = new ChillFramesIpcController();
        CommandManager = new CommandManager(Service.CommandManager, Service.PluginLog, "chillframes", "pcf");
        WindowManager = new WindowManager(Service.PluginInterface);

        WindowManager.AddWindow(new SettingsWindow(), true, true);
    }

    public void Dispose() {
        frameLimiterController.Dispose();
        ipcController.Dispose();
        WindowManager.Dispose();
        CommandManager.Dispose();
    }

    // [SingleTierCommandHandler("Enable Plugin's Framerate Limiter", "enable")]
    // private void EnableLimiter() {
    //     Config.PluginEnable = true;
    //     Config.Save();
    // }
    //
    // [SingleTierCommandHandler("Disable Plugin's Framerate Limiter", "disable")]
    // private void DisableLimiter() {
    //     Config.PluginEnable = false;
    //     Config.Save();
    // }
    //
    // [SingleTierCommandHandler("Toggle Plugin's Framerate Limiter", "toggle")]
    // private void ToggleLimiter() {
    //     Config.PluginEnable = !Config.PluginEnable;
    //     Config.Save();
    // }
    //
    // [DoubleTierCommandHandler("Set the Lower Limit to the specified value", "fps", "setlower")]
    // private void SetIdleLimit(params string[] args) {
    //     if (args.Length < 1) return;
    //     if (int.Parse(args[0]) < 1) return;
    //
    //     Config.Limiter.IdleFramerateTarget = int.Parse(args[0]);
    //     Config.Save();
    // }
    //
    // [DoubleTierCommandHandler("Set the Upper Limit to the specified value", "fps", "setupper")]
    // private void SetActiveLimit(params string[] args) {
    //     if (args.Length < 1) return;
    //     if (int.Parse(args[0]) < 1) return;
    //
    //     Config.Limiter.ActiveFramerateTarget = int.Parse(args[0]);
    //     Config.Save();
    // }
}