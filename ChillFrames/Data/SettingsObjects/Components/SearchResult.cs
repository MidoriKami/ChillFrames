using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillFrames.Data.SettingsObjects.Components
{
    internal class SearchResult
    {
        public uint TerritoryID { get; set; }
        public string TerritoryName { get; set; } = "Name Not Set";
        public string TerritoryIntendedUse { get; set; } = "Unknown";
    }
}
