using System;
using System.Diagnostics;
using System.Threading;
using ChillFrames.Data.SettingsObjects;
using Dalamud.Hooking;
using Dalamud.Utility.Signatures;

namespace ChillFrames.System
{
    internal class FrameLimiter : IDisposable
    {
        private delegate void SwapChainPresent(IntPtr address);

        [Signature("E8 ?? ?? ?? ?? C6 43 61 00 EB 3D ", DetourName = nameof(Swapchain_Present))]
        private readonly Hook<SwapChainPresent>? swapchainMethod = null!;

        private readonly Stopwatch timer = new();
        public GeneralSettings Settings => Service.Configuration.General;

        private int TargetFramerate => Settings.FrameRateLimit;
        private int TargetFrametime => 1000 / TargetFramerate;

        public FrameLimiter()
        {
            SignatureHelper.Initialise(this);

            timer.Start();
            swapchainMethod.Enable();
        }

        public void Dispose()
        {
            swapchainMethod?.Dispose();
        }

        private void Swapchain_Present(IntPtr address)
        {
            swapchainMethod!.Original(address);

            if (Condition.EnableFramerateLimit() && Settings.EnableLimiter)
            {
                var delayTime = TargetFrametime - timer.Elapsed.Milliseconds;

                if (delayTime > 0)
                {
                    Thread.Sleep(delayTime);
                }
            }

            timer.Restart();
        }
    }
}
