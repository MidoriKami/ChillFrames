using System;
using System.IO;
using System.Text.Json;
using ChillFrames.Utilities;

namespace ChillFrames.Classes;

public class Configuration {
    public float DisableIncrementSetting = 0.025f;
    public float EnableIncrementSetting = 0.01f;

    public GeneralSettings General = new();
    public LimiterSettings Limiter = new();

    public bool PluginEnable = true;
    public int ConfigVersion;

    public static Configuration Load() {
        var config = Config.LoadConfig<Configuration>("System.config.json");
        config.Migrate();
        return config;
    }

    public void Save()
        => Config.SaveConfig(this, "System.config.json");

    private void Migrate() {
        try {
            switch (ConfigVersion) {
                case 0:
                    MigrateToVersion1();
                    Save();
                    break;
            }
        }
        catch (Exception e) {
            Services.PluginLog.Error(e, "Error migrating config.");
        }
    }

    private void MigrateToVersion1() {
        var fileInfo = FileHelpers.GetFileInfo("System.config.json");
        if (fileInfo is { Exists: true }) {
            using var document = JsonDocument.Parse(File.ReadAllText(fileInfo.FullName));
            var root = document.RootElement;

            if (root.TryGetProperty("General", out var general)) {
                MigrateConditionField(general, "DisableDuringBardPerformance", ref General.BardPerformanceTarget);
                MigrateConditionField(general, "DisableDuringCombatSetting", ref General.CombatTarget);
                MigrateConditionField(general, "DisableDuringCraftingSetting", ref General.CraftingTarget);
                MigrateConditionField(general, "DisableDuringCutsceneSetting", ref General.CutsceneTarget);
                MigrateConditionField(general, "DisableDuringDutyRecorderPlaybackSetting", ref General.DutyRecorderPlaybackTarget);
                MigrateConditionField(general, "DisableDuringDutySetting", ref General.DutyTarget);
                MigrateConditionField(general, "DisableDuringGpose", ref General.GposeTarget);
                MigrateConditionField(general, "DisableDuringQuestEventSetting", ref General.QuestEventTarget);
                MigrateConditionField(general, "DisableIslandSanctuarySetting", ref General.IslandSanctuaryTarget);
                MigrateConditionField(general, "DisableInEstatesSetting", ref General.EstateTarget);
            }

            if (root.TryGetProperty("Limiter", out var limiter)) {
                if (limiter.TryGetProperty("IdleFramerateTarget", out var idle))
                    Limiter.LowerFramerateTarget = idle.GetInt32();
                if (limiter.TryGetProperty("ActiveFramerateTarget", out var active))
                    Limiter.UpperFramerateTarget = active.GetInt32();
            }
        }

        ConfigVersion = 1;
    }

    private static void MigrateConditionField(JsonElement parent, string oldFieldName, ref LimiterStateTarget target) {
        if (!parent.TryGetProperty(oldFieldName, out var value)) return;
        target = value.ValueKind == JsonValueKind.True ? LimiterStateTarget.UpperLimit : LimiterStateTarget.LowerLimit;
    }
}
