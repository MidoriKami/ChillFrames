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
        
        public void Draw()
        {
            Utilities.Draw.Checkbox("Enable Debug Output", ref Service.Configuration.System.EnableDebugOutput);

            ImGui.SetNextItemWidth(100.0f * ImGuiHelpers.GlobalScale);
            ImGui.InputFloat("DisableIncrement", ref Service.Configuration.DisableIncrement, 0, 0);
            ImGuiComponents.HelpMarker("Controls how quickly framerate is restored to max");
            if (ImGui.IsItemDeactivatedAfterEdit())
            {
                FrameLimiter.DisableIncrement = Service.Configuration.DisableIncrement;
                Service.Configuration.Save();
            }

            ImGui.SetNextItemWidth(100.0f * ImGuiHelpers.GlobalScale);
            ImGui.InputFloat("EnableIncrement", ref Service.Configuration.EnableIncrement, 0, 0);
            ImGuiComponents.HelpMarker("Controls how quickly framerate is limited down to configured framerate");
            if (ImGui.IsItemDeactivatedAfterEdit())
            {
                FrameLimiter.EnableIncrement = Service.Configuration.EnableIncrement;
                Service.Configuration.Save();
            }
        }

        public void Dispose()
        {

        }
    }
}
