using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillFrames.Data.SettingsObjects
{
    public class GeneralSettings
    {
        public bool EnableLimiter = true;
        public int FrameRateLimit = 60;

        public bool DisableDuringCutscene = true;
        public bool DisableDuringCombat = true;
        public bool DisableDuringDuty = true;
        public bool DisableDuringGpose = true;
        public bool DisableDuringQuestEvent = true;
    }
}
