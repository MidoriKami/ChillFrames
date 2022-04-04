using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChillFrames.Data.Enums.BattleEffect;
using ChillFrames.Utilities;
using XivCommon;

namespace ChillFrames.Data.SettingsObjects.Components
{
    public class BattleEffectCommand
    {
        public Setting Setting;
        public Target Target;

        public void Execute()
        {
            Service.XivCommon.Functions.Chat.SendMessage(ConstructCommand());
        }

        private string ConstructCommand()
        {
            return @"/battleeffect " + GetTargetString() + " " + GetSettingsString();
        }

        private string GetSettingsString()
        {
            return Setting switch
            {
                Setting.All => "all",
                Setting.Simple => "simple",
                Setting.Off => "off",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private string GetTargetString()
        {
            return Target switch
            {
                Target.Self => "self",
                Target.Party => "party",
                Target.Other => "other",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
