using Avalonia;
using Avalonia.Media;

namespace WhiteBrick.NOC.Widgets;

public sealed class WeatherPulseControl : AnimatedWidgetBase
{
    public static readonly StyledProperty<string> TemperatureProperty =
        AvaloniaProperty.Register<WeatherPulseControl, string>(nameof(Temperature), "72°F");

    public static readonly StyledProperty<string> ConditionProperty =
        AvaloniaProperty.Register<WeatherPulseControl, string>(nameof(Condition), "SIM WEATHER");

    public static readonly StyledProperty<string> WindProperty =
        AvaloniaProperty.Register<WeatherPulseControl, string>(nameof(Wind), "WIND 4 MPH");

    public string Temperature { get => GetValue(TemperatureProperty); set => SetValue(TemperatureProperty, value); }
    public string Condition { get => GetValue(ConditionProperty); set => SetValue(ConditionProperty, value); }
    public string Wind { get => GetValue(WindProperty); set => SetValue(WindProperty, value); }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var w = Bounds.Width;
        var h = Bounds.Height;
        context.FillRectangle(new SolidColorBrush(Color.FromArgb(88, 6, 16, 31)), Bounds);
        context.DrawRectangle(new Pen(new SolidColorBrush(Color.FromArgb(95, 36, 215, 255)), 1), new Rect(0.5, 0.5, Math.Max(0, w - 1), Math.Max(0, h - 1)));

        var cx = w * 0.22;
        var cy = h * 0.52;
        var pulse = 0.5 + 0.5 * Math.Sin(TimeSeconds * 1.7);

        context.DrawEllipse(new SolidColorBrush(Color.FromArgb((byte)(35 + 55 * pulse), 36, 215, 255)), null, new Point(cx, cy), 34 + pulse * 8, 34 + pulse * 8);
        context.DrawEllipse(new SolidColorBrush(Color.FromArgb(210, 255, 200, 87)), null, new Point(cx - 5, cy - 6), 11, 11);
        context.DrawEllipse(new SolidColorBrush(Color.FromArgb(210, 189, 238, 255)), null, new Point(cx + 8, cy + 5), 19, 10);
        context.DrawEllipse(new SolidColorBrush(Color.FromArgb(210, 189, 238, 255)), null, new Point(cx - 7, cy + 6), 15, 9);

        for (int i = 0; i < 7; i++)
        {
            var x = w * 0.44 + i * (w * 0.07);
            var y = h * 0.72 + Math.Sin(TimeSeconds * 1.4 + i) * 5;
            context.DrawLine(new Pen(new SolidColorBrush(Color.FromArgb(95, 142, 248, 255)), 1), new Point(x, y), new Point(x + 18, y - 8));
        }

        DrawText(context, "LIVE WEATHER SLOT", 10, 8, 11, Color.FromRgb(119, 200, 232));
        DrawText(context, Temperature, w * 0.42, 22, 28, Color.FromRgb(217, 247, 255));
        DrawText(context, Condition, w * 0.42, 55, 12, Color.FromRgb(142, 248, 255));
        DrawText(context, Wind, w * 0.42, 74, 11, Color.FromRgb(119, 200, 232));
    }

    private static void DrawText(DrawingContext context, string text, double x, double y, double size, Color color)
    {
        var formatted = new FormattedText(text, System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Typeface.Default, size, new SolidColorBrush(color));
        context.DrawText(formatted, new Point(x, y));
    }
}
