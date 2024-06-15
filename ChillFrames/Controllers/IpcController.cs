using System;
using Dalamud.Plugin.Ipc;

namespace ChillFrames.Controllers;

public class IpcController : IDisposable {
    /// <summary>
    ///     Function Signature: bool DisableLimiter(string callingPluginName) <br /><br />
    ///     Locks out ChillFrame's frame limiter, this results in the framerate being unmodified entirely by ChillFrames.
    /// </summary>
    private static ICallGateProvider<string, bool>? disableLimiter;

    /// <summary>
    ///     Function Signature: bool EnableLimiter(string callingPluginName) <br /><br />
    ///     Removes the lockout placed by callingPluginName.
    /// </summary>
    private static ICallGateProvider<string, bool>? enableLimiter;

    public IpcController() {
        disableLimiter = Service.PluginInterface.GetIpcProvider<string, bool>("ChillFrames.DisableLimiter");
        enableLimiter = Service.PluginInterface.GetIpcProvider<string, bool>("ChillFrames.EnableLimiter");

        disableLimiter.RegisterFunc(DisableLimiter);
        enableLimiter.RegisterFunc(EnableLimiter);
    }

    public void Dispose() {
        disableLimiter?.UnregisterFunc();
        enableLimiter?.UnregisterFunc();
    }

    private bool DisableLimiter(string callingPluginName) 
        => System.BlockList.Add(callingPluginName);

    private bool EnableLimiter(string callingPluginName) 
        => System.BlockList.Remove(callingPluginName);
}