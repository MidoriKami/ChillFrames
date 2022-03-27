using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Interface;
using Dalamud.Interface.Components;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;

namespace ChillFrames
{
    internal class TerritoryBlacklistWindow : Window, IDisposable
    {
        private int modifyBlacklistValue = 0;

        private readonly List<ContentFinderCondition> contentFinderConditionList;
        private readonly HashSet<string> contentTypeList;
        private HashSet<string> instanceNames = new();
        private string selectedContentTypeString = "";
        private string selectedInstanceName = "";

        public TerritoryBlacklistWindow() : base("Chill Frames Territory Blacklist", ImGuiWindowFlags.AlwaysAutoResize)
        {
            Service.WindowSystem.AddWindow(this);

            contentFinderConditionList = Service.DataManager.GetExcelSheet<ContentFinderCondition>()!.ToList();

            contentTypeList = contentFinderConditionList
                !.Where(c => c.ContentType.Value?.Name != null)
                .Select(c => c.ContentType.Value!.Name.ToString())
                .ToHashSet();

        }

        public void Dispose()
        {
            Service.WindowSystem.RemoveWindow(this);
        }

        public override void Draw()
        {
            if (IsOpen == false) return;

            PrintCurrentStatus();

            PrintAddRemoveCurrentTerritoryBlacklist();

            PrintAddRemoveManualTerritoryBlacklist();

            PrintAddRemoveManualWithNameTerritoryWhitelist();
        }

        private void PrintCurrentStatus()
        {
            ImGui.Text("Currently Blacklisted Areas");
            ImGui.Spacing();

            if (Service.Configuration.TerritoryBlacklist.Count > 0)
            {
                var blacklist = Service.Configuration.TerritoryBlacklist;
                ImGui.Text("{" + string.Join(", ", blacklist) + "}");
            }
            else
            {
                ImGui.TextColored(new (180, 0, 0, 0.8f), "Blacklist is empty");
            }
            ImGui.Spacing();
        }

        private static void PrintAddRemoveCurrentTerritoryBlacklist()
        {
            ImGui.Separator();
            ImGui.Spacing();

            ImGui.Text($"Currently in MapID: [{Service.ClientState.TerritoryType}]");
            ImGui.Spacing();

            if (ImGui.Button("Add", ImGuiHelpers.ScaledVector2(60, 23)))
            {
                var blacklist = Service.Configuration.TerritoryBlacklist;

                if (!blacklist.Contains(Service.ClientState.TerritoryType))
                {
                    blacklist.Add(Service.ClientState.TerritoryType);
                }
            }

            ImGui.SameLine();

            if (ImGui.Button("Remove", ImGuiHelpers.ScaledVector2(60, 23)))
            {
                var blacklist = Service.Configuration.TerritoryBlacklist;

                if (blacklist.Contains(Service.ClientState.TerritoryType))
                {
                    blacklist.Remove(Service.ClientState.TerritoryType);
                }
            }

            ImGuiComponents.HelpMarker("Adds or Removes the current map to or from the blacklist.");

            ImGui.Spacing();
        }

        private void PrintAddRemoveManualTerritoryBlacklist()
        {
            ImGui.Text("Manually Add or Remove");
            ImGui.Spacing();

            ImGui.PushItemWidth(50 * ImGuiHelpers.GlobalScale);
            ImGui.InputInt("##AddToBlacklist", ref modifyBlacklistValue, 0, 0);
            ImGui.PopItemWidth();

            ImGui.SameLine();

            if (ImGui.Button("Add", ImGuiHelpers.ScaledVector2(75, 23)))
            {
                AddToBlacklist((uint)modifyBlacklistValue);
            }

            ImGui.SameLine();

            if (ImGui.Button("Remove", ImGuiHelpers.ScaledVector2(75, 23)))
            {
                RemoveFromBlacklist((uint)modifyBlacklistValue);
            }

            ImGui.SameLine();

            ImGuiComponents.HelpMarker("Adds or Removes specified map to or from the blacklist");

            ImGui.Spacing();
        }

        private void PrintAddRemoveManualWithNameTerritoryWhitelist()
        {

            ImGui.Text("Add or Remove by DutyFinder Name");
            ImGui.Separator();
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
                            .Where(c =>  c.ContentType.Value!.Name == selectedContentTypeString)
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


                if (ImGui.Button("Add Instance", ImGuiHelpers.ScaledVector2(111, 25)))
                {
                    var whitelist = Service.Configuration.TerritoryBlacklist;

                    var instanceId = contentFinderConditionList
                        .Where(c => c.Name != null)
                        .First(c => c.Name.ToString() == selectedInstanceName)
                        .TerritoryType.Value!.RowId;

                    if (!whitelist.Contains(instanceId))
                    {
                        whitelist.Add(instanceId);
                    }
                }

                ImGui.SameLine();

                if (ImGui.Button("Remove Instance", ImGuiHelpers.ScaledVector2(111, 25)))
                {
                    var whitelist = Service.Configuration.TerritoryBlacklist;

                    var instanceId = contentFinderConditionList
                        .Where(c => c.Name != null)
                        .First(c => c.Name.ToString() == selectedInstanceName)
                        .TerritoryType.Value!.RowId;

                    if (whitelist.Contains(instanceId))
                    {
                        whitelist.Remove(instanceId);
                    }
                }
            }

            ImGui.Spacing();

        }

        private static void RemoveFromBlacklist(uint territory)
        {
            var blacklist = Service.Configuration.TerritoryBlacklist;

            if (blacklist.Contains(territory))
            {
                blacklist.Remove(territory);
            }
        }

        private static void AddToBlacklist(uint territory)
        {
            var blacklist = Service.Configuration.TerritoryBlacklist;

            if (!blacklist.Contains(territory))
            {
                blacklist.Add(territory);
            }
        }
    }
}
