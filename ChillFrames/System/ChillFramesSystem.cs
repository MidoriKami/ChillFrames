// ReSharper disable UnusedMember.Local
using System;
using System.Collections.Generic;
using System.IO;
using ChillFrames.Config;
using KamiLib.AutomaticUserInterface;
using KamiLib.Commands;
using Newtonsoft.Json.Linq;

namespace ChillFrames.System;

public class ChillFramesSystem : IDisposable
{
    public static Configuration Config = null!;
    private readonly FrameLimiter frameLimiter;
    private readonly ChillFramesIpcController ipcController;
    public static HashSet<string> BlockList = null!;
    
    public ChillFramesSystem()
    {
        CommandController.RegisterCommands(this);
        
        LoadConfiguration();

        BlockList = new HashSet<string>();
        frameLimiter = new FrameLimiter();
        ipcController = new ChillFramesIpcController();
    }

    public void Dispose()
    {
        frameLimiter.Dispose();
        ipcController.Dispose();
    }

    private void LoadConfiguration()
    {
        // If we have an existing config
        if (Service.PluginInterface.ConfigFile is { Exists: true, FullName: var path} )
        {
            var fileText = File.ReadAllText(path);

            switch (JObject.Parse(fileText).GetValue("Version")?.ToObject<int>())
            {
                case not 3:
                    Config = new Configuration();
                    Config.Save();
                    break;
                
                default:
                    Config = Service.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
                    break;
            }
        }
        else
        {
            // We don't have an existing config, make a new one
            Config = new Configuration();
            Config.Save();
        }
    }

    public void DrawGeneralConfig() => DrawableAttribute.DrawAttributes(Config.General, Config.Save);
    public void DrawLimiterConfig() => DrawableAttribute.DrawAttributes(Config.Limiter, Config.Save);
    public void DrawZoneFilterConfig() => DrawableAttribute.DrawAttributes(Config.Blacklist, Config.Save);

    [SingleTierCommandHandler("Enable Plugin's Framerate Limiter", "enable")]
    private void EnableLimiter()
    {
        Config.PluginEnable = true;
        Config.Save();
    }
    
    [SingleTierCommandHandler("Disable Plugin's Framerate Limiter", "disable")]
    private void DisableLimiter()
    {
        Config.PluginEnable = false;
        Config.Save();
    }
    
    [SingleTierCommandHandler("Toggle Plugin's Framerate Limiter", "toggle")]
    private void ToggleLimiter()
    {
        Config.PluginEnable = !Config.PluginEnable;
        Config.Save();
    }

    [DoubleTierCommandHandler("Set the Idle Limiter to the specified value", "idle", "set")]
    private void SetIdleLimit(params string[] args)
    {
        if (args.Length < 1) return;
        if (int.Parse(args[0]) < 1) return;
        
        Config.Limiter.IdleFramerateTarget = int.Parse(args[0]);
        Config.Save();
    }

    [DoubleTierCommandHandler("Set the Active Limiter to the specified value", "active", "set")]
    private void SetActiveLimit(params string[] args)
    {
        if (args.Length < 1) return;
        if (int.Parse(args[0]) < 1) return;

        Config.Limiter.ActiveFramerateTarget = int.Parse(args[0]);
        Config.Save();
    }
}