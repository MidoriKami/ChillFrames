using System;
using System.Diagnostics;
using ChillFrames.Data.SettingsObjects;
using ChillFrames.Interfaces;
using Dalamud.Interface;
using Dalamud.Interface.Components;
using ImGuiNET;

namespace ChillFrames.Tabs;

internal class GeneralConfigurationTab : ITabItem
{
    private GeneralSettings Settings => Service.Configuration.General;
    private BlacklistSettings Blacklist => Service.Configuration.Blacklist;

    public string TabName => "Conditions";
    public bool Enabled => true;

    private int newFramerateLimit;

    public GeneralConfigurationTab()
    {
        newFramerateLimit = Settings.FrameRateLimit;
    }

    public void Draw()
    {
        ImGui.Checkbox("Disable during combat", ref Settings.DisableDuringCombat);
        ImGui.Checkbox("Disable during duty", ref Settings.DisableDuringDuty);
        ImGui.Checkbox("Disable during cutscene", ref Settings.DisableDuringCutscene);
        ImGui.Checkbox("Disable in specific zones", ref Blacklist.Enabled);
        ImGui.Checkbox("Disable during Quest Events", ref Settings.DisableDuringQuestEvent);
        ImGui.Checkbox("Disable during Crafting", ref Settings.DisableDuringCrafting);
        ImGui.Checkbox("Disable in Island Sanctuary", ref Settings.DisableIslandSanctuary);

        ImGuiHelpers.ScaledDummy(20.0f);

        ImGui.SetNextItemWidth(50 * ImGuiHelpers.GlobalScale);

        ImGui.InputInt("Framerate Limit", ref newFramerateLimit, 0, 0);
        if (ImGui.IsItemDeactivatedAfterEdit())
        {
            Settings.FrameRateLimit = Math.Max(newFramerateLimit, 10);
        }
            
        ImGuiComponents.HelpMarker("The framerate value to limit the game to\n" + "Minimum: 10");

        var frametimeExact = 1000 / Settings.FrameRateLimit + 1;
        var approximateFramerate = 1000 / frametimeExact;

        Utilities.Draw.NumericDisplay("Approximated Framerate", approximateFramerate);
        ImGuiComponents.HelpMarker("Framerate limit will be approximated not exact");
    }

    public void Dispose()
    {

    }
}