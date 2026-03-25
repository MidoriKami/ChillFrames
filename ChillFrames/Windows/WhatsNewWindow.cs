using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Windowing;

namespace ChillFrames.Windows;

public class WhatsNewWindow : Window {
    public WhatsNewWindow() : base("ChillFrames - What's New") {
        SizeConstraints = new WindowSizeConstraints {
            MinimumSize = new Vector2(420.0f, 130.0f),
        };
    }

    public override void Draw() {
        ImGui.Text("v3.3.0.0");
        ImGui.Text("- Framerate limiting now supports three tiers: Lower, Base, and Upper");
        ImGui.Text("- Each game condition can be assigned to any tier");
        ImGui.Text("- Your existing settings have been migrated automatically");

        ImGui.Spacing();

        if (ImGui.Button("Close")) {
            IsOpen = false;
        }
    }
}
