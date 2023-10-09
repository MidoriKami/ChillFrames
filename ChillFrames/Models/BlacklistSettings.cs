// ReSharper disable CollectionNeverUpdated.Global

using System.Collections.Generic;
using KamiLib.AutomaticUserInterface;

namespace ChillFrames.Models;

[Category("Zone Filter Enable")]
public interface IBlacklistEnable
{
    [BoolConfig("Enable")] public bool Enabled { get; set; }
}

[Category("Zone Filters")]
public class BlacklistSettings : IBlacklistEnable
{
    [Blacklist] public HashSet<uint> BlacklistedZones { get; set; } = new();
    public bool Enabled { get; set; } = false;
}