using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ChillFrames.Data.Enums;
using ChillFrames.Data.SettingsObjects;
using ChillFrames.Data.SettingsObjects.Components;
using ChillFrames.Interfaces;
using ChillFrames.Utilities;
using Dalamud.Interface;
using Dalamud.Interface.Components;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;

namespace ChillFrames.Tabs
{
    internal class BlacklistTab : ITabItem
    {
        private BlacklistSettings Settings => Service.Configuration.Blacklist;
        private int modeSelect;
        private int modifyBlacklistValue;

        private readonly List<ContentFinderCondition> contentFinderConditionList;
        private readonly HashSet<string> contentTypeList;

        private HashSet<string> instanceNames = new();
        private string selectedContentTypeString = "";
        private string selectedInstanceName = "";

        public string TabName => "Blacklist";
        public bool Enabled => Service.Configuration.Blacklist.Enabled;

        public BlacklistTab()
        {
            modeSelect = (int)Settings.Mode;

            contentFinderConditionList = Service.DataManager.GetExcelSheet<ContentFinderCondition>()!.ToList();

            contentTypeList = contentFinderConditionList
                !.Where(c => c.ContentType.Value?.Name != null)
                .Select(c => c.ContentType.Value!.Name.ToString())
                .ToHashSet();
        }
        
        public void Dispose()
        {
        }

        public void Draw()
        {
            ModeSelect();

            CurrentStatus();

            CurrentZone();

            ManualNumeric();

            NameLookup();
        }

        private void ModeSelect()
        {
            ImGui.Text("Blacklist Mode");

            ImGui.Indent(15 * ImGuiHelpers.GlobalScale);

            ImGui.RadioButton("Exclude", ref modeSelect, (int) BlacklistMode.Exclusion);
            ImGuiComponents.HelpMarker("Do not limit framerate in these zones");

            ImGui.SameLine(150.0f * ImGuiHelpers.GlobalScale);

            ImGui.RadioButton("Include", ref modeSelect, (int) BlacklistMode.Inclusion);
            ImGuiComponents.HelpMarker("Limit framerate only in these zones");

            if (Settings.Mode != (BlacklistMode)modeSelect)
            {
                Settings.Mode = (BlacklistMode)modeSelect;
                Service.Configuration.Save();
            }

            ImGui.Indent(-15 * ImGuiHelpers.GlobalScale);

            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();
        }

        private void CurrentStatus()
        {
            ImGui.Text("ZoneID :: ZoneName");

            if (Settings.Territories.Count > 0)
            {
                if (Settings.Territories.Count > 5)
                {
                    ImGui.BeginChild("TerritoryBlacklistChild", ImGuiHelpers.ScaledVector2(0, 150), true);
                }

                foreach (var territory in Settings.Territories)
                {
                    ImGui.Text(territory.ToString());
                }

                if (Settings.Territories.Count > 5)
                {
                    ImGui.EndChild();
                }
            }
            else
            {
                ImGui.TextColored(new(180, 0, 0, 0.8f), "Blacklist is empty");
            }

            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();
        }

        private void CurrentZone()
        {
            ImGui.PushID("CurrentZone");

            var currentTerritoryID = Service.ClientState.TerritoryType;
            var territoryInfo = new SimpleTerritory(currentTerritoryID);

            ImGui.Text($"Currently in {territoryInfo}");
            ImGui.Spacing();

            if (ImGui.Button("Add", ImGuiHelpers.ScaledVector2(60, 23)))
            {
                Add(currentTerritoryID);
            }

            ImGui.SameLine();

            if (ImGui.Button("Remove", ImGuiHelpers.ScaledVector2(60, 23)))
            {
                Remove(currentTerritoryID);
            }

            ImGuiComponents.HelpMarker("Adds or Removes the current map to or from the blacklist.");

            ImGui.Spacing();

            ImGui.PopID();
        }

