using ChillFrames.Controllers;
using KamiLib.AutomaticUserInterface;
using KamiLib.Interfaces;

namespace ChillFrames.Views.ConfigWindow;

public class ZoneFilterTab : ITabItem
{
    public string TabName => "Zone Filters";
    public bool Enabled => true;
    public void Draw()
    {
        DrawableAttribute.DrawAttributes(ChillFramesSystem.Config.Blacklist, ChillFramesSystem.Config.Save);
    }
}