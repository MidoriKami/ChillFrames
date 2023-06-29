using KamiLib.Interfaces;

namespace ChillFrames.Windows.Tabs;

public class LimiterSettingsTab : ITabItem
{
    public string TabName => "Limiter Settings";
    public bool Enabled => true;
    public void Draw()
    {
        ChillFramesPlugin.System.DrawLimiterConfig();
    }
}