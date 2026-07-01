namespace WhiteBrick.NOC.Core.Scene;

public readonly record struct SceneVector2(double X, double Y)
{
    public static SceneVector2 Zero { get; } = new(0, 0);
}
