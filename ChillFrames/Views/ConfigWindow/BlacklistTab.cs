using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Numerics;
using ChillFrames.Controllers;
using Dalamud.Interface;
using Dalamud.Interface.Components;
using Dalamud.Interface.Utility;
using Dalamud.Utility;
using ImGuiNET;
using KamiLib.Game;
using KamiLib.Interfaces;
using KamiLib.Search;
using Lumina.Excel.GeneratedSheets;

namespace ChillFrames.Views.ConfigWindow;

public class BlacklistTab : ITabItem {
    public string TabName => "Territory Blacklist";
    public bool Enabled => true;

    private readonly HashSet<uint> removalList = new();

    private readonly TerritorySearchModal territorySearchModal = new() {
        Name = "Territory Search",
        AddAction = territory => {
            if (!ChillFramesSystem.Config.Blacklist.BlacklistedZones.Contains(territory.RowId)) {
                ChillFramesSystem.Config.Blacklist.BlacklistedZones.Add(territory.RowId);
                ChillFramesSystem.Config.Save();
            }
        }
    };
    
    public void Draw() {
        if (ImGui.BeginChild("BlacklistChild", new Vector2(ImGui.GetContentRegionMax().X, ImGui.GetContentRegionMax().Y - 23.0f * ImGuiHelpers.GlobalScale - ImGui.GetStyle().ItemSpacing.Y))) {
            if (ChillFramesSystem.Config.Blacklist.BlacklistedZones.Any()) {
                foreach (var territory in ChillFramesSystem.Config.Blacklist.BlacklistedZones.OrderBy(item => item)) {
                    DrawTerritoryLine(territory);
                }
            }
            else {
                const string text = "Nothing Blacklisted";
                var textSize = ImGui.CalcTextSize(text);
                ImGui.SetCursorPos(ImGui.GetContentRegionMax() / 2.0f - textSize / 2.0f);
                ImGui.TextColored(KnownColor.OrangeRed.Vector(), text);
            }
        }
        ImGui.EndChild();
        
        ImGui.PushFont(UiBuilder.IconFont);
        if (ImGui.Button(FontAwesomeIcon.Plus.ToIconString(), new Vector2(ImGui.GetContentRegionMax().X, 23.0f * ImGuiHelpers.GlobalScale))) {
            ImGui.OpenPopup("Territory Search");
        }
        ImGui.PopFont();

        territorySearchModal.DrawSearchModal();

        if (removalList.Any()) {
            foreach (var toRemove in removalList) {
                ChillFramesSystem.Config.Blacklist.BlacklistedZones.Remove(toRemove);
            }
            
            ChillFramesSystem.Config.Save();
            removalList.Clear();
        }
    }

    private void DrawTerritoryLine(uint territory) {
        var territoryType = LuminaCache<TerritoryType>.Instance.GetRow(territory)!;
        
        if (ImGuiComponents.IconButton($"Remove{territory}", FontAwesomeIcon.Trash)) {
            removalList.Add(territory);
        }
        
        ImGui.SameLine();
        
        ImGui.SameLine();
        ImGui.TextColored(KnownColor.Gray.Vector(), territoryType.RowId.ToString());
        
        ImGui.SameLine(75.0f);
        ImGui.TextUnformatted(territoryType.PlaceName.Value?.Name.ToDalamudString().ToString());

        ImGui.SameLine();
        if (GetDutyNameForTerritoryType(territoryType.RowId) is { } dutyName && !dutyName.IsNullOrEmpty()) {
            ImGui.TextColored(KnownColor.Gray.Vector(), $"( {dutyName} )");
        }
        else {
            ImGui.Text(string.Empty);
        }
    }
    
    private static string? GetDutyNameForTerritoryType(uint territory) {
        if (LuminaCache<TerritoryType>.Instance.GetRow(territory) is not { ContentFinderCondition.Row: var cfcRow }) return null;
        if (LuminaCache<ContentFinderCondition>.Instance.GetRow(cfcRow) is not { Name: var dutyName }) return null;

        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dutyName.ToDalamudString().TextValue);
    }
}