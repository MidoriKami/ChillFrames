using System;
using Dalamud.Plugin.Ipc;
using KamiLib.Utility;

namespace ChillFrames.Controllers;

public class ChillFramesIpcController : IDisposable
{
    /// <summary>
    ///     Function Signature: string GetVersion() <br /><br />
    ///     Gets the current version of this plugin, can be used as a IPC Ready Check<br /><br />
    ///     Returns the current version string<br />"Version #.#.#.#"
    /// </summary>
    private static ICallGateProvider<string>? _getVersion;

    /// <summary>
    ///     Function Signature: bool DisableLimiter(string callingPluginName) <br /><br />
    ///     Locks out ChillFrame's frame limiter, this results in the framerate being unmodified entirely by ChillFrames.
    /// </summary>
    private static ICallGateProvider<string, bool>? _disableLimiter;

    /// <summary>
    ///     Function Signature: bool EnableLimiter(string callingPluginName) <br /><br />
    ///     Removes the lockout placed by callingPluginName.
    /// </summary>
    private static ICallGateProvider<string, bool>? _enableLimiter;

    public ChillFramesIpcController()
    {
        _getVersion = Service.PluginInterface.GetIpcProvider<string>("ChillFrames.GetVersion");
        _disableLimiter = Service.PluginInterface.GetIpcProvider<string, bool>("ChillFrames.DisableLimiter");
        _enableLimiter = Service.PluginInterface.GetIpcProvider<string, bool>("ChillFrames.EnableLimiter");

        _getVersion.RegisterFunc(GetVersion);
        _disableLimiter.RegisterFunc(DisableLimiter);
        _enableLimiter.RegisterFunc(EnableLimiter);
    }

    public void Dispose()
    {
        _getVersion?.UnregisterFunc();
        _disableLimiter?.UnregisterFunc();
        _enableLimiter?.UnregisterFunc();
    }

    private static string GetVersion() 
        => PluginVersion.Instance.VersionText;

    private bool DisableLimiter(string callingPluginName) 
        => ChillFramesSystem.BlockList.Add(callingPluginName);

    private bool EnableLimiter(string callingPluginName) 
        => ChillFramesSystem.BlockList.Remove(callingPluginName);
}