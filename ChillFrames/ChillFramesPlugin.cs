using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ChillFrames.Classes;
using ChillFrames.Controllers;
using ChillFrames.Utilities;
using ChillFrames.Windows;
using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace ChillFrames;

public sealed class ChillFramesPlugin : IAsyncDalamudPlugin {
	[PluginService] internal static IDalamudPluginInterface PluginInterface { get; set; } = null!;

	public Task LoadAsync(CancellationToken cancellationToken) {
		// We need to disable these, so users can monitor the config window and see what conditions are active at what times.
		PluginInterface.UiBuilder.DisableCutsceneUiHide = true;
		PluginInterface.UiBuilder.DisableAutomaticUiHide = true;
		PluginInterface.UiBuilder.DisableGposeUiHide = true;
		PluginInterface.UiBuilder.DisableUserUiHide = true;

		System.LimiterOptions = GetFrameLimiterOptions();

		System.Config = Configuration.Load();

		System.IdleFpsController = new IdleFpsController();
		System.DtrController = new DtrController();
		System.FrameLimiterController = new FrameLimiterController();
		ICommandManager.Get().AddHandler("/chillframes", new CommandInfo(OnCommand) {
			ShowInHelp = true, HelpMessage = "Open ChillFrames Config",
		});

		ICommandManager.Get().AddHandler("/pcf", new CommandInfo(OnCommand) {
			ShowInHelp = true, HelpMessage = "Open ChillFrames Config",
		});

		System.WindowSystem = new WindowSystem("ChillFrames");
		System.ConfigWindow = new SettingsWindow();

		System.WindowSystem.AddWindow(System.ConfigWindow);

		PluginInterface.UiBuilder.Draw += System.WindowSystem.Draw;
		PluginInterface.UiBuilder.OpenConfigUi += System.ConfigWindow.Toggle;
		PluginInterface.UiBuilder.OpenMainUi += System.ConfigWindow.Toggle;

		return Task.CompletedTask;
	}

	public async ValueTask DisposeAsync() {
		PluginInterface.UiBuilder.Draw -= System.WindowSystem.Draw;
		PluginInterface.UiBuilder.OpenConfigUi -= System.ConfigWindow.Toggle;
		PluginInterface.UiBuilder.OpenMainUi -= System.ConfigWindow.Toggle;

		ICommandManager.Get().RemoveHandler("/chillframes");
		ICommandManager.Get().RemoveHandler("/pcf");

		System.FrameLimiterController.Dispose();
		System.WindowSystem.RemoveAllWindows();

		await System.IdleFpsController.DisposeAsync();
	}

	private void OnCommand(string command, string arguments) {
		if (ICondition.Get().IsInCombat) {
			IChatGui.Get().PrintError("Unable to modify ChillFrames config while in combat.");
			return;
		}

		if (command is not ("/chillframes" or "/pcf")) return;

		switch (arguments.Split(' ')) {
			case [""] or []:
				System.ConfigWindow.Toggle();
				break;

			case ["enable"]:
				System.Config.PluginEnable = true;
				break;

			case ["disable"]:
				System.Config.PluginEnable = false;
				break;

			case ["toggle"]:
				System.Config.PluginEnable = !System.Config.PluginEnable;
				break;

			case ["fps", "setlower", { } newLowerLimit]:
				if (!int.TryParse(newLowerLimit, out var newLowTarget) || newLowTarget <= 0) return;
				System.Config.Limiter.LowerFramerateTarget = newLowTarget;
				break;

			case ["fps", "setbase", { } newBaseLimit]:
				if (!int.TryParse(newBaseLimit, out var newBaseTarget) || newBaseTarget <= 0) return;
				System.Config.Limiter.BaseFramerateTarget = newBaseTarget;
				break;

			case ["fps", "setupper", { } newUpperLimit]:
				if (!int.TryParse(newUpperLimit, out var newHighTarget) || newHighTarget <= 0) return;
				System.Config.Limiter.UpperFramerateTarget = newHighTarget;
				break;
		}

		System.Config.Save();
	}

	private static List<IFrameLimiterOption> GetFrameLimiterOptions()
		=> Assembly
		   .GetCallingAssembly()
		   .GetTypes()
		   .Where(type => type.IsAssignableTo(typeof(IFrameLimiterOption)))
		   .Where(type => !type.IsAbstract)
		   .Select(type => (IFrameLimiterOption?) Activator.CreateInstance(type))
		   .OfType<IFrameLimiterOption>()
		   .ToList();
}