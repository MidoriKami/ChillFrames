using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using ChillFrames.Config;
using Dalamud.Game;

namespace ChillFrames.System;

public enum LimiterState
{
    Enabled,
    Disabled,
    SteadyState
}

internal class FrameLimiter : IDisposable
{
    private readonly Stopwatch timer = Stopwatch.StartNew();
    private readonly Stopwatch steppingStopwatch = Stopwatch.StartNew();
    private static GeneralSettings Settings => Service.Configuration.General;

    private static int TargetFramerate => Settings.FrameRateLimitSetting.Value;
    private static int TargetFrametime => 1000 / TargetFramerate;
    private static int PreciseFrameTickTime => (int)(1000.0f / Settings.FrameRateLimitSetting.Value * 10000);

    private LimiterState state;
    private bool enabledLastFrame;
    private float delayRatio = 1.0f;

    private static float DisableIncrement => Service.Configuration.DisableIncrementSetting.Value;
    private static float EnableIncrement => Service.Configuration.EnableIncrementSetting.Value;

    public FrameLimiter()
    { 
        Service.Framework.Update += OnFrameworkUpdate;
    }

    public void Dispose()
    {
        Service.Framework.Update -= OnFrameworkUpdate;
    }

    private void OnFrameworkUpdate(Framework framework)
    {
        UpdateState();

        UpdateRate();

        TryLimitFramerate();

        timer.Restart();
    }
    
    [MethodImpl(MethodImplOptions.NoOptimization)]
    private void TryLimitFramerate()
    {
        if (Settings.EnableLimiterSetting && (!FrameLimiterCondition.DisableFramerateLimit() || state != LimiterState.SteadyState))
        {
            var delayTime = (int)(TargetFrametime - timer.ElapsedMilliseconds);

            if (Settings.PreciseFramerate)
            {
                if (delayTime - 1 > 0)
                {
                    Thread.Sleep(delayTime - 1);
                }
                
                while (timer.ElapsedTicks <= PreciseFrameTickTime)
                {
                    ((Action)(() => { }))();
                }
            }
            else
            {
                delayTime = (int) (delayRatio * delayTime);

                if (delayTime > 0)
                {
                    Thread.Sleep(delayTime);
                }
            }
        }
        
    }

    private void UpdateState()
    {
        var shouldLimit = !FrameLimiterCondition.DisableFramerateLimit();

        if (enabledLastFrame != shouldLimit)
        {
            state = enabledLastFrame switch
            {
                true => LimiterState.Disabled,
                false => LimiterState.Enabled,
            };
        }

        enabledLastFrame = shouldLimit;
    }

    private void UpdateRate()
    {
        const int stepDelay = 40;

        if (steppingStopwatch.ElapsedMilliseconds > stepDelay)
        {
            switch (state)
            {
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