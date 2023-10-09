using System.Drawing;
using System.Numerics;
using ChillFrames.Controllers;
using Dalamud.Interface;
using Dalamud.Interface.Components;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using KamiLib.Command;
using KamiLib.Interfaces;
using KamiLib.System;
using KamiLib.UserInterface;

namespace ChillFrames.Views.ConfigWindow;

public class SettingsWindow : Window
{
    private int idleFramerateLimitTemp = int.MinValue;
    private int activeFramerateLimitTemp = int.MinValue;

    private readonly TabBar tabBar;

    public SettingsWindow() : base("ChillFrames Settings")
    {
        tabBar = new TabBar {
            TabItems = new ITabItem[] {
                new LimiterSettingsTab(),
                new ZoneFilterTab()
            }
        };

        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(425.0f, 435.0f),
            MaximumSize = new Vector2(9999, 9999)
        };

        Flags |= ImGuiWindowFlags.NoScrollbar;
        Flags |= ImGuiWindowFlags.NoScrollWithMouse;

        IsOpen = true;

        CommandController.RegisterCommands(this);
    }

    public override void PreDraw()
    {
        if (idleFramerateLimitTemp is int.MinValue) idleFramerateLimitTemp = ChillFramesSystem.Config.Limiter.IdleFramerateTarget;
        if (activeFramerateLimitTemp is int.MinValue) activeFramerateLimitTemp = ChillFramesSystem.Config.Limiter.ActiveFramerateTarget;
    }

    public override void Draw()
    {
        DrawLimiterStatus();
        tabBar.Draw();
        DrawSettingsModal();
    }
    
    private void DrawSettingsModal()
    {
        ImGui.SetNextWindowSize( new Vector2(350.0f, 250.0f));
        if (ImGui.BeginPopupContextWindow("ChillFrames Framerate Configuration"))
        {
            ImGui.Text("The idle limiter is used when no overrides are active");
            ImGui.Separator();
            var configChanged = ImGui.Checkbox("Enable Idle Limiter", ref ChillFramesSystem.Config.Limiter.EnableIdleFramerateLimit);

            ImGui.PushItemWidth(ImGui.GetContentRegionMax().X / 3.0f);
            ImGui.InputInt("Target Idle Framerate", ref idleFramerateLimitTemp, 0, 0);
            if (ImGui.IsItemDeactivatedAfterEdit())
            {
                ChillFramesSystem.Config.Limiter.IdleFramerateTarget = idleFramerateLimitTemp;
                configChanged = true;
            }
            
            ImGuiHelpers.ScaledDummy(25.0f);
            ImGui.Text("The active limiter is used when overrides are active");
            ImGui.Separator();
            
            configChanged |= ImGui.Checkbox("Enable Active Limiter", ref ChillFramesSystem.Config.Limiter.EnableActiveFramerateLimit);

            ImGui.PushItemWidth(ImGui.GetContentRegionMax().X / 3.0f);
            ImGui.InputInt("Target Active Framerate", ref activeFramerateLimitTemp, 0, 0);
            if (ImGui.IsItemDeactivatedAfterEdit())
            {
                ChillFramesSystem.Config.Limiter.ActiveFramerateTarget = activeFramerateLimitTemp;
                configChanged = true;
            }
            
            if (configChanged) ChillFramesSystem.Config.Save();
            
            DrawModalCloseButtons();
            ImGui.EndPopup();
        }
    }
    
    private void DrawLimiterStatus()
    {
        var config = ChillFramesSystem.Config;
        
        if (ChillFramesSystem.BlockList.Count > 0)
        {
            if (ImGuiComponents.IconButton("##ReleaseLocks", FontAwesomeIcon.Unlock))
            {
                ChillFramesSystem.BlockList.Clear();
            }
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip("Remove limiter lock");
            }

            ImGui.SameLine();
            ImGuiHelpers.SafeTextColoredWrapped(KnownColor.Red.Vector(), $"Limiter is inactive - requested by plugin(s): {string.Join(", ", ChillFramesSystem.BlockList)}");
        }
        else if (FrameLimiterCondition.IsBlacklisted)
        {
            ImGui.TextColored(KnownColor.Red.Vector(), "Limiter Inactive, In Blacklisted Zone");
        }
        else if (!FrameLimiterCondition.DisableFramerateLimit() && config.Limiter.EnableIdleFramerateLimit && config.PluginEnable)
        {
            ImGui.TextColored(KnownColor.Green.Vector(), $"Limiter Active. Target Framerate: {config.Limiter.IdleFramerateTarget}");
        }
        else if (FrameLimiterCondition.DisableFramerateLimit() && config.Limiter.EnableActiveFramerateLimit && config.PluginEnable)
        {
            ImGui.TextColored(KnownColor.Green.Vector(), $"Limiter Active. Target Framerate: {config.Limiter.ActiveFramerateTarget}");
        }
        else
        {
            ImGui.TextColored(KnownColor.Red.Vector(), "Limiter Inactive");
        }
        
        ImGui.SameLine(ImGui.GetContentRegionMax().X - 23.0f * ImGuiHelpers.GlobalScale - ImGui.GetStyle().FramePadding.X);
        if (ImGuiComponents.IconButton("SettingsButton", FontAwesomeIcon.Cog))
        {
            ImGui.OpenPopup("ChillFrames Framerate Configuration");
        }
    }
        
    private void DrawModalCloseButtons()
    {
        ImGui.SetCursorPosY(ImGui.GetContentRegionMax().Y - 23.0f * ImGuiHelpers.GlobalScale);
        if (ImGui.Button("Save", ImGuiHelpers.ScaledVector2(100.0f, 23.0f)))
        {
            ChillFramesSystem.Config.Save();
        }

        ImGui.SameLine(ImGui.GetContentRegionMax().X - 100.0f * ImGuiHelpers.GlobalScale);
        if (ImGui.Button("Save & Close", ImGuiHelpers.ScaledVector2(100.0f, 23.0f)))
        {
            ChillFramesSystem.Config.Save();
            ImGui.CloseCurrentPopup();
        }
    }

    [BaseCommandHandler("OpenConfigWindow")]
    public void OpenConfigWindow()
    {
        if (!Service.ClientState.IsLoggedIn) return;
        if (Service.ClientState.IsPvP) return;

        Toggle();
    }
}