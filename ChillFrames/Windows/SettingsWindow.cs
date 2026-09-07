using System;
using System.Drawing;
using System.Linq;
using System.Numerics;
using ChillFrames.Classes;
using ChillFrames.Controllers;
using Dalamud.Bindings.ImGui;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Interface;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;

namespace ChillFrames.Windows;

public class SettingsWindow : Window {
    private static Configuration Config => System.Config;

    public SettingsWindow() : base("ChillFrames Settings") {
       SizeConstraints = new WindowSizeConstraints {
          MinimumSize = new Vector2(500.0f, 625.0f),
          MaximumSize = new Vector2(500.0f, 625.0f),
       };

       Flags |= ImGuiWindowFlags.NoScrollbar;
       Flags |= ImGuiWindowFlags.NoScrollWithMouse;
       Flags |= ImGuiWindowFlags.NoResize;
    }

    public override void Draw() {
       using var uiLockout = ImRaii.Disabled(ICondition.Get().Any(ConditionFlag.InCombat));

       DrawLimiterStatus();
       ImGuiHelpers.ScaledDummy(5.0f);

       using var tabBar = ImRaii.TabBar("ChillFramesSettingsTabBar");
       if (!tabBar) return;

       using (var settingsTab = ImRaii.TabItem("Limiter Settings")) {
          if (settingsTab) {
             DrawSettings();
          }
       }

       using (var dtrSettings = ImRaii.TabItem("DTR Entry")) {
          if (dtrSettings) {
             DrawDtrSettings();
          }
       }

       using (var idleFpsSettings = ImRaii.TabItem("Idle Framerate")) {
          if (idleFpsSettings) {
             DrawIdleFpsSettings();
          }
       }
    }

    private void DrawLimiterStatus() {
       using var statusTable = ImRaii.Table("status_table", 2);
       if (!statusTable) return;

       ImGui.TableNextColumn();
       ImGui.Text("Current Framerate");

       ImGui.TableNextColumn();
       ImGui.TextDisabled($"{1000 / FrameLimiterController.LastFrametime.TotalMilliseconds:F1} fps");

       ImGui.TableNextColumn();
       ImGui.Text("Target Framerate");

       ImGui.TableNextColumn();
       if (Config.PluginEnable) {
          var targetFps = FrameLimiterCondition.GetTargetState() switch {
             LimiterStateTarget.LowerLimit => Config.Limiter.LowerFramerateTarget,
             LimiterStateTarget.BaseLimit => Config.Limiter.BaseFramerateTarget,
             LimiterStateTarget.UpperLimit => Config.Limiter.UpperFramerateTarget,
             _ => Config.Limiter.BaseFramerateTarget,
          };
          ImGui.Text($"{targetFps} fps");
       }
       else {
          ImGui.TextColored(KnownColor.OrangeRed.Vector(), "Limiter Inactive");
       }
    }

    private void DrawSettings() {
       ImGuiHelpers.ScaledDummy(5.0f);
       DrawFpsLimitOptions();

       ImGuiHelpers.ScaledDummy(10.0f);
       DrawLimiterOptions();
    }

    private void DrawDtrSettings() {
       ImGuiHelpers.ScaledDummy(10.0f);

       ImGui.Text("Feature Toggles");
       ImGui.Separator();
       ImGuiHelpers.ScaledDummy(5.0f);
       DrawFeatureToggles();

       ImGuiHelpers.ScaledDummy(10.0f);

       ImGui.Text("Color Options");
       ImGui.Separator();
       ImGuiHelpers.ScaledDummy(5.0f);
       DrawColorOptions();
    }

    private static void DrawFpsLimitOptions() {
       using var fpsInputTable = ImRaii.Table("fps_input_settings", 3, ImGuiTableFlags.SizingStretchSame);
       if (!fpsInputTable) return;

       ImGui.TableNextRow();

       // Lower Limit
       ImGui.TableNextColumn();
       ImGui.AlignTextToFramePadding();
       ImGui.Text("Lower Limit");
       ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X * 0.85f);
       var lowerLimit = System.Config.Limiter.LowerFramerateTarget;
       if (ImGui.InputInt("##LowerLimit", ref lowerLimit)) {
          System.Config.Limiter.LowerFramerateTarget = Math.Clamp(lowerLimit, 1, System.Config.Limiter.BaseFramerateTarget);
          System.Config.Save();
       }

