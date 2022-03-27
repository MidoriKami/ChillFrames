using System;
using System.Diagnostics;
using Dalamud.Interface;
using Dalamud.Interface.Components;
using Dalamud.Interface.Internal.Notifications;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace ChillFrames
{
    internal class SettingsWindow : Window, IDisposable
    {
        private const ImGuiWindowFlags DefaultFlags = ImGuiWindowFlags.NoFocusOnAppearing |
                                                      ImGuiWindowFlags.NoScrollbar |
                                                      ImGuiWindowFlags.NoCollapse |
                                                      ImGuiWindowFlags.AlwaysAutoResize;

        private int newFramerateLimit = Service.Configuration.FrameRateLimit;

        private readonly TerritoryBlacklistWindow blacklist = new();

        private readonly Stopwatch timer = new();

        public SettingsWindow() : base("ChillFrames Settings", DefaultFlags)
        {
            Service.WindowSystem.AddWindow(this);
        }
        public void Dispose()
        {
            Service.WindowSystem.RemoveWindow(this);
        }

        public override void Draw()
        {
            ImGui.PushID("SettingsWindow");

            ImGui.Text("This plugin will limit your games framerate\n" +
                       "when you are not in combat or in a duty");

            ImGui.Spacing();

            ImGui.Checkbox("Enable", ref Service.Configuration.EnableLimiter);
            
            ImGui.Spacing();

            if (Service.Configuration.EnableLimiter)
            {
                ImGui.Indent(15 * ImGuiHelpers.GlobalScale);

                DrawSettings();

                ImGui.Spacing();

                if (Service.Configuration.DisableInBlacklistedTerritories)
                {
                    ImGui.Indent(15 * ImGuiHelpers.GlobalScale);

                    if (ImGui.Button("Open Configuration"))
                    {
                        blacklist.IsOpen = true;
                    }

                    ImGui.Indent(-15 * ImGuiHelpers.GlobalScale);
                }

                DrawFramerateEdit();

                ImGui.Indent(-15 * ImGuiHelpers.GlobalScale);
            }

            ImGui.PopID();
        }

        public override void OnClose()
        {
            Service.PluginInterface.UiBuilder.AddNotification("Configuration Saved", "Chill Frames", NotificationType.Success);
            Service.Configuration.Save();
        }

        private void DrawFramerateEdit()
        {
            ImGui.SetNextItemWidth(50 * ImGuiHelpers.GlobalScale);

            if (ImGui.InputInt("Framerate Limit", ref newFramerateLimit, 0, 0))
            {
                timer.Restart();
            }

            if (timer.IsRunning && timer.Elapsed.Seconds >= 1)
            {
                timer.Stop();
                Service.Configuration.FrameRateLimit = Math.Max(newFramerateLimit, 10);
            }
            
            ImGuiComponents.HelpMarker("The framerate value to limit the game to\n" + "Minimum: 10");
            
            var frametimeExact = 1000 / Service.Configuration.FrameRateLimit + 1;
            var approximateFramerate = 1000 / frametimeExact;

            ImGui.Text("Approximated Framerate: " + approximateFramerate);
            ImGuiComponents.HelpMarker("Framerate limit will be approximated not exact");
        }

        private static void DrawSettings()
        {
            ImGui.Checkbox("Disable during combat", ref Service.Configuration.DisableDuringCombat);
            ImGui.Checkbox("Disable during duty", ref Service.Configuration.DisableDuringDuty);
            ImGui.Checkbox("Disable during cutscene", ref Service.Configuration.DisableDuringCutscene);
            ImGui.Checkbox("Disable in specific zones", ref Service.Configuration.DisableInBlacklistedTerritories);
        }
    }
}
