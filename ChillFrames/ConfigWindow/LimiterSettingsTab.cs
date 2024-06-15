using System;
using System.Drawing;
using ChillFrames.Controllers;
using ChillFrames.LimiterOptions;
using Dalamud.Interface;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using ImGuiNET;
using KamiLib.Components;

namespace ChillFrames.ConfigWindow;

public class LimiterSettingsTab : ITabItem {
    public string Name => "Limiter Settings";
    public bool Disabled => false;
    private string LowerLimitString => $"Use Lower Limit ( {ChillFramesSystem.Config.Limiter.IdleFramerateTarget} fps )";
    private string UpperLimitString => $"Use Upper Limit ( {ChillFramesSystem.Config.Limiter.ActiveFramerateTarget} fps )";

    public void Draw() {
        DrawFpsLimitOptions();
        ImGuiHelpers.ScaledDummy(5.0f);
        DrawLimiterOptions();
    }
    
    private static void DrawFpsLimitOptions() {
        using var fpsInputTable = ImRaii.Table("fps_input_settings", 2);
        if (!fpsInputTable) return;
        
        ImGui.TableNextColumn();
        ImGui.AlignTextToFramePadding();
        ImGui.Text("Lower Limit");
        ImGui.SameLine();
        ImGui.PushItemWidth(ImGui.GetContentRegionAvail().X * 0.75f);
        var idleLimit = ChillFramesSystem.Config.Limiter.IdleFramerateTarget;
        ImGui.InputInt("##LowerLimit", ref idleLimit, 0);
        if (ImGui.IsItemDeactivatedAfterEdit()) {
            ChillFramesSystem.Config.Limiter.IdleFramerateTarget = Math.Clamp(idleLimit, 1, ChillFramesSystem.Config.Limiter.ActiveFramerateTarget);
            ChillFramesSystem.Config.Save();
        }

        ImGui.TableNextColumn();
        ImGui.AlignTextToFramePadding();
        ImGui.Text("Upper Limit");
        ImGui.SameLine();
        ImGui.PushItemWidth(ImGui.GetContentRegionAvail().X * 0.75f);
        var activeLimit = ChillFramesSystem.Config.Limiter.ActiveFramerateTarget;
        ImGui.InputInt("##UpperLimit", ref activeLimit, 0);
        if (ImGui.IsItemDeactivatedAfterEdit()) {
            ChillFramesSystem.Config.Limiter.ActiveFramerateTarget = Math.Clamp(activeLimit, ChillFramesSystem.Config.Limiter.IdleFramerateTarget, 1000);
            ChillFramesSystem.Config.Save();
        }
    }

    private void DrawLimiterOptions() {
        using var table = ImRaii.Table("limiter_options_table", 3);
        if (!table) return;
        
        ImGui.TableSetupColumn("Condition", ImGuiTableColumnFlags.WidthFixed, 150.0f * ImGuiHelpers.GlobalScale);
        ImGui.TableSetupColumn("Status", ImGuiTableColumnFlags.WidthFixed, 75.0f * ImGuiHelpers.GlobalScale);
        ImGui.TableSetupColumn("When Condition Active", ImGuiTableColumnFlags.WidthStretch);
            
        ImGui.TableHeadersRow();
            
        foreach (var option in ChillFramesPlugin.System.LimiterOptions) {
            DrawOption(option);
        }
    }

    private void DrawOption(IFrameLimiterOption option) {
        ImGui.TableNextColumn();
        ImGui.Text(option.Label);

        ImGui.TableNextColumn();
        if (option.Active) {
            ImGui.TextColored(KnownColor.Green.Vector(), "Active");
        }
        else {
            ImGui.TextColored(KnownColor.OrangeRed.Vector(), "Inactive");
        }

        ImGui.TableNextColumn();
        ImGui.PushItemWidth(185.0f * ImGuiHelpers.GlobalScale);

        DrawOptionCombo(option);
    }
    
    private void DrawOptionCombo(IFrameLimiterOption option) {
        using var combo = ImRaii.Combo($"##OptionCombo_{option.Label}", option.Enabled ? UpperLimitString : LowerLimitString);
        if (!combo) return;
        
        if (ImGui.Selectable(UpperLimitString, option.Enabled)) {
            option.Enabled = true;
            ChillFramesSystem.Config.Save();
        }

        if (ImGui.Selectable(LowerLimitString, !option.Enabled)) {
            option.Enabled = false;
            ChillFramesSystem.Config.Save();
        }
    }
}