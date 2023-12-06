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
                new BlacklistTab()
            }
        };

        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(450.0f, 435.0f),
            MaximumSize = new Vector2(9999, 9999)
        };

        Flags |= ImGuiWindowFlags.NoScrollbar;
        Flags |= ImGuiWindowFlags.NoScrollWithMouse;

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
        else if (!FrameLimiterCondition.DisableFramerateLimit() && config.PluginEnable)
        {
            ImGui.TextColored(KnownColor.Green.Vector(), $"Target Framerate: {config.Limiter.IdleFramerateTarget}");
        }
        else if (FrameLimiterCondition.DisableFramerateLimit() && config.PluginEnable)
        {
            ImGui.TextColored(KnownColor.Green.Vector(), $"Target Framerate: {config.Limiter.ActiveFramerateTarget}");
        }
        else
        {
            ImGui.TextColored(KnownColor.Red.Vector(), "Limiter Inactive");
        }
        
        ImGuiHelpers.ScaledDummy(5.0f);
    }

    [BaseCommandHandler("OpenConfigWindow")]
    public void OpenConfigWindow() => Toggle();
}