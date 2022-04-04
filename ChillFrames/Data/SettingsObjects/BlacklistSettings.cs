using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChillFrames.Data.Enums;
using ChillFrames.Data.SettingsObjects.Components;
using Lumina.Excel.GeneratedSheets;

namespace ChillFrames.Data.SettingsObjects
{
    public class BlacklistSettings
    {
        public bool Enabled = false;
        public BlacklistMode Mode = BlacklistMode.Exclusion;

        public List<SimpleTerritory> Territories = new();
    }
}
