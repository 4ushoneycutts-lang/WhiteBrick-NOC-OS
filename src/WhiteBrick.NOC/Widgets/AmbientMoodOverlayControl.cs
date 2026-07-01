using Avalonia;
using Avalonia.Media;

namespace WhiteBrick.NOC.Widgets;

public sealed class AmbientMoodOverlayControl : AnimatedWidgetBase
{
    public static readonly StyledProperty<string> ModeProperty =
        AvaloniaProperty.Register<AmbientMoodOverlayControl, string>(nameof(Mode), "OPERATIONS");

    public string Mode
    {
        get => GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var w = Bounds.Width;
        var h = Bounds.Height;
        var pulse = 0.50 + 0.50 * Math.Sin(TimeSeconds * 0.55);

        Color color = Mode switch
        {
            "ALERT" => Color.FromArgb((byte)(32 + 22 * pulse), 255, 80, 80),
            "INVESTIGATION" => Color.FromArgb((byte)(28 + 18 * pulse), 91, 125, 255),
            "NIGHT" => Color.FromArgb((byte)(34 + 12 * pulse), 30, 20, 70),
            _ => Color.FromArgb((byte)(24 + 16 * pulse), 36, 215, 255)
        };

        // Compatibility-first atmospheric overlay:
        // large soft ellipses create a room-mood wash without relying on newer gradient APIs.
        var brush = new SolidColorBrush(color);
        context.DrawEllipse(brush, null, new Point(w * 0.50, h * 0.35), w * 0.55, h * 0.48);
        context.DrawEllipse(new SolidColorBrush(Color.FromArgb((byte)(color.A * 0.55), color.R, color.G, color.B)), null, new Point(w * 0.18, h * 0.82), w * 0.30, h * 0.22);
        context.DrawEllipse(new SolidColorBrush(Color.FromArgb((byte)(color.A * 0.45), color.R, color.G, color.B)), null, new Point(w * 0.88, h * 0.72), w * 0.34, h * 0.28);
    }
}
