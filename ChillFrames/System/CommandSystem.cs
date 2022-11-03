using System.Collections.Generic;
using System.Linq;
using ChillFrames.Commands;
using ChillFrames.Interfaces;
using ChillFrames.Utilities;

namespace ChillFrames.System;

internal class CommandSystem
{
    private readonly List<ICommand> commandProcessors = new()
    {
        new GeneralCommands()
    };

    public void DispatchCommands(string command, string arguments)
    {
        var primaryCommand = CommandHelper.GetPrimaryCommand(arguments)?.ToLower();
        
        if (arguments == string.Empty)
        {
            ChillFramesPlugin.SettingsWindow.Toggle();
        }
        else if(primaryCommand != null)
        {
            if (commandProcessors.Any(moduleCommand => moduleCommand.ModuleCommands.Contains(primaryCommand)))
            {
                foreach (var commandProcessor in commandProcessors)
                {
                    commandProcessor.ProcessCommand(command, arguments);
                }
            }
            else
            {
                Chat.Error("Command", $"Invalid Command '{arguments}'");
            }
        }
    }
}