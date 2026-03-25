using System;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Text.Json;
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
    public int LowerFramerateTarget = 30;
    public int BaseFramerateTarget = 60;
    public int UpperFramerateTarget = 144;
}

public class Configuration {
    public float DisableIncrementSetting = 0.025f;
    public float EnableIncrementSetting = 0.01f;

    public GeneralSettings General = new();
    public LimiterSettings Limiter = new();

    public bool PluginEnable = true;
    public int ConfigVersion = 0;

    public static Configuration Load() {
        var config = Config.LoadConfig<Configuration>("System.config.json");
        config.Migrate();
        return config;
    }

    public void Save()
        => Config.SaveConfig(this, "System.config.json");

    private void Migrate() {
        if (ConfigVersion >= 1) return;

        var filePath = FileHelpers.GetFileInfo("System.config.json").FullName;
        if (File.Exists(filePath)) {
            try {
                using var document = JsonDocument.Parse(File.ReadAllText(filePath));
                var root = document.RootElement;

                if (root.TryGetProperty("General", out var general)) {
                    MigrateConditionField(general, "DisableDuringBardPerformance",            ref General.BardPerformanceTarget);
                    MigrateConditionField(general, "DisableDuringCombatSetting",               ref General.CombatTarget);
                    MigrateConditionField(general, "DisableDuringCraftingSetting",             ref General.CraftingTarget);
                    MigrateConditionField(general, "DisableDuringCutsceneSetting",             ref General.CutsceneTarget);
                    MigrateConditionField(general, "DisableDuringDutyRecorderPlaybackSetting", ref General.DutyRecorderPlaybackTarget);
                    MigrateConditionField(general, "DisableDuringDutySetting",                 ref General.DutyTarget);
                    MigrateConditionField(general, "DisableDuringGpose",                       ref General.GposeTarget);
                    MigrateConditionField(general, "DisableDuringQuestEventSetting",           ref General.QuestEventTarget);
                    MigrateConditionField(general, "DisableIslandSanctuarySetting",            ref General.IslandSanctuaryTarget);
                    MigrateConditionField(general, "DisableInEstatesSetting",                  ref General.EstateTarget);
                }

                if (root.TryGetProperty("Limiter", out var limiter)) {
                    if (limiter.TryGetProperty("IdleFramerateTarget", out var idle))
                        Limiter.LowerFramerateTarget = idle.GetInt32();
                    if (limiter.TryGetProperty("ActiveFramerateTarget", out var active))
                        Limiter.UpperFramerateTarget = active.GetInt32();
                }
            }
            catch (Exception e) {
                Services.PluginLog.Error(e, "Error migrating config, using defaults.");
            }
        }

        ConfigVersion = 1;
        Save();
    }

    private static void MigrateConditionField(JsonElement parent, string oldFieldName, ref LimiterStateTarget target) {
        if (!parent.TryGetProperty(oldFieldName, out var value)) return;
        target = value.ValueKind == JsonValueKind.True ? LimiterStateTarget.UpperLimit : LimiterStateTarget.LowerLimit;
    }
}
