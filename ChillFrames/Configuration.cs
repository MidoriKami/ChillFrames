using System.Collections.Generic;
using Dalamud.Configuration;

namespace ChillFrames.Models;

public class BlacklistSettings {
    public HashSet<uint> BlacklistedZones { get; set; } = [];
}

public class GeneralSettings {
    public bool DisableDuringBardPerformance = true;
    public bool DisableDuringCombatSetting = true;
    public bool DisableDuringCraftingSetting = true;
    public bool DisableDuringCutsceneSetting = true;
    public bool DisableDuringDutyRecorderPlaybackSetting = true;
    public bool DisableDuringDutySetting = true;
    public bool DisableDuringGpose = true;
    public bool DisableDuringQuestEventSetting = true;
    public bool DisableIslandSanctuarySetting = true;
    public bool EnableDtrBar = true;
    public bool EnableDtrColor = true;
}

public class LimiterSettings {
    public int ActiveFramerateTarget = 60;
    public int IdleFramerateTarget = 60;
}

public class Configuration : IPluginConfiguration {
    public BlacklistSettings Blacklist = new();
    public float DisableIncrementSetting = 0.025f;
    public float EnableIncrementSetting = 0.01f;

    public GeneralSettings General = new();
    public LimiterSettings Limiter = new();

    public bool PluginEnable = true;
    public int Version { get; set; } = 3;

    public void Save() => Service.PluginInterface.SavePluginConfig(this);
}