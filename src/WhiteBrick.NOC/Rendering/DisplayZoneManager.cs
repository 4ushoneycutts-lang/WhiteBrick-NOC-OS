using WhiteBrick.NOC.Core.Scene;

namespace WhiteBrick.NOC.Rendering;

public sealed class DisplayZoneManager
{
    private readonly List<DisplayZone> _zones = new();

    public DisplayZoneManager()
    {
        _zones.Add(new DisplayZone("LEFT / INFRASTRUCTURE", new SceneRect(0, 0, 1920, 1080), 0));
        _zones.Add(new DisplayZone("CENTER / HERO", new SceneRect(1920, 0, 1920, 1080), 1));
        _zones.Add(new DisplayZone("RIGHT / OPERATIONS", new SceneRect(3840, 0, 1920, 1080), 2));
    }

    public IReadOnlyList<DisplayZone> Zones => _zones;
    public double TotalWidth => _zones.Sum(z => z.Bounds.Width);
    public double TotalHeight => _zones.Count == 0 ? 0 : _zones.Max(z => z.Bounds.Height);
}
