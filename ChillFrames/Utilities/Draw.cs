using Dalamud.Interface.Components;
using ImGuiNET;

namespace ChillFrames.Utilities;

internal static class Draw
{
    public static void NumericDisplay(string label, int value)
    {
        ImGui.Text(label);
        ImGui.SameLine();
        ImGui.Text($"{value}");
    }

    public static void Checkbox(string label, ref bool refValue, string helpText = "")
    {
        ImGui.Checkbox($"{label}", ref refValue);

        if (helpText != string.Empty)
        {
            ImGuiComponents.HelpMarker(helpText);
        }
    }
}