using System;
using System.Collections.Generic;
using KamiLib.CommandSystem;
using KamiLib.Interfaces;
using KamiLib.Utilities;
using Condition = ChillFrames.System.Condition;

namespace ChillFrames.Commands;

public class GeneralCommands : IPluginCommand
{
    public string CommandArgument => "limiter";

    public IEnumerable<ISubCommand> SubCommands { get; } = new List<ISubCommand>
    {
        new SubCommand
        {
            CommandKeyword = "enable",
            Aliases = new List<string>{ "on" },
            CommandAction = () =>
            {
                Chat.Print("Command", "Enabling Limiter");
                Service.Configuration.General.EnableLimiter = true;
                Service.Configuration.Save(); 
            },
            GetHelpText = () => "Enable framerate limiter"
        },
        new SubCommand
        {
            CommandKeyword = "disable",
            Aliases = new List<string>{"off"},
            CommandAction = () =>
            {
                Chat.Print("Command", "Disabling Limiter");
                Service.Configuration.General.EnableLimiter = false;
                Service.Configuration.Save(); 
            },
            GetHelpText = () => "Disable framerate limiter"
        },
        new SubCommand
        {
            CommandKeyword = "toggle",
            Aliases = new List<string>{"t"},
            CommandAction = () =>
            {
                Chat.Print("Command", Service.Configuration.General.EnableLimiter ? "Disabling Limiter" : "Enabling Limiter");
                Service.Configuration.General.EnableLimiter = !Service.Configuration.General.EnableLimiter;
                Service.Configuration.Save();
            },
            GetHelpText = () => "Toggle framerate limiter"
        },
        new SubCommand
        {
            CommandKeyword = "set",
            ParameterAction = strings =>
            {
                switch (strings)
                {
                    case null:
                    case [ null ]:
                        Chat.PrintError("Additional Parameter Missing");
                        break;
                    
                    case [ { } param ]:
                        var targetFramerate = Math.Clamp(int.Parse(param), 10, 255);
                        Chat.Print("Command", $"Setting Framerate Limit: {targetFramerate}");
                        Service.Configuration.General.FrameRateLimit = targetFramerate;
                        Service.Configuration.Save();
                        break;
                }
            },
            GetHelpText = () => "Set target framerate"
        },
        new SubCommand
        {
            CommandKeyword = "status",
            CanExecute = () => Service.Configuration.General.EnableLimiter,
            CommandAction = () => Chat.Print("Command", Condition.DisableFramerateLimit() ? "Limiter Inactive" : "Limiter Active"),
            GetHelpText = () => "Get limiter status"
        },
        new SubCommand
        {
            CommandKeyword = "status",
            CanExecute = () => !Service.Configuration.General.EnableLimiter,
            CommandAction = () => Chat.Print("Command", "Limiter is disabled"),
            GetHelpText = () => "Get limiter status"
        }
    };
}

// internal class GeneralCommands : ICommand
// {
//     List<string> ICommand.ModuleCommands { get; } = new()
//     {
//         "enable",
//         "disable",
//         "on",
//         "off",
//         "toggle",
//         "set",
//         "status"
//     };
//
//     void ICommand.Execute(string primaryCommand, string? secondaryCommand)
//     {
//         switch (primaryCommand)
//         {
//             case "on" or "enable":
//                 Chat.Print("Command", "Enabling Limiter");
//                 Service.Configuration.General.EnableLimiter = true;
//                 Service.Configuration.Save();
//                 break;
//
//             case "off" or "disable":
//                 Chat.Print("Command", "Disabling Limiter");
//                 Service.Configuration.General.EnableLimiter = false;
//                 Service.Configuration.Save();
//                 break;
//             
//             case "toggle":
//                 Chat.Print("Command", Service.Configuration.General.EnableLimiter ? "Disabling Limiter" : "Enabling Limiter");
//                 Service.Configuration.General.EnableLimiter = !Service.Configuration.General.EnableLimiter;
//                 Service.Configuration.Save();
//                 break;
//             
//             case "set" when secondaryCommand != null:
//                 var targetFramerate = Math.Clamp(int.Parse(secondaryCommand), 10, 255);
//                 Chat.Print("Command", $"Setting Framerate Limit: {targetFramerate}");
//                 Service.Configuration.General.FrameRateLimit = targetFramerate;
//                 Service.Configuration.Save();
//                 break;
//             
//             case "status" when Service.Configuration.General.EnableLimiter:
//                 Chat.Print("Command", Condition.DisableFramerateLimit() ? "Limiter Inactive" : "Limiter Active");
//                 break;
//             
//             case "status" when !Service.Configuration.General.EnableLimiter:
//                 Chat.Print("Command", "Limiter is disabled");
//                 break;
//         }
//         
//     }
// }