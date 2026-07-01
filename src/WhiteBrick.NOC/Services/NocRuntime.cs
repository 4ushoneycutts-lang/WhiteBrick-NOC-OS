using WhiteBrick.NOC.Core;
using WhiteBrick.NOC.Telemetry;
using WhiteBrick.NOC.Widgets;

namespace WhiteBrick.NOC.Services;

public sealed class NocRuntime
{
    public ITelemetryProvider TelemetryProvider { get; }
    public NocEngine Engine { get; }
    public WidgetRegistry WidgetRegistry { get; }
    public OperatingModeManager OperatingModes { get; }

    private NocRuntime(ITelemetryProvider telemetryProvider, NocEngine engine, WidgetRegistry widgetRegistry, OperatingModeManager operatingModes)
    {
        TelemetryProvider = telemetryProvider;
        Engine = engine;
        WidgetRegistry = widgetRegistry;
        OperatingModes = operatingModes;
    }

    public static NocRuntime CreateDefault()
    {
        return new NocRuntime(
            new FakeTelemetryProvider(),
            NocEngine.CreateDefault(),
            WidgetRegistry.CreateDefault(),
            new OperatingModeManager());
    }
}
