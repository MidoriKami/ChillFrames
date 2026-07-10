using ChillFrames.Utilities;

namespace ChillFrames.Classes;

public class Configuration {
	public float DisableIncrementSetting = 0.025f;
	public float EnableIncrementSetting = 0.01f;

	public GeneralSettings General = new();
	public LimiterSettings Limiter = new();
	public int IdleFpsWaitTime = 50;

	public bool PluginEnable = true;

	public static Configuration Load()
		=> Config.LoadConfig<Configuration>("System.config.json");

	public void Save()
		=> Config.SaveConfig(this, "System.config.json");
}