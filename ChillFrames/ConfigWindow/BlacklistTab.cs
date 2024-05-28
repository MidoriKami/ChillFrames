using System.Drawing;
using System.Linq;
using System.Numerics;
using ChillFrames.Controllers;
using Dalamud.Interface;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using ImGuiNET;
using KamiLib.Components;
using KamiLib.Extensions;
using KamiLib.Window.SelectionWindows;
using Lumina.Excel.GeneratedSheets;

namespace ChillFrames.Views.ConfigWindow;

public class BlacklistTab : ITabItem {
    public string Name => "Zone Blacklist";
    public bool Disabled => false;

    public void Draw() {
        using (var _ = ImRaii.PushColor(ImGuiCol.ChildBg, KnownColor.OrangeRed.Vector() with { W = 0.15f }, ChillFramesSystem.Config.Blacklist.BlacklistedZones.Contains(Service.ClientState.TerritoryType))) {
            DrawAddRemovableTerritory(GetCurrentTerritory());
        }
        
        ImGui.Separator();
        
        DrawCurrentlyBlacklisted();
        
        DrawAddNewButton();
    }
    
    private void DrawAddRemovableTerritory(TerritoryType territory) {
        using (var _ = ImRaii.PushFont(UiBuilder.IconFont)) {
            if (ChillFramesSystem.Config.Blacklist.BlacklistedZones.Contains(territory.RowId)) {
                if (ImGui.Button($"{FontAwesomeIcon.Trash.ToIconString()}##removeZone{territory.RowId}", new Vector2(25.0f, 75.0f))) {
                    ChillFramesSystem.Config.Blacklist.BlacklistedZones.Remove(territory.RowId);
                    ChillFramesSystem.Config.Save();
                }
            }
            else {
                if (ImGui.Button($"{FontAwesomeIcon.Plus.ToIconString()}##addZone{territory.RowId}", new Vector2(25.0f, 75.0f))) {
                    ChillFramesSystem.Config.Blacklist.BlacklistedZones.Add(territory.RowId);
                    ChillFramesSystem.Config.Save();
                }
            }
        }
        
        ImGui.SameLine();
        
        territory.Draw(Service.DataManager, Service.TextureProvider);
    }
    
    private void DrawCurrentlyBlacklisted() {
        using var child = ImRaii.Child("blacklist_frame", new Vector2(ImGui.GetContentRegionAvail().X, ImGui.GetContentRegionAvail().Y - 25.0f * ImGuiHelpers.GlobalScale));
        if (!child) return;

        ImGuiClip.ClippedDraw(ChillFramesSystem.Config.Blacklist.BlacklistedZones.ToList(), zoneId => {
            if (Service.DataManager.GetExcelSheet<TerritoryType>()?.GetRow(zoneId) is { } territory) {
                DrawAddRemovableTerritory(territory);
            }
        }, 75.0f);
    }
    
    private void DrawAddNewButton() {
        using var child = ImRaii.Child("open_selector_frame", ImGui.GetContentRegionAvail());
        if (!child) return;

        using var _ = ImRaii.PushFont(UiBuilder.IconFont);
        if (ImGui.Button($"{FontAwesomeIcon.Plus.ToIconString()}", ImGui.GetContentRegionAvail())) {
            ChillFramesSystem.WindowManager.AddWindow(new TerritorySelectionWindow(Service.PluginInterface) {
                MultiSelectionCallback = selections => {
                    foreach (var selection in selections) {
                        ChillFramesSystem.Config.Blacklist.BlacklistedZones.Add(selection.RowId);
                        ChillFramesSystem.Config.Save();
                    }
                }
            });
        }
    }

    private static TerritoryType GetCurrentTerritory() 
        => Service.DataManager.GetExcelSheet<TerritoryType>()!.GetRow(Service.ClientState.TerritoryType)!;
}