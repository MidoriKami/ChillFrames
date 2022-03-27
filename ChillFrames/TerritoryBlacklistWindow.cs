using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private readonly List<ContentFinderCondition> ContentFinderConditionList;
        private readonly HashSet<string> ContentTypeList;
        private HashSet<string> InstanceNames;
        private string SelectedContentTypeString = "";
        private string SelectedInstanceName = "";
        public TerritoryBlacklistWindow() : base("Chill Frames Territory Blacklist", ImGuiWindowFlags.AlwaysAutoResize)
        {
            Service.WindowSystem.AddWindow(this);

            ContentFinderConditionList = Service.DataManager.GetExcelSheet<ContentFinderCondition>()!.ToList();

            ContentTypeList = ContentFinderConditionList
                !.Where(c => c.ContentType.Value?.Name != null)
                .Select(c => c.ContentType.Value!.Name.ToString())
                .ToHashSet();

            //SizeConstraints = new WindowSizeConstraints()
            //{
            //    MinimumSize = new(300, 250),
            //    MaximumSize = new(300, 250)
            //};
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
            if (ImGui.BeginCombo("##ContentTypeSelection", SelectedContentTypeString))
            {
                foreach (var name in ContentTypeList)
                {
                    bool isSelected = name == SelectedContentTypeString;
                    if (ImGui.Selectable(name, isSelected))
                    {
                        SelectedContentTypeString = name;
                        InstanceNames = ContentFinderConditionList
                            .Where(c => c.ContentType.Value != null)
                            .Where(c => c.Name != null)
                            .Where(c =>  c.ContentType.Value!.Name == SelectedContentTypeString)
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

            if (SelectedContentTypeString != "")
            {
                ImGui.SetNextItemWidth(230.0f * ImGuiHelpers.GlobalScale);
                if (ImGui.BeginCombo("##TerritorySelectByName", SelectedInstanceName))
                {

                    foreach (var instanceName in InstanceNames)
                    {
                        bool isSelected = instanceName == SelectedInstanceName;
                        if (ImGui.Selectable(instanceName, isSelected))
                        {
                            SelectedInstanceName = instanceName;
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

                    var instanceId = ContentFinderConditionList
                        .Where(c => c.Name != null)
                        .First(c => c.Name.ToString() == SelectedInstanceName)
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

                    var instanceId = ContentFinderConditionList
                        .Where(c => c.Name != null)
                        .First(c => c.Name.ToString() == SelectedInstanceName)
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
