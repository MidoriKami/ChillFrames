using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using ChillFrames.Classes;
using Dalamud.Plugin.Services;

namespace ChillFrames.Controllers;

public enum LimiterState {
    Enabled,
    Disabled,
    SteadyState,
}

public class FrameLimiterController : IDisposable {
    private readonly Stopwatch steppingStopwatch = Stopwatch.StartNew();
    private readonly Stopwatch timer = Stopwatch.StartNew();
    private float delayRatio = 1.0f;
    private bool enabledLastFrame;

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

    public static TimeSpan LastFrametime;

    public FrameLimiterController() {
        Services.Framework.Update += OnFrameworkUpdate;
    }

    public void Dispose() {
        Services.Framework.Update -= OnFrameworkUpdate;
    }

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
        if (System.BlockList.Count > 0) return;

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
                false => LimiterState.Enabled
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
}
