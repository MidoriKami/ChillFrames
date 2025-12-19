using System.Drawing;
using System.Numerics;
using ChillFrames.Utilities;
using Dalamud.Interface;

namespace ChillFrames.Classes;

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
    public bool DisableInEstatesSetting = true;
    public bool EnableDtrBar = true;
    public bool EnableDtrColor = true;
    public Vector4 ActiveColor = KnownColor.LightGreen.Vector();
    public Vector4 InactiveColor = KnownColor.OrangeRed.Vector();
}

public class LimiterSettings {
    public int ActiveFramerateTarget = 60;
    public int IdleFramerateTarget = 60;
}

public class Configuration {
    public float DisableIncrementSetting = 0.025f;
    public float EnableIncrementSetting = 0.01f;

    public GeneralSettings General = new();
    public LimiterSettings Limiter = new();

    public bool PluginEnable = true;

    public static Configuration Load()
        => Config.LoadConfig<Configuration>("System.config.json");

    public void Save() 
        => Config.SaveConfig(this, "System.config.json");
}