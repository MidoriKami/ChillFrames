// ReSharper disable CollectionNeverUpdated.Global

using System.Collections.Generic;
using KamiLib.AutomaticUserInterface;

namespace ChillFrames.Models;

[Category("Zone Filters")]
public class BlacklistSettings
{
    [Blacklist] public HashSet<uint> BlacklistedZones { get; set; } = new();
}