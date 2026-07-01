namespace WhiteBrick.NOC.Models;

public sealed record NocTelemetrySnapshot(
    DateTime Timestamp,
    double NetworkScore,
    double LatencyMs,
    double DownloadMbps,
    double UploadMbps,
    double PacketRate,
    double CpuPercent,
    double MemoryPercent,
    string WeatherSummary,
    NocHealthState HealthState);
