using WhiteBrick.NOC.Models;

namespace WhiteBrick.NOC.Telemetry;

public interface ITelemetryProvider
{
    string Name { get; }
    NocTelemetrySnapshot GetSnapshot();
}
