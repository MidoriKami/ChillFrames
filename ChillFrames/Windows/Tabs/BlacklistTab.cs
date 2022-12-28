using ChillFrames.Config;
using KamiLib.BlacklistSystem;
using KamiLib.InfoBoxSystem;
using KamiLib.Interfaces;

namespace ChillFrames.Windows.Tabs;

internal class BlacklistTab : ITabItem
{
    private static BlacklistSettings Settings => Service.Configuration.Blacklist;
    
    public string TabName => "Blacklist";
    public bool Enabled => Service.Configuration.Blacklist.EnabledSetting.Value;

    public void Draw()
    {
        InfoBox.Instance
            .AddTitle("Blacklist Mode", 1.0f)
            .BeginTable()
            .BeginRow()
            .AddConfigRadio("Exclude", Settings.ModeSetting, BlacklistMode.Exclusion, "Do not limit framerate in these zones")
            .AddConfigRadio("Include", Settings.ModeSetting, BlacklistMode.Inclusion, "Limit framerate only in these zones")
            .EndRow()
            .EndTable()
            .Draw();

        if (Service.ClientState.TerritoryType != 0)
        {
            BlacklistDraw.DrawAddRemoveHere(Settings.BlacklistedZones);

        }
        
        BlacklistDraw.DrawTerritorySearch(Settings.BlacklistedZones);
        
        BlacklistDraw.DrawBlacklist(Settings.BlacklistedZones);
    }
}