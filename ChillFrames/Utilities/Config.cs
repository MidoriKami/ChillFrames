namespace ChillFrames.Utilities;

/// <summary>
/// Configuration File Utilities
/// </summary>
public static class Config {
    public static string ConfigPath => FileHelpers.GetFileInfo().FullName;

    /// <summary>
    /// Loads a configuration file from PluginConfigs\VanillaPlus\Configs\{FileName}
    /// Creates a `new T()` or uses passed in defaultValue object if the file can't be loaded
    /// </summary>
    public static T LoadConfig<T>(string fileName, T? defaultValue = null) where T : class, new()
        => FileHelpers.LoadFile(FileHelpers.GetFileInfo(fileName).FullName, defaultValue);
    
    /// <summary>
    /// Saves a configuration file to PluginConfigs\ChillFrames\{FileName}
    /// </summary>
    public static void SaveConfig<T>(T configObject, string fileName)
        => FileHelpers.SaveFile(configObject, FileHelpers.GetFileInfo(fileName).FullName);
}

