using System;
using System.Collections.Generic;
using ChillFrames.Interfaces;
using ChillFrames.System;
using ChillFrames.Utilities;

namespace ChillFrames.Commands;

internal class GeneralCommands : ICommand
{
    List<string> ICommand.ModuleCommands { get; } = new()
    {
        "enable",
        "disable",
        "on",
        "off",
        "toggle",
        "set",
        "status"
    };

    void ICommand.Execute(string primaryCommand, string? secondaryCommand)
    {
        switch (primaryCommand)
        {
            case "on" or "enable":
                Chat.Print("Command", "Enabling Limiter");
                Service.Configuration.General.EnableLimiter = true;
                Service.Configuration.Save();
                break;

            case "off" or "disable":
                Chat.Print("Command", "Disabling Limiter");
                Service.Configuration.General.EnableLimiter = false;
                Service.Configuration.Save();
                break;
            
            case "toggle":
                Chat.Print("Command", Service.Configuration.General.EnableLimiter ? "Disabling Limiter" : "Enabling Limiter");
                Service.Configuration.General.EnableLimiter = !Service.Configuration.General.EnableLimiter;
                Service.Configuration.Save();
                break;
            
            case "set" when secondaryCommand != null:
                var targetFramerate = Math.Clamp(int.Parse(secondaryCommand), 10, 255);
                Chat.Print("Command", $"Setting Framerate Limit: {targetFramerate}");
                Service.Configuration.General.FrameRateLimit = targetFramerate;
                Service.Configuration.Save();
                break;
            
            case "status" when Service.Configuration.General.EnableLimiter:
                Chat.Print("Command", Condition.DisableFramerateLimit() ? "Limiter Inactive" : "Limiter Active");
                break;
            
            case "status" when !Service.Configuration.General.EnableLimiter:
                Chat.Print("Command", "Limiter is disabled");
                break;
        }
        
    }
}