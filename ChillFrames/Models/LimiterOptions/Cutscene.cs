using ChillFrames.Controllers;
using ChillFrames.Interfaces;
using KamiLib.Game;

namespace ChillFrames.Models.LimiterOptions;

public class Cutscene : IFrameLimiterOption {
    public string Label => "Cutscenes";
    public bool Active => Condition.IsInCutscene();
    public ref bool Enabled => ref ChillFramesSystem.Config.General.DisableDuringCutsceneSetting;
}