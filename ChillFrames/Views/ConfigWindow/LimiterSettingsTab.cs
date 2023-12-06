using System;
using System.Drawing;
using ChillFrames.Controllers;
using ChillFrames.Interfaces;
using Dalamud.Interface;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using ImGuiNET;
using KamiLib.Interfaces;

namespace ChillFrames.Views.ConfigWindow;

public class LimiterSettingsTab : ITabItem {
    public string TabName => "Limiter Settings";
    public bool Enabled => true;

    private string LowerLimitString => $"Use Lower Limit ( {ChillFramesSystem.Config.Limiter.IdleFramerateTarget} fps )";
    private string UpperLimitString => $"Use Upper Limit ( {ChillFramesSystem.Config.Limiter.ActiveFramerateTarget} fps )";

    public void Draw() {
        ImGui.AlignTextToFramePadding();
        ImGui.Text("Lower Limit");
        ImGui.SameLine();
        ImGui.PushItemWidth(ImGui.GetContentRegionAvail().X / 3.0f);
        var idleLimit = ChillFramesSystem.Config.Limiter.IdleFramerateTarget;
        ImGui.InputInt("##LowerLimit", ref idleLimit, 0);
        if (ImGui.IsItemDeactivatedAfterEdit()) {
            ChillFramesSystem.Config.Limiter.IdleFramerateTarget = Math.Clamp(idleLimit, 1,  ChillFramesSystem.Config.Limiter.ActiveFramerateTarget);
            ChillFramesSystem.Config.Save();
        }
        
        ImGui.AlignTextToFramePadding();
        ImGui.Text("Upper Limit");
        ImGui.SameLine();
        ImGui.PushItemWidth(ImGui.GetContentRegionAvail().X / 3.0f);
        var activeLimit = ChillFramesSystem.Config.Limiter.ActiveFramerateTarget;
        ImGui.InputInt("##UpperLimit", ref activeLimit, 0);
        if (ImGui.IsItemDeactivatedAfterEdit()) {
            ChillFramesSystem.Config.Limiter.ActiveFramerateTarget = Math.Clamp(activeLimit, ChillFramesSystem.Config.Limiter.IdleFramerateTarget, 1000);
            ChillFramesSystem.Config.Save();
        }
        
        ImGuiHelpers.ScaledDummy(10.0f);
        
        using var table = ImRaii.Table("limiter_options_table", 3);
        if (table) {
            ImGui.TableSetupColumn("Condition", ImGuiTableColumnFlags.WidthFixed, 150.0f * ImGuiHelpers.GlobalScale);
            ImGui.TableSetupColumn("Status", ImGuiTableColumnFlags.WidthFixed, 75.0f * ImGuiHelpers.GlobalScale);
            ImGui.TableSetupColumn("When Condition Active", ImGuiTableColumnFlags.WidthStretch);
            
            ImGui.TableHeadersRow();
            
            foreach (var option in ChillFramesPlugin.System.LimiterOptions) {
                DrawOption(option);
            }
        }
    }

    private void DrawOption(IFrameLimiterOption option) {
        ImGui.TableNextColumn();
        ImGui.Text(option.Label);

        ImGui.TableNextColumn();
        if (option.GetActive()) {
            ImGui.TextColored(KnownColor.Green.Vector(), "Active");
        }
        else {
            ImGui.TextColored(KnownColor.OrangeRed.Vector(), "Inactive");
        }

        ImGui.TableNextColumn();
        ImGui.PushItemWidth(185.0f * ImGuiHelpers.GlobalScale);
        if (ImGui.BeginCombo($"##OptionCombo_{option.Label}", option.IsEnabled ? UpperLimitString : LowerLimitString)) {
            if (ImGui.Selectable(UpperLimitString, option.IsEnabled)) {
                option.GetSetting() = true;
                ChillFramesSystem.Config.Save();
            }

            if (ImGui.Selectable(LowerLimitString, !option.IsEnabled)) {
                option.GetSetting() = false;
                ChillFramesSystem.Config.Save();
            }
            ImGui.EndCombo();
        }
    }
}