using ChillFrames.Config;
using KamiLib.InfoBoxSystem;
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
            .Draw();
        
        InfoBox.Instance
            .AddTitle("Framerate Target", 1.0f)
            .AddInputInt("Framerate Limit", Settings.FrameRateLimitSetting, 10, 255, 0, 0, InfoBox.Instance.InnerWidth / 4.0f)
            .AddString($"Approximated Framerate {1000 / (1000 / Settings.FrameRateLimitSetting.Value + 1)}")
            .AddHelpMarker("Framerate limit will be approximated not exact")
            .Draw();
    }
}