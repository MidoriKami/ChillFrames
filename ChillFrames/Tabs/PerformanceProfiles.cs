using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ChillFrames.Data.Enums.BattleEffect;
using ChillFrames.Data.SettingsObjects;
using ChillFrames.Data.SettingsObjects.Components;
using ChillFrames.Interfaces;
using Dalamud.Interface;
using Dalamud.Interface.Components;
using ImGuiNET;

namespace ChillFrames.Tabs
{
    internal class PerformanceProfiles : ITabItem
    {
        private PerformanceSettings Settings => Service.Configuration.Performance;
        public string TabName => "Performance";
        public bool Enabled => Service.Configuration.Performance.Enabled;
        
        public void Draw()
        {
            ImGui.PushID("InactiveProfile");
            ImGui.Text("Framerate Limiter Off");
            ImGuiComponents.HelpMarker("This profile will be used when the frame limiter is NOT ACTIVE\n" +
                                       "In other words, when your frame rate is NOT being limited,\n" +
                                       "these settings will be used.");

            DrawEditPerformanceProfile(Settings.InactiveProfile);
            ImGui.PopID();

            ImGui.PushID("ActiveProfile");
            ImGui.Text("Frame Limiter On");
            ImGuiComponents.HelpMarker("This profile will be used when the frame limiter is ACTIVE\n" +
                                       "In other words, when your frame rate is being limited,\n" +
                                       "these settings will be used.");

            DrawEditPerformanceProfile(Settings.ActiveProfile);
            ImGui.PopID();
        }

        public void Dispose()
        {

        }

        private void DrawEditPerformanceProfile(PerformanceProfile profile)
        {
            if (ImGui.BeginChild("PerformanceProfile", ImGuiHelpers.ScaledVector2(0, 160.0f), true))
            {
                ImGui.Spacing();
                ImGui.Text("Battle Effect Settings");
                ImGui.Spacing();
                ImGui.Spacing();

                ImGui.PushID("Self");
                ImGui.Text($"Self");
                ImGui.SameLine(100.0f *ImGuiHelpers.GlobalScale);
                DrawSettingsCombo(profile.Self);
                ImGui.PopID();

                ImGui.PushID("Party");
                ImGui.Text($"Party");
                ImGui.SameLine(100.0f *ImGuiHelpers.GlobalScale);
                DrawSettingsCombo(profile.Party);
                ImGui.PopID();

                ImGui.PushID("Other");
                ImGui.Text($"Other");
                ImGui.SameLine(100.0f *ImGuiHelpers.GlobalScale);
                DrawSettingsCombo(profile.Other);
                ImGui.PopID();

                ImGui.EndChild();
            }
        }

        private void DrawSettingsCombo(BattleEffectCommand command)
        {
            ImGui.SetNextItemWidth(100.0f * ImGuiHelpers.GlobalScale);
            if (ImGui.BeginCombo("##TerritorySelectByName", command.Setting.ToString()))
            {
                foreach (var setting in Enum.GetValues<Setting>())
                {
                    if (ImGui.Selectable(setting.ToString(), setting == command.Setting))
                    {
                        command.Setting = setting;
                        Service.Configuration.Save();
                    }

                    if (setting == command.Setting)
                    {
                        ImGui.SetItemDefaultFocus();
                    }
                }

                ImGui.EndCombo();
            }
        }
    }
}
