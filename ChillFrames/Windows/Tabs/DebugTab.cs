using KamiLib.InfoBoxSystem;
using KamiLib.Interfaces;

namespace ChillFrames.Windows.Tabs;

internal class DebugTab : ITabItem
{
    public string TabName => "Debug";
    public bool Enabled => Service.Configuration.DevMode;
        
    public void Draw()
    {
        InfoBox.Instance
            .AddTitle("Debug Features", 1.0f)
            .AddDragFloat("Disable Increment", Service.Configuration.DisableIncrementSetting, 0.001f, 0.150f, InfoBox.Instance.InnerWidth / 2.0f, 3)
            .AddDragFloat("Enable Increment", Service.Configuration.EnableIncrementSetting, 0.001f, 0.150f, InfoBox.Instance.InnerWidth / 2.0f, 3)
            .Draw();
    }
}