using System.Collections.Generic;
using KamiLib.Configuration;

namespace ChillFrames.Config;

public enum BlacklistMode
{
    /// <summary>
    /// Do not limit Framerate in these zones
    /// </summary>
    Exclusion,

    /// <summary>
    /// Only limit framerate in these zones
    /// </summary>
    Inclusion
}

public class BlacklistSettings
{
    public Setting<bool> EnabledSetting = new(false);
    public Setting<BlacklistMode> ModeSetting = new(BlacklistMode.Exclusion);

    public Setting<List<uint>> BlacklistedZones = new(new List<uint>());
}