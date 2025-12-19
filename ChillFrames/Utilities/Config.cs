namespace ChillFrames.Utilities;

/// <summary>
/// Configuration File Utilities
/// </summary>
public static class Config {
    public static string ConfigPath => FileHelpers.GetFileInfo().FullName;

    /// <summary>
    /// Loads a configuration file from PluginConfigs\ChillFrames\{FileName}
    /// Creates a `new T()` if the file can't be loaded
    /// </summary>
    public static T LoadConfig<T>(string fileName) where T : new()
        => FileHelpers.LoadFile<T>(FileHelpers.GetFileInfo(fileName).FullName);
    
    /// <summary>
    /// Saves a configuration file to PluginConfigs\ChillFrames\{FileName}
    /// </summary>
    public static void SaveConfig<T>(T modificationConfig, string fileName)
        => FileHelpers.SaveFile(modificationConfig, FileHelpers.GetFileInfo(fileName).FullName);
}

