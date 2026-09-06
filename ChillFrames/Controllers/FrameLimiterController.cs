using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using ChillFrames.Classes;
using ChillFrames.Utilities;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.System.Framework;

namespace ChillFrames.Controllers;

public class FrameLimiterController : IDisposable {
	private readonly Stopwatch steppingStopwatch = Stopwatch.StartNew();
	private readonly Stopwatch timer = Stopwatch.StartNew();
	private readonly Stopwatch windowIdleTimer = new();
	private float delayRatio = 1.0f;
	private bool enabledLastFrame;
	private bool idleLimiterDisabled;

	private LimiterState state;

	private static LimiterSettings Settings => System.Config.Limiter;

	private static int TargetLowerFramerate => Settings.LowerFramerateTarget;
	private static int TargetLowerFrametime => 1000 / TargetLowerFramerate;
	private static int PreciseLowerFrametime => (int) (1000.0f / TargetLowerFramerate * 10000);

	private static int TargetBaseFramerate => Settings.BaseFramerateTarget;
	private static int TargetBaseFrametime => 1000 / TargetBaseFramerate;
	private static int PreciseBaseFrametime => (int) (1000.0f / TargetBaseFramerate * 10000);

	private static int TargetUpperFramerate => Settings.UpperFramerateTarget;
	private static int TargetUpperFrametime => 1000 / TargetUpperFramerate;
	private static int PreciseUpperFrametime => (int) (1000.0f / TargetUpperFramerate * 10000);

	private static float DisableIncrement => System.Config.DisableIncrementSetting;
	private static float EnableIncrement => System.Config.EnableIncrementSetting;

	public static TimeSpan LastFrametime { get; private set; }

	private static unsafe bool IsWindowInactive
		=> Framework.Instance()->WindowInactive;

	public FrameLimiterController()
		=> IFramework.Get().Update += OnFrameworkUpdate;

	public void Dispose()
		=> IFramework.Get().Update -= OnFrameworkUpdate;

	private void OnFrameworkUpdate(IFramework framework) {
		UpdateState();

		UpdateRate();

		TryLimitFramerate();

		LastFrametime = timer.Elapsed;
		timer.Restart();

		if (System.Config.General.EnableDtrBar) {
			System.DtrController.Update();
		}
	}

	[MethodImpl(MethodImplOptions.NoOptimization)]
	private void TryLimitFramerate() {
		if (!System.Config.PluginEnable) return;

		TryDelayIdleFpsActivation();
		if (TryDisableIdleFpsInLoadingAreas()) return;

		var targetState = FrameLimiterCondition.GetTargetState();

		switch (targetState) {
			case LimiterStateTarget.UpperLimit when state is LimiterState.SteadyState:
				PerformLimiting(TargetUpperFrametime, PreciseUpperFrametime);
				break;

			case LimiterStateTarget.LowerLimit:
				PerformLimiting(TargetLowerFrametime, PreciseLowerFrametime);
				break;

			case LimiterStateTarget.BaseLimit:
			default:
				PerformLimiting(TargetBaseFrametime, PreciseBaseFrametime);
				break;
		}
	}

	private void PerformLimiting(int targetFrametime, int preciseFrameTickTime) {
		var delayTime = (int) (targetFrametime - timer.ElapsedMilliseconds);

		if (delayTime - 1 > 0) {
			Thread.Sleep(delayTime - 1);
		}

		while (timer.ElapsedTicks <= preciseFrameTickTime) {
			((Action) (() => { }))();
		}
	}

	private void UpdateState() {
		var shouldLimit = FrameLimiterCondition.GetTargetState() is not LimiterStateTarget.UpperLimit;

		if (enabledLastFrame != shouldLimit) {
			state = enabledLastFrame switch {
				true => LimiterState.Disabled,
				false => LimiterState.Enabled,
			};
		}

		enabledLastFrame = shouldLimit;
	}

	private void UpdateRate() {
		const int stepDelay = 40;

		if (steppingStopwatch.ElapsedMilliseconds > stepDelay) {
			switch (state) {
				case LimiterState.Enabled when delayRatio < 1.0f:
					delayRatio += EnableIncrement;
					break;

				case LimiterState.Enabled when delayRatio >= 1.0f:
					state = LimiterState.SteadyState;
					delayRatio = 1.0f;
					break;

				case LimiterState.Disabled when delayRatio > 0.0f:
					delayRatio -= DisableIncrement;
					break;

				case LimiterState.Disabled when delayRatio <= 0.0f:
					delayRatio = 0.0f;
					state = LimiterState.SteadyState;
					break;

				case LimiterState.SteadyState:
					break;
			}

			steppingStopwatch.Restart();
		}
	}

	private bool TryDisableIdleFpsInLoadingAreas() {

		// Disable IdleFPS Limiter while in loading areas.
		if (ICondition.Get().IsBetweenAreas || IFramework.Get().IsFrameworkUnloading) {
			if (!idleLimiterDisabled) {
				System.IdleFpsController.SetWaitTime(0);
				idleLimiterDisabled = true;
			}
			return true;
		}

		// Then re-enable it when we're out of the loading area.
		if (idleLimiterDisabled) {
			System.IdleFpsController.SetWaitTime(System.Config.IdleFpsWaitTime);
			idleLimiterDisabled = false;
		}

		return false;
	}

	private void TryDelayIdleFpsActivation() {

		// If the window isn't idle, restart timer.
		if (!IsWindowInactive) {

			// And set the idle time to 0, so we don't enable the idle limiter immediately after going idle.
			System.IdleFpsController.SetWaitTime(0);
			windowIdleTimer.Restart();
		}

		// The window is idle
		else {

			// And we have waited long enough to enable the idle limiter.
			if (windowIdleTimer.Elapsed > TimeSpan.FromSeconds(System.Config.IdleFpsDelayTime)) {
				System.IdleFpsController.SetWaitTime(System.Config.IdleFpsWaitTime);
			}
		}
	}
}