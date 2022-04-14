using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using ChillFrames.Data.SettingsObjects;
using ChillFrames.Interfaces;
using ChillFrames.System;
using ChillFrames.Tabs;
using ChillFrames.Utilities;
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
            //new PerformanceProfiles(),
            new DebugTab()
        };

        public SettingsWindow() : base("ChillFrames Settings")
        {
            Service.WindowSystem.AddWindow(this);

            Flags |= ImGuiWindowFlags.NoScrollbar;
            Flags |= ImGuiWindowFlags.NoScrollWithMouse;
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


            Utilities.Draw.Checkbox("Enable Framerate Limiter", ref Settings.EnableLimiter, "Enables the Framerate Limiter\n" + "When the configured conditions are true");

            ImGui.Indent(28.0f * ImGuiHelpers.GlobalScale);

            if (Condition.EnableFramerateLimit() && Settings.EnableLimiter)
            {
                ImGui.TextColored(Colors.Green, "Limiter Active");
            }
            else
            {
                ImGui.TextColored(Colors.Red, "Limiter Inactive");
            }

            ImGui.Indent(-25.0f * ImGuiHelpers.GlobalScale);

            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, ImGuiHelpers.ScaledVector2(10, 8));

            ImGui.Spacing();

            DrawTabs();

            ImGui.PopStyleVar();

            DrawVersionNumber();
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

        private void DrawVersionNumber()
        {
            var assemblyInformation = Assembly.GetExecutingAssembly().FullName!.Split(',');

            var versionString = assemblyInformation[1].Replace('=', ' ');

            var stringSize = ImGui.CalcTextSize(versionString);

            var x = ImGui.GetWindowWidth() / 2 - (stringSize.X / 2) * ImGuiHelpers.GlobalScale;
            var y = ImGui.GetWindowHeight() - 30 * ImGuiHelpers.GlobalScale;
            
            ImGui.SetCursorPos(new Vector2(x, y));

            ImGui.TextColored(Colors.Grey, versionString);
        }
    }
}