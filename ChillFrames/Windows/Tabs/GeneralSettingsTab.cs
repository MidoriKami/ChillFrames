using Dalamud.Interface;
using ImGuiNET;
using KamiLib.Interfaces;

namespace ChillFrames.Windows.Tabs;

public class GeneralSettingsTab : ITabItem
{
    public string TabName => "General Settings";
    public bool Enabled => true;
    public void Draw()
    {
        ImGui.TextWrapped("The `Idle Framerate Target` will be used\nwhile any of the below conditions are met and enabled");
        ImGuiHelpers.ScaledDummy(5.0f);
        
        ChillFramesPlugin.System.DrawGeneralConfig();
    }
}