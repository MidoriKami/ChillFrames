using KamiLib.Interfaces;

namespace ChillFrames.Windows.Tabs;

public class ZoneFilterTab : ITabItem
{
    public string TabName => "Zone Filters";
    public bool Enabled => true;
    public void Draw() => ChillFramesPlugin.System.DrawZoneFilterConfig();
}