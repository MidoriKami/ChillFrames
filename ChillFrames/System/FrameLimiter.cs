using System;
using System.Diagnostics;
using System.Threading;
using ChillFrames.Data.SettingsObjects;
using ChillFrames.Enums;
using Dalamud.Hooking;
using Dalamud.Utility.Signatures;

namespace ChillFrames.System
{
    internal class FrameLimiter : IDisposable
    {
        private delegate void SwapChainPresent(IntPtr address);

        [Signature("E8 ?? ?? ?? ?? C6 47 79 00", DetourName = nameof(Swapchain_Present))]
        private readonly Hook<SwapChainPresent>? swapchainMethod = null;

        private readonly Stopwatch timer = new();
        private readonly Stopwatch steppingStopwatch = new();
        public GeneralSettings Settings => Service.Configuration.General;

        private int TargetFramerate => Settings.FrameRateLimit;
        private int TargetFrametime => 1000 / TargetFramerate;

        private LimiterState state;
        private bool enabledLastFrame;
        private float delayRatio = 1.0f;

        public static float DisableIncrement = Service.Configuration.DisableIncrement;
        public static float EnableIncrement = Service.Configuration.EnableIncrement;

        public FrameLimiter()
        {
            SignatureHelper.Initialise(this);

            timer.Start();
            steppingStopwatch.Start();
            swapchainMethod?.Enable();
        }

        public void Dispose()
        {
            swapchainMethod?.Dispose();
        }

        private void Swapchain_Present(IntPtr address)
        {
            swapchainMethod!.Original(address);

            UpdateState();

            UpdateRate();

            if (Settings.EnableLimiter && (Condition.EnableFramerateLimit() || state != LimiterState.SteadyState))
            {
                var delayTime = TargetFrametime - timer.Elapsed.Milliseconds;

                delayTime = (int)(delayRatio * delayTime);

                if (delayTime > 0)
                {
                    Thread.Sleep(delayTime);
                }
            }

            timer.Restart();
        }

        private void UpdateState()
        {
            var shouldLimit = Condition.EnableFramerateLimit();

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
}
