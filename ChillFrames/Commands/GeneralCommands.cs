using System.Collections.Generic;
using ChillFrames.Interfaces;
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
        "toggle"
    };

    void ICommand.Execute(string primaryCommand, string? secondaryCommand)
    {
        switch (primaryCommand)
        {
            case "on" or "enable":
                Chat.Print("Command", "Enabling Limiter");
                Service.Configuration.General.EnableLimiter = true;
                break;

            case "off" or "disable":
                Chat.Print("Command", "Disabling Limiter");
                Service.Configuration.General.EnableLimiter = false;
                break;
            
            case "toggle":
                Chat.Print("Command", Service.Configuration.General.EnableLimiter ? "Disabling Limiter" : "Enabling Limiter");
                Service.Configuration.General.EnableLimiter = !Service.Configuration.General.EnableLimiter;
                break;
        }
    }
}