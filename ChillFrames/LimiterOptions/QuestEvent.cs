using ChillFrames.Classes;
using ChillFrames.Utilities;
using Dalamud.Plugin.Services;

namespace ChillFrames.LimiterOptions;

public class QuestEvent : IFrameLimiterOption {
	public string Label => "Quest Event";

	public bool Active => ICondition.Get().IsInQuestEvent;

	public ref LimiterStateTarget Target => ref System.Config.General.QuestEventTarget;
}