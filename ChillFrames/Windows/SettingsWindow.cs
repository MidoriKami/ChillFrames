using System;
using System.Collections.Generic;
using System.Numerics;
using ChillFrames.Data.SettingsObjects;
using ChillFrames.Interfaces;
using ChillFrames.Tabs;
using Dalamud.Interface;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace ChillFrames.Windows
{
    public class SettingsWindow : Window, IDisposable
    {
        private GeneralSettings Settings => Service.Configuration.General;

        private readonly List<ITabItem> tabs = new()
        {
            new GeneralConfigurationTab(),
            new BlacklistTab(),
            new PerformanceProfiles(),
            new DebugTab()
        };

        public SettingsWindow() : base("ChillFrames Settings")
        {
            Service.WindowSystem.AddWindow(this);

            Flags |= ImGuiWindowFlags.NoScrollbar;
            Flags |= ImGuiWindowFlags.NoScrollWithMouse;

            // todo: remove this
            IsOpen = true;
        }

        public void Dispose()
        {
            Service.Configuration.Save();

            foreach (var tab in tabs)
            {
                tab.Dispose();
            }

            Service.WindowSystem.RemoveWindow(this);
        }

        public override void Draw()
        {
            if (!IsOpen) return;

            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, ImGuiHelpers.ScaledVector2(10, 8));

            Utilities.Draw.Checkbox("Enable Framerate Limiter", ref Settings.EnableLimiter, "Enables the Framerate Limiter\n" + "When the configured conditions are true");

            ImGui.Spacing();

            DrawTabs();

            ImGui.PopStyleVar();
        }

        private void DrawTabs()
        {
            if (ImGui.BeginTabBar("ChillFramesTabBar", ImGuiTabBarFlags.NoTooltip))
            {
                foreach (var tab in tabs)
                {
                    if(tab.Enabled == false) continue;

                    if (ImGui.BeginTabItem(tab.TabName))
                    {
                        if (ImGui.BeginChild("ChillFramesSettings", new Vector2(0, 0), true)) 
                        {
                            ImGui.PushID(tab.TabName);

                            tab.Draw();

                            ImGui.PopID();

                            ImGui.EndChild();
                        }

                        ImGui.EndTabItem();
                    }
                }
            }
        }

        public override void OnClose()
        {
            Service.Configuration.Save();
        }
    }
}