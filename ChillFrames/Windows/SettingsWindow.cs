using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using ChillFrames.Config;
using ChillFrames.System;
using ChillFrames.Tabs;
using Dalamud.Interface;
using Dalamud.Interface.Components;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using KamiLib.CommandSystem;
using KamiLib.Interfaces;
using KamiLib.Utilities;

namespace ChillFrames.Windows;

public class SettingsWindow : Window, IDisposable
{
    private static GeneralSettings Settings => Service.Configuration.General;

    private readonly TabBar tabBar = new("ChillFramesSettingsTabBar", ImGuiHelpers.ScaledVector2(0.0f, -23.0f));

    public SettingsWindow() : base("ChillFrames Settings")
    {
        KamiLib.KamiLib.CommandManager.AddCommand(new ConfigurationWindowCommands<SettingsWindow>());
        
        tabBar.AddTab(new List<ITabItem>
        {
            new GeneralConfigurationTab(),
            new BlacklistTab(),
            new DebugTab(),
        });
        
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(350, 510),
            MaximumSize = new Vector2(9999,9999)
        };

        Flags |= ImGuiWindowFlags.NoScrollbar;
        Flags |= ImGuiWindowFlags.NoScrollWithMouse;
    }

    public void Dispose()
    {
        tabBar.Dispose();
    }

    public override void Draw()
    {
        if (!IsOpen) return;
        var indentSize = 25.0f * ImGuiHelpers.GlobalScale;

        DrawLimiterEnableDisable();
        
        ImGui.Indent(indentSize);
        DrawLimiterStatus();
        ImGui.Unindent(indentSize);
        
        ImGuiHelpers.ScaledDummy(5.0f);

        tabBar.Draw();

        DrawVersionNumber();
    }

    private static void DrawLimiterEnableDisable()
    {
        if (ImGui.Checkbox("Enable Framerate Limiter", ref Settings.EnableLimiterSetting.Value))
        {
            Service.Configuration.Save();
        }
        ImGuiComponents.HelpMarker("Enables the Framerate Limiter\n" + "When the configured conditions are true");

    }

    private static void DrawLimiterStatus()
    {
        if (!FrameLimiterCondition.DisableFramerateLimit() && Settings.EnableLimiterSetting.Value)
        {
            ImGui.TextColored(Colors.Green, "Limiter Active");
        }
        else
        {
            ImGui.TextColored(Colors.Red, "Limiter Inactive");
        }
    }

    public override void OnClose() => Service.Configuration.Save();

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