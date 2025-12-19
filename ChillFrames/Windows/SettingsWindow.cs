using System;
using System.Drawing;
using System.Numerics;
using ChillFrames.Classes;
using ChillFrames.Controllers;
using Dalamud.Bindings.ImGui;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Interface;
using Dalamud.Interface.Components;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;

namespace ChillFrames.Windows;

public class SettingsWindow : Window {
	private int idleFramerateLimitTemp = int.MinValue;
	private int activeFramerateLimitTemp = int.MinValue;
	private static Configuration Config => System.Config;

	public SettingsWindow() : base("ChillFrames Settings") {
		SizeConstraints = new WindowSizeConstraints {
			MinimumSize = new Vector2(500.0f, 500.0f),
		};

		Flags |= ImGuiWindowFlags.NoScrollbar;
		Flags |= ImGuiWindowFlags.NoScrollWithMouse;
	}

	public override void PreDraw() {
		if (idleFramerateLimitTemp is int.MinValue) {
			idleFramerateLimitTemp = System.Config.Limiter.IdleFramerateTarget;
		}

		if (activeFramerateLimitTemp is int.MinValue) {
			activeFramerateLimitTemp = System.Config.Limiter.ActiveFramerateTarget;
		}
	}

	public override void Draw() {
		using var uiLockout = ImRaii.Disabled(Services.Condition.Any(ConditionFlag.InCombat));
		DrawLimiterStatus();

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
	}

	private void DrawLimiterStatus() {
		using var statusTable = ImRaii.Table("status_table", 2);
		if (!statusTable) return;
        
		ImGui.TableNextColumn();
		ImGui.Text($"Current Framerate");

		ImGui.TableNextColumn();
		ImGui.Text($"{1000 / FrameLimiterController.LastFrametime.TotalMilliseconds:F} fps");

		ImGui.TableNextColumn();
		if (System.BlockList.Count > 0) {
			if (ImGuiComponents.IconButton("##ReleaseLocks", FontAwesomeIcon.Unlock)) {
				System.BlockList.Clear();
			}
			if (ImGui.IsItemHovered()) {
				ImGui.SetTooltip("Remove limiter lock");
			}

			ImGui.SameLine();
			ImGui.TextColoredWrapped(KnownColor.Red.Vector(), $"Limiter is inactive - requested by plugin(s): {string.Join(", ", System.BlockList)}");
			ImGui.TableNextColumn();
		}
		else if (!FrameLimiterCondition.DisableFramerateLimit() && Config.PluginEnable) {
			ImGui.Text($"Target Framerate");
			ImGui.TableNextColumn();
			ImGui.Text($"{Config.Limiter.IdleFramerateTarget} fps");
		}
		else if (FrameLimiterCondition.DisableFramerateLimit() && Config.PluginEnable) {
			ImGui.Text($"Target Framerate");
			ImGui.TableNextColumn();
			ImGui.Text($"{Config.Limiter.ActiveFramerateTarget} fps");
		}
		else {
			ImGui.TextColored(KnownColor.Red.Vector(), "Limiter Inactive");
			ImGui.TableNextColumn();
		}
	}

	private void DrawSettings() {
		ImGuiHelpers.ScaledDummy(5.0f);
		DrawFpsLimitOptions();

		ImGuiHelpers.ScaledDummy(5.0f);
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
		using var fpsInputTable = ImRaii.Table("fps_input_settings", 2);
		if (!fpsInputTable) return;
        
		ImGui.TableNextColumn();
		ImGui.AlignTextToFramePadding();
		ImGui.Text("Lower Limit");
		ImGui.SameLine();
		ImGui.PushItemWidth(ImGui.GetContentRegionAvail().X * 0.75f);
		var idleLimit = System.Config.Limiter.IdleFramerateTarget;
		ImGui.InputInt("##LowerLimit", ref idleLimit);
		if (ImGui.IsItemDeactivatedAfterEdit()) {
			System.Config.Limiter.IdleFramerateTarget = Math.Clamp(idleLimit, 1, System.Config.Limiter.ActiveFramerateTarget);
			System.Config.Save();
		}

		ImGui.TableNextColumn();
		ImGui.AlignTextToFramePadding();
		ImGui.Text("Upper Limit");
		ImGui.SameLine();
		ImGui.PushItemWidth(ImGui.GetContentRegionAvail().X * 0.75f);
		var activeLimit = System.Config.Limiter.ActiveFramerateTarget;
		ImGui.InputInt("##UpperLimit", ref activeLimit);
		if (ImGui.IsItemDeactivatedAfterEdit()) {
			System.Config.Limiter.ActiveFramerateTarget = Math.Clamp(activeLimit, System.Config.Limiter.IdleFramerateTarget, 1000);
			System.Config.Save();
		}
	}

	private void DrawLimiterOptions() {
		using var table = ImRaii.Table("limiter_options_table", 3);
		if (!table) return;
        
		ImGui.TableSetupColumn("Condition", ImGuiTableColumnFlags.WidthFixed, 150.0f * ImGuiHelpers.GlobalScale);
		ImGui.TableSetupColumn("Status", ImGuiTableColumnFlags.WidthFixed, 75.0f * ImGuiHelpers.GlobalScale);
		ImGui.TableSetupColumn("When Condition Active", ImGuiTableColumnFlags.WidthStretch);
            
		ImGui.TableHeadersRow();
            
		foreach (var option in System.LimiterOptions) {
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
		ImGui.PushItemWidth(ImGui.GetContentRegionAvail().X);

		DrawOptionCombo(option);
	}
    
	private string LowerLimitString => $"Use Lower Limit ( {System.Config.Limiter.IdleFramerateTarget} fps )";
	private string UpperLimitString => $"Use Upper Limit ( {System.Config.Limiter.ActiveFramerateTarget} fps )";
	
	private void DrawOptionCombo(IFrameLimiterOption option) {
		using var combo = ImRaii.Combo($"##OptionCombo_{option.Label}", option.Enabled ? UpperLimitString : LowerLimitString);
		if (!combo) return;
        
		if (ImGui.Selectable(UpperLimitString, option.Enabled)) {
			option.Enabled = true;
			System.Config.Save();
		}

		if (ImGui.Selectable(LowerLimitString, !option.Enabled)) {
			option.Enabled = false;
			System.Config.Save();
		}
	}
	
	private static void DrawFeatureToggles() {
		using var pushIndent = ImRaii.PushIndent();

		if (ImGui.Checkbox("Enable", ref System.Config.General.EnableDtrBar)) {
			System.DtrController.UpdateEnabled();
			System.Config.Save();
		}

		if (ImGui.Checkbox("Show Color", ref System.Config.General.EnableDtrColor)) {
			System.Config.Save();
		}
	}
    
	private static void DrawColorOptions() {
		using var pushIndent = ImRaii.PushIndent();
		if (ImGui.ColorEdit4("Enabled Color", ref System.Config.General.ActiveColor)) {
			System.Config.Save();
		}

		if (ImGui.ColorEdit4("Disabled Color", ref System.Config.General.InactiveColor)) {
			System.Config.Save();
		}
	}
}