       // Base Limit
       ImGui.TableNextColumn();
       ImGui.AlignTextToFramePadding();
       ImGui.Text("Base Limit");
       ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X * 0.85f);
       var baseLimit = System.Config.Limiter.BaseFramerateTarget;
       if (ImGui.InputInt("##BaseLimit", ref baseLimit)) {
          System.Config.Limiter.BaseFramerateTarget = Math.Clamp(baseLimit, System.Config.Limiter.LowerFramerateTarget, System.Config.Limiter.UpperFramerateTarget);
          System.Config.Save();
       }

       // Upper Limit
       ImGui.TableNextColumn();
       ImGui.AlignTextToFramePadding();
       ImGui.Text("Upper Limit");
       ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X * 0.85f);
       var upperLimit = System.Config.Limiter.UpperFramerateTarget;
       if (ImGui.InputInt("##UpperLimit", ref upperLimit)) {
          System.Config.Limiter.UpperFramerateTarget = Math.Clamp(upperLimit, System.Config.Limiter.BaseFramerateTarget, 1000);
          System.Config.Save();
       }
    }

    private void DrawLimiterOptions() {
       ImGui.TextColored(KnownColor.White.Vector(), "Limiter Conditions");
       ImGui.Separator();
       ImGuiHelpers.ScaledDummy(5.0f);

       var cellPaddingAmount = ImGui.GetStyle().CellPadding + ImGuiHelpers.ScaledVector2(0.0f, 4.0f);
       using var padding = ImRaii.PushStyle(ImGuiStyleVar.CellPadding, cellPaddingAmount);
       using var table = ImRaii.Table("limiter_options_table", 3, ImGuiTableFlags.RowBg);
       if (!table) return;

       ImGui.TableSetupColumn("Condition", ImGuiTableColumnFlags.WidthFixed, 150.0f * ImGuiHelpers.GlobalScale);
       ImGui.TableSetupColumn("Status", ImGuiTableColumnFlags.WidthFixed, 70.0f * ImGuiHelpers.GlobalScale);
       ImGui.TableSetupColumn("Target When Active", ImGuiTableColumnFlags.WidthStretch);
       ImGui.TableHeadersRow();

       foreach (var option in System.LimiterOptions.OrderBy(option => option.Label)) {
          DrawOption(option);
       }
    }

    private void DrawOption(IFrameLimiterOption option) {
       ImGui.TableNextRow();
       ImGui.TableNextColumn();
       ImGui.AlignTextToFramePadding();
       ImGui.Text(option.Label);

       ImGui.TableNextColumn();
       ImGui.AlignTextToFramePadding();
       if (option.Active) {
          ImGui.TextColored(KnownColor.LimeGreen.Vector(), "Active");
       }
       else {
          ImGui.TextColored(KnownColor.DarkGray.Vector(), "Inactive");
       }

       ImGui.TableNextColumn();
       ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X);
       DrawOptionCombo(option);
    }

    private string LowerLimitString => $"Use Lower Limit ({System.Config.Limiter.LowerFramerateTarget} fps)";
    private string BaseLimitString => $"Use Base Limit ({System.Config.Limiter.BaseFramerateTarget} fps)";
    private string UpperLimitString => $"Use Upper Limit ({System.Config.Limiter.UpperFramerateTarget} fps)";

    private string TargetString(LimiterStateTarget target) => target switch {
       LimiterStateTarget.LowerLimit => LowerLimitString,
       LimiterStateTarget.BaseLimit => BaseLimitString,
       LimiterStateTarget.UpperLimit => UpperLimitString,
       _ => BaseLimitString,
    };

    private void DrawOptionCombo(IFrameLimiterOption option) {
       using var combo = ImRaii.Combo($"##OptionCombo_{option.Label}", TargetString(option.Target));
       if (!combo) return;

       if (ImGui.Selectable(UpperLimitString, option.Target == LimiterStateTarget.UpperLimit)) {
          option.Target = LimiterStateTarget.UpperLimit;
          System.Config.Save();
       }

       if (ImGui.Selectable(BaseLimitString, option.Target == LimiterStateTarget.BaseLimit)) {
          option.Target = LimiterStateTarget.BaseLimit;
          System.Config.Save();
       }

       if (ImGui.Selectable(LowerLimitString, option.Target == LimiterStateTarget.LowerLimit)) {
          option.Target = LimiterStateTarget.LowerLimit;
          System.Config.Save();
       }
    }

    private static void DrawFeatureToggles() {
       using var pushIndent = ImRaii.PushIndent(10.0f);

       if (ImGui.Checkbox("Enable DTR Bar Integration", ref System.Config.General.EnableDtrBar)) {
          System.DtrController.UpdateEnabled();
          System.Config.Save();
       }

       if (ImGui.Checkbox("Show Color in DTR Bar", ref System.Config.General.EnableDtrColor)) {
          System.Config.Save();
       }
    }

    private static void DrawColorOptions() {
       using var pushIndent = ImRaii.PushIndent(10.0f);

       if (ImGui.ColorEdit4("Enabled Color", ref System.Config.General.ActiveColor, ImGuiColorEditFlags.NoInputs | ImGuiColorEditFlags.AlphaPreview)) {
          System.Config.Save();
       }

       if (ImGui.ColorEdit4("Disabled Color", ref System.Config.General.InactiveColor, ImGuiColorEditFlags.NoInputs | ImGuiColorEditFlags.AlphaPreview)) {
          System.Config.Save();
       }
    }

    private void DrawIdleFpsSettings() {
       ImGuiHelpers.ScaledDummy(10.0f);

       ImGui.Text("Idle FPS Settings");
       ImGui.Separator();
       ImGuiHelpers.ScaledDummy(5.0f);
       ImGui.TextWrapped("Sets the target FPS for the game's native idle limiters.");
       ImGui.TextDisabled("Requires the 'Inactive' or 'AFK' framerate limits enabled in System Config.");

       ImGuiHelpers.ScaledDummy(10.0f);

       using (var table = ImRaii.Table("idle_fps_target", 2, ImGuiTableFlags.None)) {
          if (table) {
             ImGui.TableSetupColumn("Labels", ImGuiTableColumnFlags.WidthFixed, 220.0f * ImGuiHelpers.GlobalScale);
             ImGui.TableSetupColumn("Inputs", ImGuiTableColumnFlags.WidthStretch);

             ImGui.TableNextRow();
             ImGui.TableNextColumn();
             ImGui.AlignTextToFramePadding();
             ImGui.Text("Idle FPS Target");

             ImGui.TableNextColumn();
             ImGui.SetNextItemWidth(100.0f * ImGuiHelpers.GlobalScale);
             ImGui.InputInt("##WaitTime", ref Config.IdleFpsTarget);
             Config.IdleFpsTarget = Math.Max(Config.IdleFpsTarget, 2);

             if (ImGui.IsItemDeactivatedAfterEdit()) {
                System.IdleFpsController.UpdateWaitTime();
                Config.Save();
             }
          }
       }

       ImGuiHelpers.ScaledDummy(15.0f);

       ImGui.TextWrapped("Delay in seconds before the idle limiter activates.");
       ImGui.TextDisabled("Smooths out quick alt-tabs.");

       ImGuiHelpers.ScaledDummy(10.0f);

       using (var table = ImRaii.Table("idle_fps_delay", 2, ImGuiTableFlags.None)) {
          if (table) {
             ImGui.TableSetupColumn("Labels", ImGuiTableColumnFlags.WidthFixed, 220.0f * ImGuiHelpers.GlobalScale);
             ImGui.TableSetupColumn("Inputs", ImGuiTableColumnFlags.WidthStretch);

             ImGui.TableNextRow();
             ImGui.TableNextColumn();
             ImGui.AlignTextToFramePadding();
             ImGui.Text("Activation Delay (sec)");

             ImGui.TableNextColumn();
             ImGui.SetNextItemWidth(100.0f * ImGuiHelpers.GlobalScale);
             ImGui.InputInt("##DelayTime", ref Config.IdleFpsDelayTime);
             Config.IdleFpsDelayTime = Math.Clamp(Config.IdleFpsDelayTime, 0, 600);

             if (ImGui.IsItemDeactivatedAfterEdit()) {
                Config.Save();
             }
          }
       }
    }
}