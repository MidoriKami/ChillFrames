namespace ChillFrames.Interfaces;

public interface IFrameLimiterOption
{
    string Label { get; }

    bool IsActive();
    ref bool GetSetting();

    bool IsEnabled() => GetSetting();
}