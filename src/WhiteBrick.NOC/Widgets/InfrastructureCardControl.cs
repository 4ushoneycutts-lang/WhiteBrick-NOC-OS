using Avalonia;
using Avalonia.Media;

namespace WhiteBrick.NOC.Widgets;

public sealed class InfrastructureCardControl : AnimatedWidgetBase
{
    public static readonly StyledProperty<string> DeviceNameProperty =
        AvaloniaProperty.Register<InfrastructureCardControl, string>(nameof(DeviceName), "DEVICE");

    public static readonly StyledProperty<string> StatusProperty =
        AvaloniaProperty.Register<InfrastructureCardControl, string>(nameof(Status), "ONLINE");

    public static readonly StyledProperty<double> LoadProperty =
        AvaloniaProperty.Register<InfrastructureCardControl, double>(nameof(Load), 0.75);

    public string DeviceName { get => GetValue(DeviceNameProperty); set => SetValue(DeviceNameProperty, value); }
    public string Status { get => GetValue(StatusProperty); set => SetValue(StatusProperty, value); }
    public double Load { get => GetValue(LoadProperty); set => SetValue(LoadProperty, value); }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var w = Bounds.Width;
        var h = Bounds.Height;
        var statusColor = Status == "STAGED" ? Color.FromRgb(255, 200, 87) : Color.FromRgb(110, 255, 161);

        // Avalonia 11.3 does not expose DrawRoundedRectangle on DrawingContext.
        // Use normal rectangles for compatibility.
        context.FillRectangle(new SolidColorBrush(Color.FromArgb(150, 8, 15, 30)), new Rect(0, 0, w, h));
        context.DrawRectangle(new Pen(new SolidColorBrush(Color.FromArgb(120, 31, 106, 141)), 1), new Rect(0.5, 0.5, Math.Max(0, w - 1), Math.Max(0, h - 1)));

        var pulse = 0.55 + 0.45 * Math.Sin(TimeSeconds * 2.4);
        context.DrawEllipse(new SolidColorBrush(Color.FromArgb((byte)(80 + 80 * pulse), statusColor.R, statusColor.G, statusColor.B)), null, new Point(20, h / 2), 7, 7);

        var nameText = new FormattedText(DeviceName, System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Typeface.Default, 14, new SolidColorBrush(Color.FromRgb(217, 247, 255)));
        context.DrawText(nameText, new Point(36, 9));

        var statusText = new FormattedText(Status, System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Typeface.Default, 11, new SolidColorBrush(statusColor));
        context.DrawText(statusText, new Point(w - statusText.Width - 12, 11));

        var barWidth = Math.Max(0, Math.Min(1, Load)) * (w - 54);
        var barY = h - 13;
        context.FillRectangle(new SolidColorBrush(Color.FromArgb(70, 36, 215, 255)), new Rect(36, barY, Math.Max(0, w - 54), 4));
        context.FillRectangle(new SolidColorBrush(Color.FromArgb(210, statusColor.R, statusColor.G, statusColor.B)), new Rect(36, barY, barWidth, 4));
    }
}
