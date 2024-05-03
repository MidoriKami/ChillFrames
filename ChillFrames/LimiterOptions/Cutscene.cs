using ChillFrames.Controllers;
using ChillFrames.Interfaces;
using KamiLib.Classes;

namespace ChillFrames.Models.LimiterOptions;

public class Cutscene : IFrameLimiterOption {
    public string Label => "Cutscenes";
    public bool Active => Service.Condition.IsInCutscene();
    public ref bool Enabled => ref ChillFramesSystem.Config.General.DisableDuringCutsceneSetting;
}