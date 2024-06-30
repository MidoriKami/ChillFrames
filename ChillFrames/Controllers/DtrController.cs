using System;
using Dalamud.Game.Gui.Dtr;
using Dalamud.Game.Text.SeStringHandling;
    
namespace ChillFrames.Controllers;

public class DtrController : IDisposable {
    private readonly IDtrBarEntry dtrEntry;
    
    public DtrController() {
        dtrEntry = Service.DtrBar.Get("Chill Frames");

        dtrEntry.Tooltip = System.Config.PluginEnable ? "Click to Disable Limiter" : "Click to Enable Limiter";
        dtrEntry.OnClick = DtrOnClick;

        dtrEntry.Shown = System.Config.General.EnableDtrBar;
    }
    
    public void Dispose() 
        => dtrEntry.Remove();

    private void DtrOnClick() {
        System.Config.PluginEnable = !System.Config.PluginEnable;
        System.Config.Save();
        
        dtrEntry.Tooltip = System.Config.PluginEnable ? "Click to Disable Limiter" : $"Click to Enable Limiter";
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

    public void UpdateEnabled() 
        => dtrEntry.Shown = System.Config.General.EnableDtrBar;
}