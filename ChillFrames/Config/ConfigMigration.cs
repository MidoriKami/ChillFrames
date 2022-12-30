using System.Collections.Generic;
using KamiLib.Configuration;

namespace ChillFrames.Config;

public static class ConfigMigration
{
    public static void LoadConfiguration()
    {
        Migrate.LoadFile(Service.PluginInterface.ConfigFile);

        switch (Migrate.GetFileVersion())
        {
            case 1:
                Service.Configuration = MigrateVersionOne();
                Service.Configuration.Initialize(Service.PluginInterface);
                Service.Configuration.Save();
                break;
            
            case 2:
                Service.Configuration = Service.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
                Service.Configuration.Initialize(Service.PluginInterface);
                break;
        }
    }

    private static Configuration MigrateVersionOne()
    {
        return new Configuration
        {
            DevMode = false,
            DisableIncrementSetting = Migrate.GetSettingValue<float>("DisableIncrement"),
            EnableIncrementSetting = Migrate.GetSettingValue<float>("EnableIncrement"),
            Blacklist = new BlacklistSettings
            {
                EnabledSetting = Migrate.GetSettingValue<bool>("Blacklist.Enabled"),
                ModeSetting = Migrate.GetSettingEnum<BlacklistMode>("Blacklist.Mode"),
                BlacklistedZones = new Setting<List<uint>>(new List<uint>()),
            },
            General = new GeneralSettings
            {
                EnableLimiterSetting = Migrate.GetSettingValue<bool>("General.EnableLimiter"),
                FrameRateLimitSetting = Migrate.GetSettingValue<int>("General.FrameRateLimit"),
                DisableDuringCutsceneSetting = Migrate.GetSettingValue<bool>("General.DisableDuringCutscene"),
                DisableDuringCombatSetting = Migrate.GetSettingValue<bool>("General.DisableDuringCombat"),
                DisableDuringDutySetting = Migrate.GetSettingValue<bool>("General.DisableDuringDuty"),
                DisableDuringQuestEventSetting = Migrate.GetSettingValue<bool>("General.DisableDuringQuestEvent"),
                DisableDuringCraftingSetting = Migrate.GetSettingValue<bool>("General.DisableDuringCrafting"),
                DisableIslandSanctuarySetting = Migrate.GetSettingValue<bool>("General.DisableIslandSanctuary"),
                DisableDuringDutyRecorderPlaybackSetting = Migrate.GetSettingValue<bool>("General.DisableDuringDutyRecorderPlayback"),
            },
            Version = 2,
        };
    }
}