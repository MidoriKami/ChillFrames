using System.Collections.Generic;
using ChillFrames.Classes;
using ChillFrames.Controllers;
using ChillFrames.LimiterOptions;
using KamiLib.CommandManager;
using KamiLib.Window;

namespace ChillFrames;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public static class System {
	public static Configuration Config { get; set; }
	public static WindowManager WindowManager { get; set; }
	public static CommandManager CommandManager { get; set; }
	public static DtrController DtrController { get; set; }
	
	public static HashSet<string> BlockList { get; set; } = [];

	public static FrameLimiterController frameLimiterController { get; set; }
	public static IpcController ipcController { get; set; }

	public static List<IFrameLimiterOption> LimiterOptions { get; set; } = [];
}