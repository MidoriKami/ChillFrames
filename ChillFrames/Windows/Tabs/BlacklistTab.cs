using ChillFrames.Config;
using KamiLib.Drawing;
using KamiLib.Interfaces;
using KamiLib.ZoneFilterList;

namespace ChillFrames.Windows.Tabs;

internal class BlacklistTab : ITabItem
{
    private static BlacklistSettings Settings => Service.Configuration.Blacklist;
    
    public string TabName => "Blacklist";
    public bool Enabled => Service.Configuration.Blacklist.EnabledSetting;

    private ZoneFilterType Filter => Settings.FilterSetting.Value == ZoneFilterTypeId.Blacklist ? ZoneFilterType.BlackList : ZoneFilterType.WhiteList;
    
    public void Draw()
    {
        InfoBox.Instance
            .AddTitle("Zone Filter Mode", 1.0f)
            .BeginTable()
            .BeginRow()
            .AddConfigRadio("Never Limit", Settings.FilterSetting, ZoneFilterTypeId.Blacklist, "Never limit framerate in these zones")
            .AddConfigRadio("Always Limit", Settings.FilterSetting, ZoneFilterTypeId.Whitelist, "Always limit framerate in these zones")
            .EndRow()
            .EndTable()
            .Draw();
        
        if (Service.ClientState.TerritoryType != 0)
        {
            ZoneFilterListDraw.DrawAddRemoveHere(Settings.BlacklistedZones);
        }
        
        ZoneFilterListDraw.DrawTerritorySearch(Settings.BlacklistedZones, Filter);
        
        ZoneFilterListDraw.DrawZoneList(Settings.BlacklistedZones, Filter);
    }
}