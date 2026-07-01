using System.Diagnostics;

namespace WhiteBrick.NOC.Core.Timing;

public sealed class PerformanceClock
{
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
    private TimeSpan _lastFrame;

    public double TotalSeconds => _stopwatch.Elapsed.TotalSeconds;
    public double DeltaSeconds { get; private set; }
    public double FramesPerSecond { get; private set; }

    public void Tick()
    {
        var now = _stopwatch.Elapsed;
        DeltaSeconds = Math.Max(0.0001, (now - _lastFrame).TotalSeconds);
        FramesPerSecond = 1.0 / DeltaSeconds;
        _lastFrame = now;
    }
}
