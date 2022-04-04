using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChillFrames.Interfaces;

namespace ChillFrames.Tabs
{
    internal class PerformanceProfiles : ITabItem
    {
        public string TabName => "Performance";
        public bool Enabled => Service.Configuration.Performance.Enabled;
        
        public void Draw()
        {

        }

        public void Dispose()
        {

        }
    }
}
