using System.Drawing;
using ChillFrames.Controllers;
using ChillFrames.Interfaces;
using Dalamud.Interface;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using ImGuiNET;
using KamiLib.Interfaces;

namespace ChillFrames.Views.ConfigWindow;

public class LimiterSettingsTab : ITabItem
{

    
    public string TabName => "Limiter Settings";
    public bool Enabled => true;

    public void Draw()
    {
        ImGui.Text("These options act as an override for the idle limiter");
        ImGui.Text("If an option is enabled, and the condition is active:\nThe idle limiter will be disabled, and the active limiter will be used ");
        
        foreach (var option in ChillFramesPlugin.System.LimiterOptions)
        {
            DrawOption(option);
        }
    }

    private void DrawOption(IFrameLimiterOption option)
    {
        using var table = ImRaii.Table("limiter_options_table", 4);
        if (!table) return;
        
        ImGui.TableSetupColumn("##option_enable", ImGuiTableColumnFlags.WidthFixed, 25.0f * ImGuiHelpers.GlobalScale);
        ImGui.TableSetupColumn("##option_label", ImGuiTableColumnFlags.WidthFixed, 150.0f * ImGuiHelpers.GlobalScale);
        ImGui.TableSetupColumn("##option_status", ImGuiTableColumnFlags.WidthFixed, 75.0f * ImGuiHelpers.GlobalScale);
        ImGui.TableSetupColumn("##option_effect", ImGuiTableColumnFlags.WidthStretch);
        
        ImGui.TableNextColumn();
        if (ImGui.Checkbox($"##{option.Label}", ref option.GetSetting()))
        {
            ChillFramesSystem.Config.Save();
        }
        
        ImGui.TableNextColumn();
        ImGui.Text(option.Label);

        ImGui.TableNextColumn();
        if (option.IsActive())
        {
            ImGui.TextColored(KnownColor.Green.Vector(), "Active");
        }
        else
        {
            ImGui.TextColored(KnownColor.OrangeRed.Vector(), "Inactive");
        }

        ImGui.TableNextColumn();
        if (option.IsEnabled() && option.IsActive())
        {
            ImGui.TextColored(KnownColor.OrangeRed.Vector(), "Disabling idle limiter");
        }
        else if (option.IsEnabled() && !option.IsActive())
        {
            ImGui.Text("Condition not active");
        }
        else if (!option.IsEnabled())
        {
            ImGui.Text("Option not enabled");
        }
    }
}