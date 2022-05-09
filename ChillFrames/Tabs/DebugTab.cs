using System.Diagnostics;
using ChillFrames.Interfaces;
using ChillFrames.System;
using Dalamud.Interface;
using Dalamud.Interface.Components;
using ImGuiNET;

namespace ChillFrames.Tabs
{
    internal class DebugTab : ITabItem
    {
        public string TabName => "Debug";
        public bool Enabled => Service.Configuration.devMode;

        private readonly Stopwatch saveStopwatch = new();

        public void Draw()
        {
            Utilities.Draw.Checkbox("Enable Debug Output", ref Service.Configuration.System.EnableDebugOutput);

            ImGui.SetNextItemWidth(100.0f * ImGuiHelpers.GlobalScale);
            if (ImGui.InputFloat("DisableIncrement", ref Service.Configuration.DisableIncrement, 0, 0))
            {
                saveStopwatch.Restart();
            }
            ImGuiComponents.HelpMarker("Controls how quickly framerate is restored to max");

            ImGui.SetNextItemWidth(100.0f * ImGuiHelpers.GlobalScale);
            if (ImGui.InputFloat("EnableIncrement", ref Service.Configuration.EnableIncrement, 0, 0))
            {
                saveStopwatch.Restart();
            }
            ImGuiComponents.HelpMarker("Controls how quickly framerate is limited down to configured framerate");

            if (saveStopwatch.ElapsedMilliseconds > 500)
            {
                FrameLimiter.DisableIncrement = Service.Configuration.DisableIncrement;
                FrameLimiter.EnableIncrement = Service.Configuration.EnableIncrement;

                Service.Configuration.Save();
                saveStopwatch.Reset();
            }

        }

        public void Dispose()
        {

        }
    }
}
