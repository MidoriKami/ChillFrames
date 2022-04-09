using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChillFrames.Data.SettingsObjects;
using ChillFrames.Interfaces;
using ChillFrames.System;
using Dalamud.Interface;
using Dalamud.Interface.Components;
using ImGuiNET;

namespace ChillFrames.Tabs
{
    internal class GeneralConfigurationTab : ITabItem
    {
        private GeneralSettings Settings => Service.Configuration.General;
        private BlacklistSettings Blacklist => Service.Configuration.Blacklist;

        private PerformanceSettings Performance => Service.Configuration.Performance;

        public string TabName => "Conditions";
        public bool Enabled => true;


        private readonly Stopwatch timer = new();

        private int newFramerateLimit;

        public GeneralConfigurationTab()
        {
            newFramerateLimit = Settings.FrameRateLimit;
        }

        public void Draw()
        {
            ImGui.Checkbox("Disable during combat", ref Settings.DisableDuringCombat);
            ImGui.Checkbox("Disable during duty", ref Settings.DisableDuringDuty);
            ImGui.Checkbox("Disable during cutscene", ref Settings.DisableDuringCutscene);
            ImGui.Checkbox("Disable in specific zones", ref Blacklist.Enabled);
            ImGui.Checkbox("Disable during gpose", ref Settings.DisableDuringGpose);
            ImGui.Checkbox("Disable during Quest Events", ref Settings.DisableDuringQuestEvent);

            ImGui.Checkbox("Enable Performance Profiles", ref Performance.Enabled);

            ImGui.Spacing();
            ImGui.Spacing();
            ImGui.Spacing();

            ImGui.SetNextItemWidth(50 * ImGuiHelpers.GlobalScale);

            if (ImGui.InputInt("Framerate Limit", ref newFramerateLimit, 0, 0))
            {
                timer.Restart();
            }

            if (timer.IsRunning && timer.Elapsed.Seconds >= 1)
            {
                timer.Stop();
                Settings.FrameRateLimit = Math.Max(newFramerateLimit, 10);
            }

            ImGuiComponents.HelpMarker("The framerate value to limit the game to\n" + "Minimum: 10");

            var frametimeExact = 1000 / Settings.FrameRateLimit + 1;
            var approximateFramerate = 1000 / frametimeExact;

            Utilities.Draw.NumericDisplay("Approximated Framerate", approximateFramerate);
            ImGuiComponents.HelpMarker("Framerate limit will be approximated not exact");
        }

        public void Dispose()
        {

        }
    }
}
