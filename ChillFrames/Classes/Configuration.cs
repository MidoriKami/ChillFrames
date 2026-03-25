using System.Drawing;
using System.Numerics;
using ChillFrames.Utilities;
using Dalamud.Interface;

namespace ChillFrames.Classes;

public class GeneralSettings {
    public LimiterStateTarget BardPerformanceTarget = LimiterStateTarget.BaseLimit;
    public LimiterStateTarget CombatTarget = LimiterStateTarget.UpperLimit;
    public LimiterStateTarget CraftingTarget = LimiterStateTarget.BaseLimit;
    public LimiterStateTarget CutsceneTarget = LimiterStateTarget.BaseLimit;
    public LimiterStateTarget DutyRecorderPlaybackTarget = LimiterStateTarget.BaseLimit;
    public LimiterStateTarget DutyTarget = LimiterStateTarget.BaseLimit;
    public LimiterStateTarget EstateTarget = LimiterStateTarget.LowerLimit;
    public LimiterStateTarget GposeTarget = LimiterStateTarget.BaseLimit;
    public LimiterStateTarget IslandSanctuaryTarget = LimiterStateTarget.BaseLimit;
    public LimiterStateTarget QuestEventTarget = LimiterStateTarget.BaseLimit;
    public bool EnableDtrBar = true;
    public bool EnableDtrColor = true;
    public Vector4 ActiveColor = KnownColor.LightGreen.Vector();
    public Vector4 InactiveColor = KnownColor.OrangeRed.Vector();
}

public class LimiterSettings {
    public int LowerFramerateTarget = 15;
    public int BaseFramerateTarget = 60;
    public int UpperFramerateTarget = 144;
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
