using ChillFrames.Windows.Components;
using KamiLib.AutomaticUserInterface;

namespace ChillFrames.Config;


[Category("Framerate Limiter Settings",1)]
public interface ILimiterSettings
{
    [BoolConfig("Use Precise Framerate Target", "This mode is less efficient but results in a much more stable framerate")]
    public bool PreciseFramerate { get; set; }
    
    [BoolConfig("Enable Idle Framerate Limiter", "This will limit the games framerate whenever any of the conditions are not met\nFor example, while afk in Limsa you want to limit your framerate to save power")]
    public bool EnableIdleFramerateLimit { get; set; }
    
    [DelayedIntCounterConfig("Idle Framerate Target", false)]
    public int IdleFramerateTarget { get; set; }
    
    [BoolConfig("Enable Active Framerate Limiter", "This will limit the games framerate whenever any of the conditions are met\nFor example, while in duties you want to limit your framerate to your displays refresh rate")]
    public bool EnableActiveFramerateLimit { get; set; }
    
    [DelayedIntCounterConfig("Active Framerate Target", false)]
    public int ActiveFramerateTarget { get; set; }
}

public class LimiterSettings : ILimiterSettings
{
    // ILimiterSettings
    public bool PreciseFramerate { get; set; } = true;
    public bool EnableIdleFramerateLimit { get; set; } = true;
    public int IdleFramerateTarget { get; set; } = 60;
    public bool EnableActiveFramerateLimit { get; set; } = false;
    public int ActiveFramerateTarget { get; set; } = 60;
}