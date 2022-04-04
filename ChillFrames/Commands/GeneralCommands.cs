using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChillFrames.Interfaces;

namespace ChillFrames.Commands
{
    internal class GeneralCommands : ICommand
    {
        List<string> ICommand.ModuleCommands { get; } = new()
        {
            "enable",
            "disable",
            "on",
            "off"
        };

        void ICommand.Execute(string primaryCommand, string? secondaryCommand)
        {
            switch (primaryCommand)
            {
                case null:
                    break;

                case "on" or "enable":
                    Service.Configuration.General.EnableLimiter = true;
                    break;

                case "off" or "disable":
                    Service.Configuration.General.EnableLimiter = false;
                    break;
            }
        }
    }
}
