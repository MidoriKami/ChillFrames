using KamiLib.Configuration;

namespace ChillFrames.Config;

public class GeneralSettings
{
    public Setting<bool> EnableLimiterSetting = new(true);
    public Setting<int> FrameRateLimitSetting = new(60);

    public Setting<bool> DisableDuringCutsceneSetting = new(true);
    public Setting<bool> DisableDuringCombatSetting = new(true);
    public Setting<bool> DisableDuringDutySetting = new(true);
    public Setting<bool> DisableDuringQuestEventSetting = new(true);
    public Setting<bool> DisableDuringCraftingSetting = new(true);
    public Setting<bool> DisableIslandSanctuarySetting = new(true);
    public Setting<bool> DisableDuringDutyRecorderPlaybackSetting = new(true);
}