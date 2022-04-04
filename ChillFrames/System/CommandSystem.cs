using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChillFrames.Commands;
using ChillFrames.Interfaces;
using ChillFrames.Windows;

namespace ChillFrames.System
{
    internal class CommandSystem
    {
        private readonly List<ICommand> commandProcessors = new()
        {
            new GeneralCommands()
        };

        public void DispatchCommands(string command, string arguments)
        {
            if (arguments == string.Empty)
            {
                ChillFramesPlugin.SettingsWindow.Toggle();
            }
            else
            {
                foreach (var commandProcessor in commandProcessors)
                {
                    commandProcessor.ProcessCommand(command, arguments);
                }
            }
        }
    }
}
