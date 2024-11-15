using System.Collections.Generic;
using System.Text.Json.Serialization;
using KamiLib.Configuration;
using Lumina.Excel.Sheets;

namespace ChillFrames.Classes;

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
    public ushort EnabledColor = 1;
    public ushort DisabledColor = 66;

    [JsonIgnore] public UIColor EnabledUiColor => Service.DataManager.GetExcelSheet<UIColor>().GetRow(EnabledColor);
    [JsonIgnore] public UIColor DisabledUiColor => Service.DataManager.GetExcelSheet<UIColor>().GetRow(DisabledColor);
}

public class LimiterSettings {
    public int ActiveFramerateTarget = 60;
    public int IdleFramerateTarget = 60;
}

public class Configuration {
    public BlacklistSettings Blacklist = new();
    public float DisableIncrementSetting = 0.025f;
    public float EnableIncrementSetting = 0.01f;

    public GeneralSettings General = new();
    public LimiterSettings Limiter = new();

    public bool PluginEnable = true;

    public static Configuration Load()
        => Service.PluginInterface.LoadConfigFile("System.config.json", () => new Configuration());

    public void Save() 
        => Service.PluginInterface.SaveConfigFile("System.config.json", this);
}