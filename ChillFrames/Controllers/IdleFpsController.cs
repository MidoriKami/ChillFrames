using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ChillFrames.Classes;
using ChillFrames.Utilities;
using Dalamud.Plugin.Services;
using Dalamud.Utility;
using Dalamud.Utility.Signatures;
using FFXIVClientStructs.FFXIV.Client.System.Framework;

namespace ChillFrames.Controllers;

public class IdleFpsController : IAsyncDisposable {

	[Signature("74 ?? B9 ?? ?? ?? ?? E8 ?? ?? ?? ?? B0 ?? 48 83 C4")]
	private readonly nint? jumpInstructionAddress = null;
	private MemoryReplacement? waitTimePatch;

	private IdleLimiterState state = IdleLimiterState.PluginDisabled;
	private readonly Stopwatch idleTimeStopwatch = new();

	public IdleFpsController() {
		IGameInteropProvider.Get().InitializeFromAttributes(this);
		ApplyPatch();
	}

	public async ValueTask DisposeAsync() {
		if (waitTimePatch is not null) {
			await waitTimePatch.DisposeAsync();
		}
	}

	public unsafe void Update() {
		var isInLoadingArea = ICondition.Get().IsBetweenAreas || IFramework.Get().IsFrameworkUnloading;
		var isWindowActive = !Framework.Instance()->WindowInactive;

		// Override state to Disabled if plugin was disabled.
		if (!System.Config.PluginEnable && state is not IdleLimiterState.PluginDisabled) {
			IPluginLog.Get().Debug("Plugin Disabled, restoring limiter to native default.");
			SetTargetWaitFps(20);
			state = IdleLimiterState.PluginDisabled;
		}
		else if (System.Config.PluginEnable && state is IdleLimiterState.PluginDisabled) {
			IPluginLog.Get().Debug("Plugin Enabled, disabling idle limiter.");
			SetTargetWaitFps(0);
			state = IdleLimiterState.Waiting;
		}

		switch (state) {

			// If the window is focused, or we are in a loading screen, keep the limiter disabled.
			case IdleLimiterState.Waiting when isWindowActive || isInLoadingArea:
				idleTimeStopwatch.Restart();
				break;

			// Only enable limiter if we aren't in a loading area, don't have the game focused, and it's been long enough.
			case IdleLimiterState.Waiting when
				!isInLoadingArea &&
				!isWindowActive &&
				idleTimeStopwatch.Elapsed > TimeSpan.FromSeconds(System.Config.IdleFpsDelayTime):

				IPluginLog.Get().Debug($"Window has been idle for {System.Config.IdleFpsDelayTime}s, enabling idle limiter.");
				SetTargetWaitFps(System.Config.IdleFpsTarget);
				state = IdleLimiterState.Limiting;
				break;

			// Window became active again, or we entered a loading area.
			case IdleLimiterState.Limiting when isWindowActive || isInLoadingArea:
				IPluginLog.Get().Debug("Window is active again or entered a loading area, disabling idle limiter.");
				SetTargetWaitFps(0);
				state = IdleLimiterState.Waiting;
				break;
		}
	}

	public void SetTargetWaitFps(int targetFps) {
		if (waitTimePatch is null) return;
		ThreadSafety.AssertMainThread();

		var waitTime = GetWaitTimeForFps(targetFps);
		var bytes = BitConverter.GetBytes(waitTime);
		if (!BitConverter.IsLittleEndian) {
			Array.Reverse(bytes);
		}

		waitTimePatch.UpdateReplacementBytes(bytes);
	}

	public void UpdateWaitTime()
		=> SetTargetWaitFps(System.Config.IdleFpsTarget);

	private void ApplyPatch() {
		if (jumpInstructionAddress is null) return;
		if (jumpInstructionAddress == nint.Zero) return;

		var bytes = BitConverter.GetBytes(GetWaitTimeForFps(System.Config.IdleFpsTarget));
		if (!BitConverter.IsLittleEndian) {
			Array.Reverse(bytes);
		}

		// Address is the address of the jump instruction as the mov doesn't have a valid sig.
		// Offsets by 3 bytes to get to the 32h, which is the value that is modified.
		// .text:00000001400D1F10                 jz      short loc_1400D1F1C <-- Instruction Signature.
		// .text:00000001400D1F12                 mov     ecx, 32h ; '2'  ; dwMilliseconds <-- Edited value
		// .text:00000001400D1F17                 call    j_SleepEx

		waitTimePatch = new MemoryReplacement(jumpInstructionAddress.Value + 3, bytes);

		IFramework.Get().Run(waitTimePatch.Enable);
	}

	// Also compensates the wait time a little to get closer to actual target,
	// as the wait time doesn't normally compensate for how long a frame takes to compute.
	private int GetWaitTimeForFps(int fps) {
		if (fps is 0) return 0;

		var computedWaitTime = (int) (1000.0f / fps);
		if (computedWaitTime < 0) return 0;

		return computedWaitTime;
	}
}
