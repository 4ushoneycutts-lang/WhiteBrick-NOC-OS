using WhiteBrick.NOC.Models;

namespace WhiteBrick.NOC.Telemetry;

public sealed class FakeTelemetryProvider : ITelemetryProvider
{
    private readonly Random _random = new();
    private double _phase;

    public string Name => "Fake Telemetry Provider";

    public NocTelemetrySnapshot GetSnapshot()
    {
        _phase += 0.08;

        var latency = 17 + Math.Sin(_phase) * 5 + _random.NextDouble() * 3;
        var score = Math.Clamp(96 - Math.Abs(latency - 18) * 0.7, 82, 100);
        var download = 850 + Math.Sin(_phase * 0.6) * 90 + _random.NextDouble() * 40;
        var upload = 42 + Math.Cos(_phase * 0.5) * 6 + _random.NextDouble() * 3;
        var packetRate = 1200 + Math.Sin(_phase * 1.7) * 350 + _random.Next(0, 180);
        var cpu = 18 + Math.Abs(Math.Sin(_phase * 0.8)) * 22;
        var memory = 38 + Math.Abs(Math.Cos(_phase * 0.35)) * 12;

        return new NocTelemetrySnapshot(
            DateTime.Now,
            Math.Round(score, 1),
            Math.Round(latency, 1),
            Math.Round(download, 1),
            Math.Round(upload, 1),
            Math.Round(packetRate, 0),
            Math.Round(cpu, 1),
            Math.Round(memory, 1),
            "SIM WEATHER ONLINE",
            score > 90 ? NocHealthState.Healthy : NocHealthState.Warning);
    }
}
