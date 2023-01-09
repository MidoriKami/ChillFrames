using System.Collections.Generic;
using KamiLib.Configuration;

namespace ChillFrames.Config;

public enum BlacklistMode
{
    // Do not limit Framerate in these zones
    Exclusion,

    // Only limit framerate in these zones
    Inclusion
}

public class BlacklistSettings
{
    public Setting<bool> EnabledSetting = new(false);
    public Setting<BlacklistMode> ModeSetting = new(BlacklistMode.Exclusion);

    public Setting<List<uint>> BlacklistedZones = new(new List<uint>());
}