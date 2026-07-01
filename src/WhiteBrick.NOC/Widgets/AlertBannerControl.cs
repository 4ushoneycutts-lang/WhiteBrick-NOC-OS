using Avalonia;
using Avalonia.Media;

namespace WhiteBrick.NOC.Widgets;

public sealed class AlertBannerControl : AnimatedWidgetBase
{
    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<AlertBannerControl, string>(nameof(Text), "OPERATIONS MODE");

    public static readonly StyledProperty<string> SeverityProperty =
        AvaloniaProperty.Register<AlertBannerControl, string>(nameof(Severity), "SUCCESS");

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string Severity
    {
        get => GetValue(SeverityProperty);
        set => SetValue(SeverityProperty, value);
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var w = Bounds.Width;
        var h = Bounds.Height;
        var pulse = 0.55 + 0.45 * Math.Sin(TimeSeconds * 2.1);

        var c = Severity switch
        {
            "CRITICAL" => Color.FromRgb(255, 80, 80),
            "INFO" => Color.FromRgb(91, 125, 255),
            "QUIET" => Color.FromRgb(150, 130, 255),
            _ => Color.FromRgb(110, 255, 161)
        };

        context.FillRectangle(new SolidColorBrush(Color.FromArgb(155, 5, 8, 17)), new Rect(0, 0, w, h));
        context.DrawRectangle(new Pen(new SolidColorBrush(Color.FromArgb(180, c.R, c.G, c.B)), 1), new Rect(0.5, 0.5, Math.Max(0, w - 1), Math.Max(0, h - 1)));
        context.FillRectangle(new SolidColorBrush(Color.FromArgb((byte)(80 + 90 * pulse), c.R, c.G, c.B)), new Rect(0, 0, 5, h));

        var severityText = new FormattedText(Severity, System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Typeface.Default, 13, new SolidColorBrush(c));
        context.DrawText(severityText, new Point(16, h / 2 - severityText.Height / 2));

        var message = new FormattedText(Text, System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Typeface.Default, 13, new SolidColorBrush(Color.FromRgb(217, 247, 255)));
        context.DrawText(message, new Point(112, h / 2 - message.Height / 2));

        var scanX = (TimeSeconds * 120) % Math.Max(1, w);
        context.DrawLine(new Pen(new SolidColorBrush(Color.FromArgb(80, c.R, c.G, c.B)), 1), new Point(scanX, 0), new Point(scanX + 55, h));
    }
}
