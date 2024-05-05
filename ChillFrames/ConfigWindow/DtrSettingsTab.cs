using System.Drawing;
using ChillFrames.Controllers;
using Dalamud.Interface;
using Dalamud.Interface.Utility;
using ImGuiNET;
using KamiLib.TabBar;

namespace ChillFrames.Views.ConfigWindow;

public class DtrSettingsTab : ITabItem {
    public string Name => "DTR Entry";
    public bool Disabled => false;
    public void Draw() {
        if (ImGui.Checkbox("Enable", ref ChillFramesSystem.Config.General.EnableDtrBar)) {
            ChillFramesSystem.DtrController.UpdateEnabled();
            ChillFramesSystem.Config.Save();
        }

        if (ImGui.Checkbox("Show Color", ref ChillFramesSystem.Config.General.EnableDtrColor)) {
            ChillFramesSystem.Config.Save();
        }

        ImGuiHelpers.ScaledDummy(5.0f);
        
        if (ImGuiTweaks.ColorEditWithDefault("Enabled Color", ref ChillFramesSystem.Config.General.EnabledColor, KnownColor.Green.Vector())) {
            ChillFramesSystem.Config.Save();
        }
        
        if (ImGuiTweaks.ColorEditWithDefault("Disabled Color", ref ChillFramesSystem.Config.General.DisabledColor,KnownColor.Red.Vector())) {
            ChillFramesSystem.Config.Save();
        }
    }
}