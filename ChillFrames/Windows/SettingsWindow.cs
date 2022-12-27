using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using ChillFrames.Config;
using ChillFrames.Interfaces;
using ChillFrames.System;
using ChillFrames.Tabs;
using Dalamud.Interface;
using Dalamud.Interface.Components;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using KamiLib.CommandSystem;
using KamiLib.Utilities;

namespace ChillFrames.Windows;

public class SettingsWindow : Window, IDisposable
{
    private static GeneralSettings Settings => Service.Configuration.General;

    private readonly List<ITabItem> tabs = new()
    {
        new GeneralConfigurationTab(),
        new BlacklistTab(),
        new DebugTab()
    };

    public SettingsWindow() : base("ChillFrames Settings")
    {
        KamiLib.KamiLib.CommandManager.AddCommand(new ConfigurationWindowCommands<SettingsWindow>());
        
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(350, 535),
            MaximumSize = new Vector2(9999,9999)
        };

        Flags |= ImGuiWindowFlags.NoScrollbar;
        Flags |= ImGuiWindowFlags.NoScrollWithMouse;
    }

    public void Dispose()
    {
        Service.Configuration.Save();

        foreach (var tab in tabs)
        {
            tab.Dispose();
        }

        Service.WindowSystem.RemoveWindow(this);
    }

    public override void Draw()
    {
        if (!IsOpen) return;

        if (ImGui.Checkbox("Enable Framerate Limiter", ref Settings.EnableLimiterSetting.Value))
        {
            Service.Configuration.Save();
        }
        ImGuiComponents.HelpMarker("Enables the Framerate Limiter\n" + "When the configured conditions are true");
        
        ImGui.Indent(25.0f * ImGuiHelpers.GlobalScale);

        if (!FrameLimiterCondition.DisableFramerateLimit() && Settings.EnableLimiterSetting.Value)
        {
            ImGui.TextColored(Colors.Green, "Limiter Active");
        }
        else
        {
            ImGui.TextColored(Colors.Red, "Limiter Inactive");
        }

        ImGui.Indent(-25.0f * ImGuiHelpers.GlobalScale);

        ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, ImGuiHelpers.ScaledVector2(10, 8));

        ImGui.Spacing();

        DrawTabs();

        ImGui.PopStyleVar();

        DrawVersionNumber();
    }

    private void DrawTabs()
    {
        if (ImGui.BeginTabBar("ChillFramesTabBar", ImGuiTabBarFlags.NoTooltip))
        {
            foreach (var tab in tabs)
            {
                if(tab.Enabled == false) continue;

                if (ImGui.BeginTabItem(tab.TabName))
                {
                    if (ImGui.BeginChild("ChillFramesSettings", ImGuiHelpers.ScaledVector2(0, -23), false, ImGuiWindowFlags.NoScrollbar)) 
                    {
                        ImGui.PushID(tab.TabName);

                        tab.Draw();

                        ImGui.PopID();

                        ImGui.EndChild();
                    }

                    ImGui.EndTabItem();
                }
            }
        }
    }

    public override void OnClose()
    {
        Service.Configuration.Save();
    }

    private void DrawVersionNumber()
    {
        var assemblyInformation = Assembly.GetExecutingAssembly().FullName!.Split(',');

        var versionString = assemblyInformation[1].Replace('=', ' ');

        var stringSize = ImGui.CalcTextSize(versionString);

        var x = ImGui.GetWindowWidth() / 2 - (stringSize.X / 2) * ImGuiHelpers.GlobalScale;
        var y = ImGui.GetWindowHeight() - 30 * ImGuiHelpers.GlobalScale;
            
        ImGui.SetCursorPos(new Vector2(x, y));

        ImGui.TextColored(Colors.Grey, versionString);
    }
}