using System;
using ChillFrames.Config;
using ImGuiNET;
using KamiLib;
using KamiLib.Drawing;
using KamiLib.Interfaces;

namespace ChillFrames.Windows.Tabs;

internal class GeneralConfigurationTab : ITabItem
{
    private static GeneralSettings Settings => Service.Configuration.General;
    private static BlacklistSettings Blacklist => Service.Configuration.Blacklist;

    public string TabName => "Conditions";
    public bool Enabled => true;

    public void Draw()
    {
        InfoBox.Instance
            .AddTitle("General Settings", 1.0f)
            .AddString("The framerate limiter will be active unless any of the below conditions are met and enabled")
            .AddHelpMarker("For example, with 'Disable during combat' enabled\nAnytime you are in combat, ChillFrames will NOT lower your framerate")
            .AddConfigCheckbox("Disable during combat", Settings.DisableDuringCombatSetting)
            .AddConfigCheckbox("Disable during duty", Settings.DisableDuringDutySetting)
            .AddConfigCheckbox("Disable during cutscene", Settings.DisableDuringCutsceneSetting)
            .AddConfigCheckbox("Disable in specific zones", Blacklist.EnabledSetting)
            .AddConfigCheckbox("Disable during Quest Events", Settings.DisableDuringQuestEventSetting)
            .AddConfigCheckbox("Disable during Crafting", Settings.DisableDuringCraftingSetting)
            .AddConfigCheckbox("Disable in Island Sanctuary", Settings.DisableIslandSanctuarySetting)
            .AddConfigCheckbox("Disable during duty recorder playback", Settings.DisableDuringDutyRecorderPlaybackSetting)
            .AddConfigCheckbox("Disable during bard performance", Settings.DisableDuringBardPerformance)
            .Draw();

        if (Settings.PreciseFramerate)
        {
            InfoBox.Instance
                .AddTitle("Framerate Target", out var innerWidth, 1.0f)
                .AddConfigCheckbox("Precise Mode", Settings.PreciseFramerate, "More precisely limits framerate\nEnabling this will consume more power")
                .AddAction(() => FramerateInputInt(innerWidth))
                .Draw();
        }
        else
        {
            InfoBox.Instance
                .AddTitle("Framerate Target", out var innerWidth, 1.0f)
                .AddConfigCheckbox("Precise Mode", Settings.PreciseFramerate, "More precisely limits framerate\nEnabling this will consume more power")
                .AddAction(() => FramerateInputInt(innerWidth))
                .AddString($"Approximated Framerate {1000 / (1000 / Settings.FrameRateLimitSetting.Value + 1)}")
                .AddHelpMarker("Framerate limit will be approximated not exact")
                .Draw();
        }
    }

    private int tempFramerateTarget;
    
    private void FramerateInputInt(float width)
    {
        ImGui.SetNextItemWidth(width / 4.0f);

        tempFramerateTarget = Settings.FrameRateLimitSetting.Value;

        ImGui.InputInt("Framerate Limit", ref tempFramerateTarget, 0, 0);
        if (ImGui.IsItemDeactivatedAfterEdit())
        {
            Settings.FrameRateLimitSetting.Value = Math.Clamp(tempFramerateTarget, 10, 255);
            KamiCommon.SaveConfiguration();
        }
    }
}