using System.Collections.Generic;
using ChillFrames.Utilities;

namespace ChillFrames.Interfaces;

internal interface ICommand
{
    protected List<string> ModuleCommands { get; }

    protected void Execute(string primaryCommand, string? secondaryCommand);

    public void ProcessCommand(string command, string arguments)
    {
        var primaryCommand = CommandHelper.GetPrimaryCommand(arguments)?.ToLower();
        var secondaryCommand = CommandHelper.GetSecondaryCommand(arguments)?.ToLower();

        if (primaryCommand != null)
        {
            if (ModuleCommands.Contains(primaryCommand))
            {
                Execute(primaryCommand, secondaryCommand);
            }
        }
    }
}