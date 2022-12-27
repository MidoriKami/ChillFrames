using ChillFrames.Config;
using ChillFrames.Interfaces;
using KamiLib.BlacklistSystem;
using KamiLib.InfoBoxSystem;

namespace ChillFrames.Tabs;

internal class BlacklistTab : ITabItem
{
    private BlacklistSettings Settings => Service.Configuration.Blacklist;
    
    public string TabName => "Blacklist";
    public bool Enabled => Service.Configuration.Blacklist.EnabledSetting.Value;

    public void Dispose()
    {
    }

    public void Draw()
    {
        InfoBox.Instance
            .AddTitle("Blacklist Mode", 1.0f)
            .AddConfigRadio("Exclude", Settings.ModeSetting, BlacklistMode.Exclusion, "Do not limit framerate in these zones")
            .AddConfigRadio("Include", Settings.ModeSetting, BlacklistMode.Inclusion, "Limit framerate only in these zones")
            .Draw();

        BlacklistDraw.DrawAddRemoveHere(Settings.BlacklistedZones);
        
        BlacklistDraw.DrawTerritorySearch(Settings.BlacklistedZones);
        
        BlacklistDraw.DrawBlacklist(Settings.BlacklistedZones);
    }
}