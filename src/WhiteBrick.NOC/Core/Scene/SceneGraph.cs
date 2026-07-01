namespace WhiteBrick.NOC.Core.Scene;

public sealed class SceneGraph
{
    public const double DefaultWorldWidth = 5760;
    public const double DefaultWorldHeight = 1080;

    public SceneGraph(double worldWidth = DefaultWorldWidth, double worldHeight = DefaultWorldHeight)
    {
        WorldBounds = new SceneRect(0, 0, worldWidth, worldHeight);
        Root = new SceneNode("root", SceneLayer.Background, WorldBounds);
    }

    public SceneRect WorldBounds { get; }
    public SceneNode Root { get; }

    public IReadOnlyList<SceneNode> Nodes => Root.Flatten()
        .Where(n => n.Id != "root")
        .OrderBy(n => n.Layer)
        .ToList();

    public SceneNode Add(string id, SceneLayer layer, SceneRect bounds)
        => Root.AddChild(new SceneNode(id, layer, bounds));

    public int VisibleNodeCount => Nodes.Count(n => n.IsVisible);

    public static SceneGraph CreateDefaultWallScene()
    {
        var graph = new SceneGraph();
        graph.Add("background.particle-field", SceneLayer.Background, graph.WorldBounds);
        graph.Add("left.infrastructure", SceneLayer.Panels, new SceneRect(0, 0, 1920, 1080));
        graph.Add("center.hero-globe", SceneLayer.Widgets, new SceneRect(1920, 0, 1920, 1080));
        graph.Add("right.operations", SceneLayer.Panels, new SceneRect(3840, 0, 1920, 1080));
        graph.Add("packet-route.primary", SceneLayer.PacketTrails, new SceneRect(0, 0, 5760, 1080));
        graph.Add("developer.overlay", SceneLayer.DeveloperOverlay, new SceneRect(4140, 40, 1540, 360));
        return graph;
    }
}
