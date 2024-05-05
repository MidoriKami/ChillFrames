using System;
using System.Drawing;
using Dalamud.Game.Gui.Dtr;
using Dalamud.Interface;
using Dalamud.Utility;
using Lumina.Text;

namespace ChillFrames.Controllers;

public class DtrController : IDisposable {
    private readonly DtrBarEntry dtrEntry;
    
    public DtrController() {
        dtrEntry = Service.DtrBar.Get("Chill Frames");

        dtrEntry.Tooltip = ChillFramesSystem.Config.PluginEnable ? "Click to Disable Limiter" : $"Click to Enable Limiter";
        dtrEntry.OnClick = DtrOnClick;

        dtrEntry.Shown = ChillFramesSystem.Config.General.EnableDtrBar;
    }
    
    public void Dispose() 
        => dtrEntry.Dispose();

    private void DtrOnClick() {
        ChillFramesSystem.Config.PluginEnable = !ChillFramesSystem.Config.PluginEnable;
        ChillFramesSystem.Config.Save();
        
        dtrEntry.Tooltip = ChillFramesSystem.Config.PluginEnable ? "Click to Disable Limiter" : $"Click to Enable Limiter";
    }

    public void Update() {
        if (ChillFramesSystem.Config.General.EnableDtrColor) {
            dtrEntry.Text = new SeStringBuilder()
                .PushColorRgba(ChillFramesSystem.Config.PluginEnable ? ChillFramesSystem.Config.General.EnabledColor : ChillFramesSystem.Config.General.DisabledColor)
                .Append($"{1000 / FrameLimiterController.LastFrametime.TotalMilliseconds:N0} FPS")
                .PopColor()
                .ToSeString()
                .ToDalamudString();
        }
        else {
            dtrEntry.Text = $"{1000 / FrameLimiterController.LastFrametime.TotalMilliseconds:N0} FPS";
        }
    }

    public void UpdateEnabled() 
        => dtrEntry.Shown = ChillFramesSystem.Config.General.EnableDtrBar;
}