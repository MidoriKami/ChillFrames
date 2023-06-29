// ReSharper disable CollectionNeverUpdated.Global
using System.Collections.Generic;
using KamiLib.AutomaticUserInterface;
using NoTankYou.Models.Attributes;

namespace ChillFrames.Config;

[Category("Zone Filter Enable")]
public interface IBlacklistEnable
{
    [BoolConfig("Enable")]
    public bool Enabled { get; set; }
}

[Category("Zone Filters")]
public class BlacklistSettings : IBlacklistEnable
{
    public bool Enabled { get; set; } = false;

    [Blacklist]
    public HashSet<uint> BlacklistedZones { get; set; } = new();
}