using System.Drawing;
using System.Numerics;
using ChillFrames.Controllers;
using ChillFrames.Models;
using Dalamud.Interface;
using Dalamud.Interface.Components;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using ImGuiNET;
using KamiLib.CommandManager;
using KamiLib.TabBar;
using Window = KamiLib.Window.Window;

namespace ChillFrames.Views.ConfigWindow;

public class SettingsWindow : Window {
    private int idleFramerateLimitTemp = int.MinValue;
    private int activeFramerateLimitTemp = int.MinValue;
    private Configuration Config => ChillFramesSystem.Config;

    private readonly TabBar tabBar = new("ChillFramesSettingsTabBar", [
        new LimiterSettingsTab(),
        new DtrSettingsTab(),
        new BlacklistTab(),
    ]);

    public SettingsWindow() : base("ChillFrames Settings", new Vector2(450.0f, 425.0f)) {
        Flags |= ImGuiWindowFlags.NoScrollbar;
        Flags |= ImGuiWindowFlags.NoScrollWithMouse;
        
        ChillFramesSystem.CommandManager.RegisterCommand(new CommandHandler {
            Delegate = OpenConfigWindow,
            ActivationPath = "/",
        });
    }

    public override void PreDraw() {
        if (idleFramerateLimitTemp is int.MinValue) idleFramerateLimitTemp = ChillFramesSystem.Config.Limiter.IdleFramerateTarget;
        if (activeFramerateLimitTemp is int.MinValue) activeFramerateLimitTemp = ChillFramesSystem.Config.Limiter.ActiveFramerateTarget;
    }

    protected override void DrawContents() {
        DrawLimiterStatus();
        tabBar.Draw();
    }
    
    private void DrawLimiterStatus() {
        using var statusTable = ImRaii.Table("status_table", 2);
        if (!statusTable) return;
        
        ImGui.TableNextColumn();
        ImGui.Text($"Current Framerate");

        ImGui.TableNextColumn();
        ImGui.Text($"{1000 / FrameLimiterController.LastFrametime.TotalMilliseconds:F} fps");

        ImGui.TableNextColumn();
        if (ChillFramesSystem.BlockList.Count > 0) {
            if (ImGuiComponents.IconButton("##ReleaseLocks", FontAwesomeIcon.Unlock)) {
                ChillFramesSystem.BlockList.Clear();
            }
            if (ImGui.IsItemHovered()) {
                ImGui.SetTooltip("Remove limiter lock");
            }

            ImGui.SameLine();
            ImGuiHelpers.SafeTextColoredWrapped(KnownColor.Red.Vector(), $"Limiter is inactive - requested by plugin(s): {string.Join(", ", ChillFramesSystem.BlockList)}");
            ImGui.TableNextColumn();
        }
        else if (FrameLimiterCondition.IsBlacklisted) {
            ImGui.TextColored(KnownColor.Red.Vector(), "Limiter Inactive, In Blacklisted Zone");
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

    private void OpenConfigWindow(params string[] args) => Toggle();
}