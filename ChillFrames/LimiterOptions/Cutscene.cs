using ChillFrames.Classes;
using KamiLib.Extensions;

namespace ChillFrames.LimiterOptions;

public class Cutscene : IFrameLimiterOption {
    public string Label => "Cutscenes";
    public bool Active => Service.Condition.IsInCutscene();
    public ref bool Enabled => ref System.Config.General.DisableDuringCutsceneSetting;
}