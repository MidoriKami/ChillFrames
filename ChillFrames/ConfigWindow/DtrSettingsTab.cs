using ChillFrames.Controllers;
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
    }
}