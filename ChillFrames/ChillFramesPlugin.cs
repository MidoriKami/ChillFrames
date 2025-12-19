using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ChillFrames.Classes;
using ChillFrames.Controllers;
using ChillFrames.Utilities;
using ChillFrames.Windows;
using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;

namespace ChillFrames;

public sealed class ChillFramesPlugin : IDalamudPlugin {
    public ChillFramesPlugin(IDalamudPluginInterface pluginInterface) {
        pluginInterface.Create<Services>();

        // We need to disable these, so users can monitor the config window and see what conditions are active at what times.
        pluginInterface.UiBuilder.DisableCutsceneUiHide = true;
        pluginInterface.UiBuilder.DisableAutomaticUiHide = true;
        pluginInterface.UiBuilder.DisableGposeUiHide = true;
        pluginInterface.UiBuilder.DisableUserUiHide = true;

        System.LimiterOptions = GetFrameLimiterOptions();

        System.Config = Configuration.Load();

        System.DtrController = new DtrController();
        System.FrameLimiterController = new FrameLimiterController();
        System.IpcController = new IpcController();
        Services.CommandManager.AddHandler("/chillframes", new CommandInfo(OnCommand) {
            ShowInHelp = true, HelpMessage = "Open ChillFrames Config",
        });

        Services.CommandManager.AddHandler("/pcf", new CommandInfo(OnCommand) {
            ShowInHelp = true, HelpMessage = "Open ChillFrames Config",
        });

        System.WindowSystem = new WindowSystem("ChillFrames");
        System.ConfigWindow = new SettingsWindow();

        System.WindowSystem.AddWindow(System.ConfigWindow);

        Services.PluginInterface.UiBuilder.Draw += System.WindowSystem.Draw;
        Services.PluginInterface.UiBuilder.OpenConfigUi += System.ConfigWindow.Toggle;
        Services.PluginInterface.UiBuilder.OpenMainUi += System.ConfigWindow.Toggle;
        
        System.ConfigWindow.IsOpen = true;
    }

    private void OnCommand(string command, string arguments) {
        if (Services.Condition.IsInCombat) {
            Services.ChatGui.PrintError("Unable to modify ChillFrames config while in combat.");
            return;
        }
        
        if (command is not ( "/chillframes" or "/pcf" )) return;
        
        switch (arguments.Split(' ')) {
            case [ "" ] or []:
                System.ConfigWindow.Toggle();
                break;
            
            case [ "enable" ]:
                System.Config.PluginEnable = true;
                break;
            
            case [ "disable" ]:
                System.Config.PluginEnable = false;
                break;
            
            case [ "toggle" ]:
                System.Config.PluginEnable = !System.Config.PluginEnable;
                break;

            case [ "fps", "setlower", { } newLowerLimit ]:
                if (!int.TryParse(newLowerLimit, out var newLowTarget) || newLowTarget <= 0) return;
                System.Config.Limiter.IdleFramerateTarget = newLowTarget;
                break;

            case [ "fps", "setupper", { } newUpperLimit ]:
                if (!int.TryParse(newUpperLimit, out var newHighTarget) || newHighTarget <= 0) return;
                System.Config.Limiter.ActiveFramerateTarget = newHighTarget;
                break;
        }
        
        System.Config.Save();
    }

    public void Dispose() {
        Services.PluginInterface.UiBuilder.Draw -= System.WindowSystem.Draw;
        Services.PluginInterface.UiBuilder.OpenConfigUi -= System.ConfigWindow.Toggle;
        Services.PluginInterface.UiBuilder.OpenMainUi -= System.ConfigWindow.Toggle;
        
        Services.CommandManager.RemoveHandler("/chillframes");
        Services.CommandManager.RemoveHandler("/pcf");
            
        System.FrameLimiterController.Dispose();
        System.IpcController.Dispose();
        System.WindowSystem.RemoveAllWindows();
    }
    
    private static List<IFrameLimiterOption> GetFrameLimiterOptions() => Assembly
        .GetCallingAssembly()
        .GetTypes()
        .Where(type => type.IsAssignableTo(typeof(IFrameLimiterOption)))
        .Where(type => !type.IsAbstract)
        .Select(type => (IFrameLimiterOption?) Activator.CreateInstance(type))
        .OfType<IFrameLimiterOption>()
        .ToList();
}