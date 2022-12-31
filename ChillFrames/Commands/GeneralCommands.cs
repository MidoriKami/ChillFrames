using System;
using System.Collections.Generic;
using ChillFrames.System;
using KamiLib.CommandSystem;
using KamiLib.Interfaces;
using KamiLib.Utilities;

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
                Service.Configuration.General.EnableLimiterSetting.Value = true;
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
                Service.Configuration.General.EnableLimiterSetting.Value = false;
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
                Chat.Print("Command", Service.Configuration.General.EnableLimiterSetting ? "Disabling Limiter" : "Enabling Limiter");
                Service.Configuration.General.EnableLimiterSetting.Value = !Service.Configuration.General.EnableLimiterSetting.Value;
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
                        Service.Configuration.General.FrameRateLimitSetting.Value = targetFramerate;
                        Service.Configuration.Save();
                        break;
                }
            },
            GetHelpText = () => "Set target framerate"
        },
        new SubCommand
        {
            CommandKeyword = "status",
            CanExecute = () => Service.Configuration.General.EnableLimiterSetting,
            CommandAction = () => Chat.Print("Command", FrameLimiterCondition.DisableFramerateLimit() ? "Limiter Inactive" : "Limiter Active"),
            GetHelpText = () => "Get limiter status"
        },
        new SubCommand
        {
            CommandKeyword = "status",
            CanExecute = () => !Service.Configuration.General.EnableLimiterSetting,
            CommandAction = () => Chat.Print("Command", "Limiter is disabled"),
            GetHelpText = () => "Get limiter status"
        },
        new SubCommand
        {
            CommandKeyword = "devset",
            Hidden = true,
            ParameterAction = strings =>
            {
                switch (strings)
                {
                    case null:
                    case [ null ]:
                        Chat.PrintError("Additional Parameter Missing");
                        break;
                    
                    case [ { } param ]:
                        var targetFramerate = Math.Clamp(int.Parse(param), 1, 255);
                        Chat.Print("Command", $"Setting Framerate Limit: {targetFramerate}");
                        Service.Configuration.General.FrameRateLimitSetting.Value = targetFramerate;
                        Service.Configuration.Save();
                        break;
                }
            },
            GetHelpText = () => "Set target framerate"
        },
    };
}