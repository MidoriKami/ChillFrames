using ChillFrames.Controllers;
using ChillFrames.Interfaces;
using KamiLib.Game;

namespace ChillFrames.Models.LimiterOptions;

public class QuestEvent : IFrameLimiterOption
{
    public string Label => "Quest Event";
    public bool IsActive() => Condition.IsInQuestEvent();
    public ref bool GetSetting() => ref ChillFramesSystem.Config.General.DisableDuringQuestEventSetting;
}