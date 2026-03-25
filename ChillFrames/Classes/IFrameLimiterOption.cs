namespace ChillFrames.Classes;

public interface IFrameLimiterOption {
    string Label { get; }
    ref LimiterStateTarget Target { get; }
    bool Active { get; }
}
