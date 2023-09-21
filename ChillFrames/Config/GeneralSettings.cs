using KamiLib.AutomaticUserInterface;

namespace ChillFrames.Config;

[Category("Feature Toggles")]
public interface IFeatureToggles
{
    [BoolConfig("InCutscenes")]
    public bool DisableDuringCutsceneSetting { get; set; }
    
    [BoolConfig("InCombat")]
    public bool DisableDuringCombatSetting { get; set; }
    
    [BoolConfig("InDuties")]
    public bool DisableDuringDutySetting { get; set; }
    
    [BoolConfig("InQuestEvent")]
    public bool DisableDuringQuestEventSetting { get; set; }
    
    [BoolConfig("InCrafting")]
    public bool DisableDuringCraftingSetting { get; set; }
    
    [BoolConfig("InIslandSanctuary")]
    public bool DisableIslandSanctuarySetting { get; set; }
    
    [BoolConfig("InDutyRecorderPlayback")]
    public bool DisableDuringDutyRecorderPlaybackSetting { get; set; }
    
    [BoolConfig("InBardPerformance")]
    public bool DisableDuringBardPerformance { get; set; }
}

public class GeneralSettings : IFeatureToggles
{
    // IFeatureToggles
    public bool DisableDuringCutsceneSetting { get; set; } = true;
    public bool DisableDuringCombatSetting { get; set; } = true;
    public bool DisableDuringDutySetting { get; set; } = true;
    public bool DisableDuringQuestEventSetting { get; set; } = true;
    public bool DisableDuringCraftingSetting { get; set; } = true;
    public bool DisableIslandSanctuarySetting { get; set; } = true;
    public bool DisableDuringDutyRecorderPlaybackSetting { get; set; } = true;
    public bool DisableDuringBardPerformance { get; set; } = true;

}