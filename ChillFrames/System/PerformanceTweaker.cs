using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChillFrames.Data.Enums;
using ChillFrames.Data.SettingsObjects;
using Dalamud.Game;
using XivCommon;

namespace ChillFrames.System
{
    internal class PerformanceTweaker : IDisposable
    {
        private PerformanceSettings Settings => Service.Configuration.Performance;

        private PerformanceTweakerState state = PerformanceTweakerState.Unset;

        public PerformanceTweaker()
        {
            Service.Framework.Update += FrameworkUpdate;
        }

        private void FrameworkUpdate(Framework framework)
        {
            if (Settings.Enabled)
            {
                switch (state)
                {
                    case PerformanceTweakerState.Unset when Condition.EnableFramerateLimit() == true:
                        Settings.ActiveProfile.Apply();
                        state = PerformanceTweakerState.Active;
                        break;

                    case PerformanceTweakerState.Unset when Condition.EnableFramerateLimit() == false:
                        Settings.InactiveProfile.Apply();
                        state = PerformanceTweakerState.Inactive;
                        break;

                    case PerformanceTweakerState.Active when Condition.EnableFramerateLimit() == false:
                        Settings.InactiveProfile.Apply();
                        state = PerformanceTweakerState.Inactive;
                        break;

                    case PerformanceTweakerState.Inactive when Condition.EnableFramerateLimit() == true:
                        Settings.ActiveProfile.Apply();
                        state = PerformanceTweakerState.Active;
                        break;

                    default:
                        // Do nothing
                        break;
                }
            }
        }

        public void Dispose()
        {
            Service.Framework.Update -= FrameworkUpdate;
        }
    }
}
