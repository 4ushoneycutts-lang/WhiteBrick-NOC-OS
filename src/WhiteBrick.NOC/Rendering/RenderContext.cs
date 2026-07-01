using WhiteBrick.NOC.Core.Scene;

namespace WhiteBrick.NOC.Rendering;

public sealed class RenderContext
{
    public RenderContext(SceneRect worldBounds, IReadOnlyList<DisplayZone> displayZones)
    {
        WorldBounds = worldBounds;
        DisplayZones = displayZones;
    }

    public SceneRect WorldBounds { get; }
    public IReadOnlyList<DisplayZone> DisplayZones { get; }
    public double TimeSeconds { get; init; }
    public double DeltaSeconds { get; init; }
    public double FramesPerSecond { get; init; }
}
