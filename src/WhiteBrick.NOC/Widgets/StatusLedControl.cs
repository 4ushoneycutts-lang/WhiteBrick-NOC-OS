using Avalonia;
using Avalonia.Media;

namespace WhiteBrick.NOC.Widgets;

public sealed class StatusLedControl : AnimatedWidgetBase
{
    public static readonly StyledProperty<string> StateProperty =
        AvaloniaProperty.Register<StatusLedControl, string>(nameof(State), "Healthy");

    public string State
    {
        get => GetValue(StateProperty);
        set => SetValue(StateProperty, value);
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);
        var center = new Point(Bounds.Width / 2, Bounds.Height / 2);
        var r = Math.Min(Bounds.Width, Bounds.Height) * 0.28;
        var pulse = 0.55 + 0.45 * Math.Sin(TimeSeconds * 2.5);

        var color = State switch
        {
            "Warning" => Color.FromRgb(255, 200, 87),
            "Critical" => Color.FromRgb(255, 80, 80),
            _ => Color.FromRgb(110, 255, 161)
        };

        context.DrawEllipse(new SolidColorBrush(Color.FromArgb((byte)(35 + 85 * pulse), color.R, color.G, color.B)), null, center, r * 2.2, r * 2.2);
        context.DrawEllipse(new SolidColorBrush(Color.FromArgb(235, color.R, color.G, color.B)), null, center, r, r);
    }
}
