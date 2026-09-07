using System.Threading.Tasks;
using ChillFrames.Utilities;
using Dalamud.Plugin.Services;

namespace ChillFrames.Classes;

public class Configuration {
	public float DisableIncrementSetting = 0.025f;
	public float EnableIncrementSetting = 0.01f;

	public GeneralSettings General = new();
	public LimiterSettings Limiter = new();

	public int IdleFpsTarget = 20;
	public int IdleFpsDelayTime = 5;

	public bool PluginEnable = true;

	public static async Task<Configuration> Load() {
		IPluginLog.Get().Debug("Loading System.config.json");
		return await Config.LoadConfig<Configuration>("System.config.json");
	}

	public void Save() {
		IPluginLog.Get().Debug("Saving System.config.json");
		Task.Run(() => Config.SaveConfig(this, "System.config.json"));
	}
}