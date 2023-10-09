namespace ChillFrames.Models;

public class LimiterSettings
{
    public int ActiveFramerateTarget = 60;
    public bool EnableActiveFramerateLimit = false;
    public bool EnableIdleFramerateLimit = true;
    public int IdleFramerateTarget = 60;
    public bool PreciseFramerate = true;
}