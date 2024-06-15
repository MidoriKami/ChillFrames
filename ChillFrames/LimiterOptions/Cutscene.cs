using ChillFrames.Controllers;
using KamiLib.Extensions;

namespace ChillFrames.LimiterOptions;

public class Cutscene : IFrameLimiterOption {
    public string Label => "Cutscenes";
    public bool Active => Service.Condition.IsInCutscene();
    public ref bool Enabled => ref ChillFramesSystem.Config.General.DisableDuringCutsceneSetting;
}