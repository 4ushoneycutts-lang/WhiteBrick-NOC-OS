namespace WhiteBrick.NOC.Core.Scene;

public sealed class SceneNode
{
    private readonly List<SceneNode> _children = new();

    public SceneNode(string id, SceneLayer layer, SceneRect bounds)
    {
        Id = id;
        Layer = layer;
        Bounds = bounds;
    }

    public string Id { get; }
    public SceneLayer Layer { get; set; }
    public SceneRect Bounds { get; set; }
    public bool IsVisible { get; set; } = true;
    public IReadOnlyList<SceneNode> Children => _children;

    public SceneNode AddChild(SceneNode child)
    {
        _children.Add(child);
        return child;
    }

    public IEnumerable<SceneNode> Flatten()
    {
        yield return this;
        foreach (var child in _children)
        {
            foreach (var nested in child.Flatten())
                yield return nested;
        }
    }
}
