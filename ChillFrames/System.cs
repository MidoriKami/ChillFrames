using System.Collections.Generic;
using ChillFrames.Classes;
using ChillFrames.Controllers;
using ChillFrames.Windows;
using Dalamud.Interface.Windowing;

namespace ChillFrames;

public static class System {
	public static Configuration Config { get; set; } = null!;
	public static WindowSystem WindowSystem { get; set; } = null!;
	public static SettingsWindow ConfigWindow { get; set; } = null!;
	public static DtrController DtrController { get; set; } = null!;
	public static FrameLimiterController FrameLimiterController { get; set; } = null!;
	public static IdleFpsController IdleFpsController { get; set; } = null!;
	public static List<IFrameLimiterOption> LimiterOptions { get; set; } = [];
}