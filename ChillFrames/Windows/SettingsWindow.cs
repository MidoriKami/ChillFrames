using System;
using System.Drawing;
using System.Linq;
using System.Numerics;
using ChillFrames.Classes;
using ChillFrames.Controllers;
using Dalamud.Interface;
using Dalamud.Interface.Components;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using ImGuiNET;
using KamiLib.Classes;
using KamiLib.CommandManager;
using KamiLib.Extensions;
using KamiLib.Window;
using KamiLib.Window.SelectionWindows;
using Lumina.Excel.GeneratedSheets;
using Window = KamiLib.Window.Window;

namespace ChillFrames.Windows;

public class SettingsWindow : Window {
    private int idleFramerateLimitTemp = int.MinValue;
    private int activeFramerateLimitTemp = int.MinValue;
    private Configuration Config => System.Config;

    private readonly TabBar tabBar = new("ChillFramesSettingsTabBar", [
        new LimiterSettingsTab(),
        new DtrSettingsTab(),
        new BlacklistTab(),
    ]);

    public SettingsWindow() : base("ChillFrames Settings", new Vector2(500.0f, 425.0f)) {
        Flags |= ImGuiWindowFlags.NoScrollbar;
        Flags |= ImGuiWindowFlags.NoScrollWithMouse;
        
        System.CommandManager.RegisterCommand(new CommandHandler {
            Delegate = OpenConfigWindow,
            ActivationPath = "/",
        });
    }

    public override void PreDraw() {
        if (idleFramerateLimitTemp is int.MinValue) idleFramerateLimitTemp = System.Config.Limiter.IdleFramerateTarget;
        if (activeFramerateLimitTemp is int.MinValue) activeFramerateLimitTemp = System.Config.Limiter.ActiveFramerateTarget;
    }

    protected override void DrawContents() {
        DrawLimiterStatus();
        tabBar.Draw();
    }
    
    private void DrawLimiterStatus() {
        using var statusTable = ImRaii.Table("status_table", 2);
        if (!statusTable) return;
        
        ImGui.TableNextColumn();
        ImGui.Text($"Current Framerate");

        ImGui.TableNextColumn();
        ImGui.Text($"{1000 / FrameLimiterController.LastFrametime.TotalMilliseconds:F} fps");

        ImGui.TableNextColumn();
        if (System.BlockList.Count > 0) {
            if (ImGuiComponents.IconButton("##ReleaseLocks", FontAwesomeIcon.Unlock)) {
                System.BlockList.Clear();
            }
            if (ImGui.IsItemHovered()) {
                ImGui.SetTooltip("Remove limiter lock");
            }

            ImGui.SameLine();
            ImGuiHelpers.SafeTextColoredWrapped(KnownColor.Red.Vector(), $"Limiter is inactive - requested by plugin(s): {string.Join(", ", System.BlockList)}");
            ImGui.TableNextColumn();
        }
        else if (FrameLimiterCondition.IsBlacklisted) {
            ImGui.TextColored(KnownColor.Red.Vector(), "Limiter Inactive, In Blacklisted Zone");
            ImGui.TableNextColumn();
        }
        else if (!FrameLimiterCondition.DisableFramerateLimit() && Config.PluginEnable) {
            ImGui.Text($"Target Framerate");
            ImGui.TableNextColumn();
            ImGui.Text($"{Config.Limiter.IdleFramerateTarget} fps");
        }
        else if (FrameLimiterCondition.DisableFramerateLimit() && Config.PluginEnable) {
            ImGui.Text($"Target Framerate");
            ImGui.TableNextColumn();
            ImGui.Text($"{Config.Limiter.ActiveFramerateTarget} fps");
        }
        else {
            ImGui.TextColored(KnownColor.Red.Vector(), "Limiter Inactive");
            ImGui.TableNextColumn();
        }
    }

    private void OpenConfigWindow(params string[] args) => Toggle();
}

