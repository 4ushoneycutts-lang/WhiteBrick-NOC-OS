namespace WhiteBrick.NOC.Core.Scene;

public readonly record struct SceneRect(double X, double Y, double Width, double Height)
{
    public double Right => X + Width;
    public double Bottom => Y + Height;
    public static SceneRect Empty { get; } = new(0, 0, 0, 0);
}
