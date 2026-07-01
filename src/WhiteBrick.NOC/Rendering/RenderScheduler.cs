using WhiteBrick.NOC.Core.Scene;
using WhiteBrick.NOC.Core.Timing;

namespace WhiteBrick.NOC.Rendering;

public sealed class RenderScheduler
{
    private readonly PerformanceClock _clock = new();
    private readonly DisplayZoneManager _displayZones;
    private readonly SceneGraph _sceneGraph;

    public RenderScheduler(SceneGraph sceneGraph, DisplayZoneManager displayZones)
    {
        _sceneGraph = sceneGraph;
        _displayZones = displayZones;
    }

    public RenderContext LastContext { get; private set; } = new(new SceneRect(0, 0, 5760, 1080), Array.Empty<DisplayZone>());

    public RenderContext Tick()
    {
        _clock.Tick();
        LastContext = new RenderContext(_sceneGraph.WorldBounds, _displayZones.Zones)
        {
            TimeSeconds = _clock.TotalSeconds,
            DeltaSeconds = _clock.DeltaSeconds,
            FramesPerSecond = _clock.FramesPerSecond
        };
        return LastContext;
    }
}