public class LimiterSettingsTab : ITabItem {
    public string Name => "Limiter Settings";
    
    public bool Disabled => false;
    
    private string LowerLimitString => $"Use Lower Limit ( {System.Config.Limiter.IdleFramerateTarget} fps )";
    
    private string UpperLimitString => $"Use Upper Limit ( {System.Config.Limiter.ActiveFramerateTarget} fps )";

    public void Draw() {
        DrawFpsLimitOptions();
        ImGuiHelpers.ScaledDummy(5.0f);
        DrawLimiterOptions();
    }
    
    private static void DrawFpsLimitOptions() {
        using var fpsInputTable = ImRaii.Table("fps_input_settings", 2);
        if (!fpsInputTable) return;
        
        ImGui.TableNextColumn();
        ImGui.AlignTextToFramePadding();
        ImGui.Text("Lower Limit");
        ImGui.SameLine();
        ImGui.PushItemWidth(ImGui.GetContentRegionAvail().X * 0.75f);
        var idleLimit = System.Config.Limiter.IdleFramerateTarget;
        ImGui.InputInt("##LowerLimit", ref idleLimit, 0);
        if (ImGui.IsItemDeactivatedAfterEdit()) {
            System.Config.Limiter.IdleFramerateTarget = Math.Clamp(idleLimit, 1, System.Config.Limiter.ActiveFramerateTarget);
            System.Config.Save();
        }

        ImGui.TableNextColumn();
        ImGui.AlignTextToFramePadding();
        ImGui.Text("Upper Limit");
        ImGui.SameLine();
        ImGui.PushItemWidth(ImGui.GetContentRegionAvail().X * 0.75f);
        var activeLimit = System.Config.Limiter.ActiveFramerateTarget;
        ImGui.InputInt("##UpperLimit", ref activeLimit, 0);
        if (ImGui.IsItemDeactivatedAfterEdit()) {
            System.Config.Limiter.ActiveFramerateTarget = Math.Clamp(activeLimit, System.Config.Limiter.IdleFramerateTarget, 1000);
            System.Config.Save();
        }
    }

    private void DrawLimiterOptions() {
        using var table = ImRaii.Table("limiter_options_table", 3);
        if (!table) return;
        
        ImGui.TableSetupColumn("Condition", ImGuiTableColumnFlags.WidthFixed, 150.0f * ImGuiHelpers.GlobalScale);
        ImGui.TableSetupColumn("Status", ImGuiTableColumnFlags.WidthFixed, 75.0f * ImGuiHelpers.GlobalScale);
        ImGui.TableSetupColumn("When Condition Active", ImGuiTableColumnFlags.WidthStretch);
            
        ImGui.TableHeadersRow();
            
        foreach (var option in System.LimiterOptions) {
            DrawOption(option);
        }
    }

    private void DrawOption(IFrameLimiterOption option) {
        ImGui.TableNextColumn();
        ImGui.Text(option.Label);

        ImGui.TableNextColumn();
        if (option.Active) {
            ImGui.TextColored(KnownColor.Green.Vector(), "Active");
        }
        else {
            ImGui.TextColored(KnownColor.OrangeRed.Vector(), "Inactive");
        }

        ImGui.TableNextColumn();
        ImGui.PushItemWidth(ImGui.GetContentRegionAvail().X);

        DrawOptionCombo(option);
    }
    
    private void DrawOptionCombo(IFrameLimiterOption option) {
        using var combo = ImRaii.Combo($"##OptionCombo_{option.Label}", option.Enabled ? UpperLimitString : LowerLimitString);
        if (!combo) return;
        
        if (ImGui.Selectable(UpperLimitString, option.Enabled)) {
            option.Enabled = true;
            System.Config.Save();
        }

        if (ImGui.Selectable(LowerLimitString, !option.Enabled)) {
            option.Enabled = false;
            System.Config.Save();
        }
    }
}

public class DtrSettingsTab : ITabItem {
    public string Name => "DTR Entry";
    
