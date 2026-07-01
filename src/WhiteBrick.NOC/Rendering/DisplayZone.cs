using WhiteBrick.NOC.Core.Scene;

namespace WhiteBrick.NOC.Rendering;

public sealed record DisplayZone(string Name, SceneRect Bounds, int Index);
