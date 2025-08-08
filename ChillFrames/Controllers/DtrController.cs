using System;
using ChillFrames.Windows;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.Gui.Dtr;
using Dalamud.Game.Text.SeStringHandling;
    
namespace ChillFrames.Controllers;

public class DtrController : IDisposable {
    private readonly IDtrBarEntry dtrEntry;
    
    public DtrController() {
        dtrEntry = Service.DtrBar.Get("Chill Frames");

        dtrEntry.Tooltip = GetTooltip();
        dtrEntry.OnClick = DtrOnClick;

        dtrEntry.Shown = System.Config.General.EnableDtrBar;
    }
    
    public void Dispose() 
        => dtrEntry.Remove();

    private void DtrOnClick(DtrInteractionEvent dtrInteractionEvent) {
        switch (dtrInteractionEvent.ClickType) {
            case MouseClickType.Left:
                if (Service.Condition.Any(ConditionFlag.InCombat)) return;
                System.Config.PluginEnable = !System.Config.PluginEnable;
                System.Config.Save();
                break;
            
            case MouseClickType.Right:
                System.WindowManager.GetWindow<SettingsWindow>()?.Toggle();
                break;
        }

        dtrEntry.Tooltip = GetTooltip();
    }

    public void Update() {
        if (System.Config.General.EnableDtrColor) {
            dtrEntry.Text = new SeStringBuilder()
                .AddUiForeground(System.Config.PluginEnable ? System.Config.General.EnabledColor : System.Config.General.DisabledColor)
                .AddText($"{1000 / FrameLimiterController.LastFrametime.TotalMilliseconds:N0} FPS")
                .AddUiForegroundOff()
                .Build();
        }
        else {
            dtrEntry.Text = $"{1000 / FrameLimiterController.LastFrametime.TotalMilliseconds:N0} FPS";
        }
    }

    private SeString GetTooltip()
        => $"{(System.Config.PluginEnable ? "Left Click to Disable Limiter" : "Left Click to Enable Limiter")}\n" +
           $"Right click to open Config Window";

    public void UpdateEnabled() 
        => dtrEntry.Shown = System.Config.General.EnableDtrBar;
}