using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChillFrames.Data.Enums;
using ChillFrames.Data.Enums.BattleEffect;
using ChillFrames.System;
using XivCommon;

namespace ChillFrames.Data.SettingsObjects.Components
{
    public class PerformanceProfile
    {
        public BattleEffectCommand Self = new() {Target = Target.Self, Setting = Setting.All};
        public BattleEffectCommand Party = new() {Target = Target.Party, Setting = Setting.All};
        public BattleEffectCommand Other = new() {Target = Target.Other, Setting = Setting.All};
        
        public void Apply()
        {
            Self.Execute();
            Party.Execute();
            Other.Execute();
        }
    }
}
