using System;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.Gui.Dtr;
using Dalamud.Plugin.Services;
using Dalamud.Utility;
using Lumina.Text.ReadOnly;

namespace ChillFrames.Controllers;

public class DtrController : IDisposable {
	private readonly IDtrBarEntry dtrEntry;

	public DtrController() {
		dtrEntry = IDtrBar.Get().Get("Chill Frames");

		dtrEntry.Tooltip = GetTooltip();
		dtrEntry.OnClick = DtrOnClick;

		dtrEntry.Shown = System.Config.General.EnableDtrBar;
		dtrEntry.MinimumWidth = 60;
	}

	public void Dispose()
		=> dtrEntry.Remove();

	private void DtrOnClick(DtrInteractionEvent dtrInteractionEvent) {
		switch (dtrInteractionEvent.ClickType) {
			case MouseClickType.Left:
				if (ICondition.Get().Any(ConditionFlag.InCombat)) return;
				System.Config.PluginEnable = !System.Config.PluginEnable;
				System.Config.Save();
				break;

			case MouseClickType.Right:
				System.ConfigWindow.Toggle();
				break;
		}

		dtrEntry.Tooltip = GetTooltip();
	}

	public void Update() {
		using var stringBuilder = new RentedSeStringBuilder();

		if (System.Config.General.EnableDtrColor) {
			dtrEntry.Text = stringBuilder
			                .PushColorRgba(System.Config.PluginEnable ? System.Config.General.ActiveColor : System.Config.General.InactiveColor)
			                .Append($"{1000 / FrameLimiterController.LastFrametime.TotalMilliseconds:N0} FPS")
			                .PopColor()
			                .ToReadOnlySeString();
		}
		else {
			dtrEntry.Text = $"{1000 / FrameLimiterController.LastFrametime.TotalMilliseconds:N0} FPS";
		}
	}

	private static ReadOnlySeString GetTooltip()
		=> $"{(System.Config.PluginEnable ? "Left Click to Disable Limiter" : "Left Click to Enable Limiter")}\n" +
		   $"Right click to open Config Window";

	public void UpdateEnabled()
		=> dtrEntry.Shown = System.Config.General.EnableDtrBar;
}
