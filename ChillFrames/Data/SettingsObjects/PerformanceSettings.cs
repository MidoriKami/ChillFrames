using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChillFrames.Data.SettingsObjects.Components;

namespace ChillFrames.Data.SettingsObjects
{
    public class PerformanceSettings
    {
        public bool Enabled = false;

        /// <summary>
        /// Profile for when the framerate is being limited
        /// </summary>
        public PerformanceProfile ActiveProfile = new();

        /// <summary>
        /// Profile for when the framerate is not being limited
        /// </summary>
        public PerformanceProfile InactiveProfile = new();
    }
}
