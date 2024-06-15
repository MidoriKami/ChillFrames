namespace ChillFrames.LimiterOptions;

public interface IFrameLimiterOption {
    string Label { get; }
    ref bool Enabled { get; }
    bool Active { get; }
}