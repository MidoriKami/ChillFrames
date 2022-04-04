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
        public void Dispose()
        {

        }

        public string TabName => "Debug";
        public bool Enabled => Service.ClientState.LocalContentId is 18014498561844389 or 18014498563616840;

        public void Draw()
        {
            Utilities.Draw.Checkbox("Enable Debug Output", ref Service.Configuration.System.EnableDebugOutput);
        }
    }
}