    public bool Disabled => false;
    
    public void Draw() {
        if (ImGui.Checkbox("Enable", ref System.Config.General.EnableDtrBar)) {
            System.DtrController.UpdateEnabled();
            System.Config.Save();
        }

        if (ImGui.Checkbox("Show Color", ref System.Config.General.EnableDtrColor)) {
            System.Config.Save();
        }

        ImGuiHelpers.ScaledDummy(5.0f);

        if (ImGuiTweaks.UiColorPicker("Enabled Color", System.Config.General.EnabledUiColor)) {
            System.WindowManager.AddWindow(new UiColorSelectionWindow(Service.DataManager) {
                SingleSelectionCallback = selection => {
                    if (selection is null) return;
                    
                    System.Config.General.EnabledColor = (ushort)selection.RowId;
                    System.Config.Save();
                },
            }, WindowFlags.OpenImmediately);
        }
        
        if (ImGuiTweaks.UiColorPicker("Disabled Color", System.Config.General.DisabledUiColor)) {
            System.WindowManager.AddWindow(new UiColorSelectionWindow(Service.DataManager) {
                SingleSelectionCallback = selection => {
                    if (selection is null) return;
                    
                    System.Config.General.DisabledColor = (ushort)selection.RowId;
                    System.Config.Save();
                },
            }, WindowFlags.OpenImmediately);
        }
    }
}

public class BlacklistTab : ITabItem {
    public string Name => "Zone Blacklist";
    
    public bool Disabled => false;

    public void Draw() {
        using (var _ = ImRaii.PushColor(ImGuiCol.ChildBg, KnownColor.OrangeRed.Vector() with { W = 0.15f }, System.Config.Blacklist.BlacklistedZones.Contains(Service.ClientState.TerritoryType))) {
            DrawAddRemovableTerritory(GetCurrentTerritory());
        }
        
        ImGui.Separator();
        
        DrawCurrentlyBlacklisted();
        
        DrawAddNewButton();
    }
    
    private void DrawAddRemovableTerritory(TerritoryType territory) {
        using (var _ = ImRaii.PushFont(UiBuilder.IconFont)) {
            if (System.Config.Blacklist.BlacklistedZones.Contains(territory.RowId)) {
                if (ImGui.Button($"{FontAwesomeIcon.Trash.ToIconString()}##removeZone{territory.RowId}", new Vector2(25.0f, 75.0f))) {
                    System.Config.Blacklist.BlacklistedZones.Remove(territory.RowId);
                    System.Config.Save();
                }
            }
            else {
                if (ImGui.Button($"{FontAwesomeIcon.Plus.ToIconString()}##addZone{territory.RowId}", new Vector2(25.0f, 75.0f))) {
                    System.Config.Blacklist.BlacklistedZones.Add(territory.RowId);
                    System.Config.Save();
                }
            }
        }
        
        ImGui.SameLine();
        
        territory.Draw(Service.DataManager, Service.TextureProvider);
    }
    
    private void DrawCurrentlyBlacklisted() {
        using var child = ImRaii.Child("blacklist_frame", new Vector2(ImGui.GetContentRegionAvail().X, ImGui.GetContentRegionAvail().Y - 25.0f * ImGuiHelpers.GlobalScale));
        if (!child) return;

        ImGuiClip.ClippedDraw(System.Config.Blacklist.BlacklistedZones.ToList(), zoneId => {
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
            System.WindowManager.AddWindow(new TerritorySelectionWindow(Service.PluginInterface) {
                MultiSelectionCallback = selections => {
                    foreach (var selection in selections) {
                        System.Config.Blacklist.BlacklistedZones.Add(selection.RowId);
                        System.Config.Save();
                    }
                }
            });
        }
    }

    private static TerritoryType GetCurrentTerritory() 
        => Service.DataManager.GetExcelSheet<TerritoryType>()!.GetRow(Service.ClientState.TerritoryType)!;
}