using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using ChillFrames.System;
using ChillFrames.Windows.Tabs;
using Dalamud.Interface;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using KamiLib.Commands;
using KamiLib.Interfaces;
using KamiLib.Utilities;

namespace ChillFrames.Windows;

public class SettingsWindow : Window
{
    private readonly IEnumerable<ITabItem> tabs;

    public SettingsWindow() : base("ChillFrames Settings")
    {
        tabs = new ITabItem[]
        {
            new LimiterSettingsTab(),
            new GeneralSettingsTab(),
            new ZoneFilterTab(),
        };
        
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(400, 475),
            MaximumSize = new Vector2(9999,9999)
        };

        Flags |= ImGuiWindowFlags.NoScrollbar;
        Flags |= ImGuiWindowFlags.NoScrollWithMouse;
        
        CommandController.RegisterCommands(this);
    }
    
    public override void Draw()
    {
        if (ImGui.BeginChild("##MainToggleAndStatus", ImGuiHelpers.ScaledVector2(0.0f, ChillFramesPlugin.System.BlockList.Count>0?80f:60.0f)))
        {
            var config = ChillFramesSystem.Config;
            
            var value = config.PluginEnable;
            if (ImGui.Checkbox("Enable Framerate Limiter", ref value))
            {
                config.PluginEnable = value;
                config.Save();
            }

            if(ChillFramesPlugin.System.BlockList.Count > 0)
            {
                ImGuiHelpers.SafeTextColoredWrapped(KnownColor.Red.AsVector4(), $"Limiter is inactive - requested by plugin: {string.Join(",", ChillFramesPlugin.System.BlockList)}");
                if(ImGui.SmallButton("Release all locks"))
                {
                    ChillFramesPlugin.System.BlockList.Clear();
                }
            }
            else if (FrameLimiterCondition.IsBlacklisted)
            {
                ImGui.TextColored(KnownColor.Red.AsVector4(), "Limiter Inactive, In Blacklisted Zone");
            }
            else if (!FrameLimiterCondition.DisableFramerateLimit() && config.Limiter.EnableIdleFramerateLimit && config.PluginEnable)
            {
                ImGui.TextColored(KnownColor.Green.AsVector4(), $"Limiter Active. Target Framerate: {config.Limiter.IdleFramerateTarget}");
            }
            else if (FrameLimiterCondition.DisableFramerateLimit() && config.Limiter.EnableActiveFramerateLimit && config.PluginEnable)
            {
                ImGui.TextColored(KnownColor.Green.AsVector4(), $"Limiter Active. Target Framerate: {config.Limiter.ActiveFramerateTarget}");
            }
            else
            {
                ImGui.TextColored(KnownColor.Red.AsVector4(), "Limiter Inactive");
            }
        }
        ImGui.EndChild();

        var region = ImGui.GetContentRegionAvail();
        
        if (ImGui.BeginTabBar("TabBar"))
        {
            foreach (var tab in tabs)
            {
                if (ImGui.BeginTabItem(tab.TabName))
                {
                    if (ImGui.BeginChild("TabChild", new Vector2(0.0f, region.Y - 50.0f), false, ImGuiWindowFlags.AlwaysVerticalScrollbar))
                    {
                        tab.Draw();
                    }
                    ImGui.EndChild();

                    ImGui.EndTabItem();
                }
            }

            ImGui.EndTabBar();
        }

        PluginVersion.Instance.DrawVersionText();
    }
    
    [BaseCommandHandler("OpenConfigWindow")]
    public void OpenConfigWindow()
    {
        if (!Service.ClientState.IsLoggedIn) return;
        if (Service.ClientState.IsPvP) return;
            
        Toggle();
    }
}