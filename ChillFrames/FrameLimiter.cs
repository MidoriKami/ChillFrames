using System;
using System.Diagnostics;
using System.Threading;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Hooking;
using Dalamud.Utility.Signatures;

namespace ChillFrames
{
    internal unsafe class FrameLimiter : IDisposable
    {
        private delegate void SwapChainPresent(IntPtr address);

        [Signature("E8 ?? ?? ?? ?? C6 83 ?? ?? ?? ?? ?? 48 8B 4B 70", DetourName = nameof(LimitFramerate))]
        private readonly Hook<SwapChainPresent>? swapchainMethod = null!;

        private readonly Stopwatch timer = new();

        private int TargetFramerate => Service.Configuration.FrameRateLimit;
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

        private void LimitFramerate(IntPtr address)
        {
            swapchainMethod!.Original(address);

            if (Service.Configuration.EnableLimiter == true)
            {
                if (ShouldLimitFramerate())
                {
                    var delayTime = TargetFrametime - timer.Elapsed.Milliseconds;

                    if (delayTime > 0)
                    {
                        Thread.Sleep(delayTime);
                    }
                }
            }

            timer.Restart();
        }

        private bool ShouldLimitFramerate()
        {
            var boundByDuty = BoundByDuty();
            var inCombat = InCombat();
            var inCutscene = InCutscene();
            var inBlacklistedArea = InBlacklistedZone();

            return !boundByDuty && !inCombat && !inCutscene && !inBlacklistedArea;
        }

        private bool InCutscene()
        {
            return Service.Configuration.DisableDuringCutscene &&
                   Service.Condition[ConditionFlag.OccupiedInCutSceneEvent] == true;
        }

        private bool BoundByDuty()
        {
            return Service.Configuration.DisableDuringDuty &&
                   (Service.Condition[ConditionFlag.BoundByDuty] ||
                   Service.Condition[ConditionFlag.BoundByDuty56] ||
                   Service.Condition[ConditionFlag.BoundByDuty95]);
        }

        private bool InCombat()
        {
            return Service.Configuration.DisableDuringCombat &&
                   Service.Condition[ConditionFlag.InCombat];
        }

        private bool InBlacklistedZone()
        {
            return Service.Configuration.DisableInBlacklistedTerritories &&
                   Service.Configuration.TerritoryBlacklist.Contains(Service.ClientState.TerritoryType);
        }
    }
}
