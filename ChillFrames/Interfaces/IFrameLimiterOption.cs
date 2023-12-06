namespace ChillFrames.Interfaces;

public interface IFrameLimiterOption {
    string Label { get; }

    bool GetActive();
    ref bool GetSetting();

    bool IsEnabled => GetSetting();
    bool IsActive => GetActive();
}