        private void ManualNumeric()
        {
            ImGui.PushID("Manual Numeric");

            ImGui.Separator();

            ImGui.Text("Add By Zone ID");
            ImGui.Spacing();

            ImGui.PushItemWidth(50 * ImGuiHelpers.GlobalScale);
            ImGui.InputInt("##AddToBlacklist", ref modifyBlacklistValue, 0, 0);
            ImGui.PopItemWidth();

            ImGui.SameLine();

            if (ImGui.Button("Add", ImGuiHelpers.ScaledVector2(75, 23)))
            {
                Add((uint)modifyBlacklistValue);
            }

            ImGui.SameLine();

            if (ImGui.Button("Remove", ImGuiHelpers.ScaledVector2(75, 23)))
            {
                Remove((uint)modifyBlacklistValue);
            }

            ImGui.SameLine();

            ImGuiComponents.HelpMarker("Adds or Removes specified map to or from the blacklist");

            ImGui.Spacing();

            ImGui.PopID();
        }

        private void NameLookup()
        {
            ImGui.PushID("NameLookup");

            ImGui.Separator();
            ImGui.Text("Add or Remove by DutyFinder Name");
            ImGui.Spacing();

            ImGui.SetNextItemWidth(230.0f * ImGuiHelpers.GlobalScale);
            if (ImGui.BeginCombo("##ContentTypeSelection", selectedContentTypeString))
            {
                foreach (var name in contentTypeList)
                {
                    bool isSelected = name == selectedContentTypeString;
                    if (ImGui.Selectable(name, isSelected))
                    {
                        selectedContentTypeString = name;
                        instanceNames = contentFinderConditionList
                            .Where(c => c.ContentType.Value != null)
                            .Where(c => c.Name != null)
                            .Where(c => c.ContentType.Value!.Name == selectedContentTypeString)
                            .Select(c => c.Name.ToString())
                            .ToHashSet();
                    }

                    if (isSelected)
                    {
                        ImGui.SetItemDefaultFocus();
                    }
                }

                ImGui.EndCombo();
            }
            ImGui.Spacing();

            if (selectedContentTypeString != "")
            {
                ImGui.SetNextItemWidth(230.0f * ImGuiHelpers.GlobalScale);
                if (ImGui.BeginCombo("##TerritorySelectByName", selectedInstanceName))
                {

                    foreach (var instanceName in instanceNames)
                    {
                        bool isSelected = instanceName == selectedInstanceName;
                        if (ImGui.Selectable(instanceName, isSelected))
                        {
                            selectedInstanceName = instanceName;
                        }

                        if (isSelected)
                        {
                            ImGui.SetItemDefaultFocus();
                        }
                    }

                    ImGui.EndCombo();
                }

                ImGui.Spacing();


                if (ImGui.Button("Add", ImGuiHelpers.ScaledVector2(111, 25)))
                {
                    var instanceId = contentFinderConditionList
                        .Where(c => c.Name != null)
                        .First(c => c.Name.ToString() == selectedInstanceName)
                        .TerritoryType.Value!.RowId;

                    Add(instanceId);
                }

                ImGui.SameLine();

                if (ImGui.Button("Remove", ImGuiHelpers.ScaledVector2(111, 25)))
                {
                    var instanceId = contentFinderConditionList
                        .Where(c => c.Name != null)
                        .First(c => c.Name.ToString() == selectedInstanceName)
                        .TerritoryType.Value!.RowId;

                    Remove(instanceId);
                }
            }

            ImGui.Spacing();

            ImGui.PopID();
        }

        private void Add(uint id)
        {
            var blacklist = Settings.Territories;

            var territoryInfo = new SimpleTerritory(id);

            if (!blacklist.Contains(territoryInfo))
            {
                blacklist.Add(territoryInfo);
                Service.Configuration.Save();
            }
        }

        private void Remove(uint id)
        {
            var blacklist = Settings.Territories;

            var territoryInfo = new SimpleTerritory(id);

            if (blacklist.Contains(territoryInfo))
            {
                blacklist.Remove(territoryInfo);
                Service.Configuration.Save();
            }
        }
    }
}
