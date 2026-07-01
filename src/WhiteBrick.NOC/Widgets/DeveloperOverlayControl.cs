using Avalonia;
using Avalonia.Media;

namespace WhiteBrick.NOC.Widgets;

public sealed class DeveloperOverlayControl : AnimatedWidgetBase
{
    public static readonly StyledProperty<string> EngineVersionProperty =
        AvaloniaProperty.Register<DeveloperOverlayControl, string>(nameof(EngineVersion), "2.5.001");

    public static readonly StyledProperty<string> SceneNodesProperty =
        AvaloniaProperty.Register<DeveloperOverlayControl, string>(nameof(SceneNodes), "0");

    public static readonly StyledProperty<string> DisplayZonesProperty =
        AvaloniaProperty.Register<DeveloperOverlayControl, string>(nameof(DisplayZones), "0");

    public static readonly StyledProperty<string> AssetsProperty =
        AvaloniaProperty.Register<DeveloperOverlayControl, string>(nameof(Assets), "0");

    public string EngineVersion { get => GetValue(EngineVersionProperty); set => SetValue(EngineVersionProperty, value); }
    public string SceneNodes { get => GetValue(SceneNodesProperty); set => SetValue(SceneNodesProperty, value); }
    public string DisplayZones { get => GetValue(DisplayZonesProperty); set => SetValue(DisplayZonesProperty, value); }
    public string Assets { get => GetValue(AssetsProperty); set => SetValue(AssetsProperty, value); }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var w = Bounds.Width;
        var h = Bounds.Height;
        context.FillRectangle(new SolidColorBrush(Color.FromArgb(220, 3, 8, 18)), new Rect(0, 0, w, h));
        context.DrawRectangle(new Pen(new SolidColorBrush(Color.FromArgb(190, 36, 215, 255)), 1), new Rect(0.5, 0.5, Math.Max(0, w - 1), Math.Max(0, h - 1)));

        DrawLine(context, "WHITE BRICK DEVELOPER OVERLAY", 16, 12, 16, Color.FromRgb(217, 247, 255));
        DrawLine(context, "F12 toggles this engine view", 16, 36, 11, Color.FromRgb(119, 200, 232));

        DrawLine(context, $"Rendering Engine : {EngineVersion}", 16, 72, 13, Color.FromRgb(142, 248, 255));
        DrawLine(context, $"Scene Nodes      : {SceneNodes}", 16, 96, 13, Color.FromRgb(142, 248, 255));
        DrawLine(context, $"Display Zones    : {DisplayZones}", 16, 120, 13, Color.FromRgb(142, 248, 255));
        DrawLine(context, $"Assets Loaded    : {Assets}", 16, 144, 13, Color.FromRgb(142, 248, 255));

        var pulse = 0.55 + 0.45 * Math.Sin(TimeSeconds * 2.3);
        context.DrawEllipse(new SolidColorBrush(Color.FromArgb((byte)(90 + 80 * pulse), 110, 255, 161)), null, new Point(w - 28, 28), 8, 8);
        DrawLine(context, "ENGINE FOUNDATION ONLINE", 16, h - 32, 12, Color.FromRgb(110, 255, 161));
    }

    private static void DrawLine(DrawingContext context, string text, double x, double y, double size, Color color)
    {
        var formatted = new FormattedText(text, System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Typeface.Default, size, new SolidColorBrush(color));
        context.DrawText(formatted, new Point(x, y));
    }
}
