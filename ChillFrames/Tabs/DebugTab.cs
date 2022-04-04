using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChillFrames.Interfaces;

namespace ChillFrames.Tabs
{
    internal class DebugTab : ITabItem
    {
        public string TabName => "Debug";
        public bool Enabled => Service.Configuration.devMode;

        public void Draw()
        {
            Utilities.Draw.Checkbox("Enable Debug Output", ref Service.Configuration.System.EnableDebugOutput);
        }

        public void Dispose()
        {

        }
    }
}
