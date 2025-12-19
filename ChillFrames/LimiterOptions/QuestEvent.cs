using ChillFrames.Classes;
using ChillFrames.Utilities;

namespace ChillFrames.LimiterOptions;

public class QuestEvent : IFrameLimiterOption {
    public string Label => "Quest Event";
    
    public bool Active => Services.Condition.IsInQuestEvent;
    
    public ref bool Enabled => ref System.Config.General.DisableDuringQuestEventSetting;
}