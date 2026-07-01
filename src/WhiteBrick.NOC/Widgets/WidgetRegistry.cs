namespace WhiteBrick.NOC.Widgets;

public sealed class WidgetRegistry
{
    private readonly Dictionary<string, Type> _widgets = new(StringComparer.OrdinalIgnoreCase);

    public void Register<TWidget>(string key) where TWidget : Avalonia.Controls.Control
        => _widgets[key] = typeof(TWidget);

    public IReadOnlyDictionary<string, Type> Widgets => _widgets;

    public static WidgetRegistry CreateDefault()
    {
        var registry = new WidgetRegistry();
        registry.Register<ParticleFieldControl>("particle.field");
        registry.Register<HeroGlobeControl>("hero.globe");
        registry.Register<RadarControl>("radar.sweep");
        registry.Register<HistoryGraphControl>("graph.history");
        registry.Register<InfrastructureCardControl>("infrastructure.card");
        registry.Register<PacketStreamControl>("packet.stream");
        registry.Register<GaugeRingControl>("gauge.ring");
        registry.Register<StatusLedControl>("status.led");
        registry.Register<TimelineStreamControl>("timeline.stream");
        registry.Register<WeatherPulseControl>("weather.pulse");
        registry.Register<CameraPreviewWallControl>("camera.preview.wall");
        return registry;
    }
}
