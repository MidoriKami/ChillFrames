using System.Collections.Generic;
using KamiLib.Configuration;
using KamiLib.ZoneFilterList;

namespace ChillFrames.Config;

public class BlacklistSettings
{
    public Setting<bool> EnabledSetting = new(false);
    public Setting<ZoneFilterTypeId> FilterSetting = new(ZoneFilterTypeId.Blacklist);

    public Setting<List<uint>> BlacklistedZones = new(new List<uint>());
}