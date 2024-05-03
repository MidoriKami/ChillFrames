using System;
using Dalamud.Plugin.Ipc;

namespace ChillFrames.Controllers;

public class ChillFramesIpcController : IDisposable {
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

    public ChillFramesIpcController() {
        _disableLimiter = Service.PluginInterface.GetIpcProvider<string, bool>("ChillFrames.DisableLimiter");
        _enableLimiter = Service.PluginInterface.GetIpcProvider<string, bool>("ChillFrames.EnableLimiter");

        _disableLimiter.RegisterFunc(DisableLimiter);
        _enableLimiter.RegisterFunc(EnableLimiter);
    }

    public void Dispose() {
        _disableLimiter?.UnregisterFunc();
        _enableLimiter?.UnregisterFunc();
    }

    private bool DisableLimiter(string callingPluginName) 
        => ChillFramesSystem.BlockList.Add(callingPluginName);

    private bool EnableLimiter(string callingPluginName) 
        => ChillFramesSystem.BlockList.Remove(callingPluginName);
}