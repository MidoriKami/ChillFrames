using System.Collections.Generic;
using ChillFrames.Data.Enums;
using ChillFrames.Data.SettingsObjects.Components;

namespace ChillFrames.Data.SettingsObjects
{
    public class BlacklistSettings
    {
        public bool Enabled = false;
        public BlacklistMode Mode = BlacklistMode.Exclusion;

        public List<SimpleTerritory> Territories = new();
    }
}
