using ChillFrames.Controllers;
using KamiLib.Extensions;

namespace ChillFrames.LimiterOptions;

public class QuestEvent : IFrameLimiterOption {
    public string Label => "Quest Event";
    public bool Active => Service.Condition.IsInQuestEvent();
    public ref bool Enabled => ref ChillFramesSystem.Config.General.DisableDuringQuestEventSetting;
}