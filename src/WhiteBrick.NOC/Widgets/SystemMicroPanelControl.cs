using Avalonia;
using Avalonia.Media;

namespace WhiteBrick.NOC.Widgets;

public sealed class SystemMicroPanelControl : AnimatedWidgetBase
{
    public static readonly StyledProperty<string> CpuProperty =
        AvaloniaProperty.Register<SystemMicroPanelControl, string>(nameof(Cpu), "0%");

    public static readonly StyledProperty<string> MemoryProperty =
        AvaloniaProperty.Register<SystemMicroPanelControl, string>(nameof(Memory), "0%");

    public static readonly StyledProperty<string> PacketsProperty =
        AvaloniaProperty.Register<SystemMicroPanelControl, string>(nameof(Packets), "0");

    public static readonly StyledProperty<string> ModeProperty =
        AvaloniaProperty.Register<SystemMicroPanelControl, string>(nameof(Mode), "OPERATIONS");

    public string Cpu { get => GetValue(CpuProperty); set => SetValue(CpuProperty, value); }
    public string Memory { get => GetValue(MemoryProperty); set => SetValue(MemoryProperty, value); }
    public string Packets { get => GetValue(PacketsProperty); set => SetValue(PacketsProperty, value); }
    public string Mode { get => GetValue(ModeProperty); set => SetValue(ModeProperty, value); }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var w = Bounds.Width;
        var h = Bounds.Height;
        context.FillRectangle(new SolidColorBrush(Color.FromArgb(118, 8, 15, 30)), new Rect(0, 0, w, h));
        context.DrawRectangle(new Pen(new SolidColorBrush(Color.FromArgb(110, 36, 215, 255)), 1), new Rect(0.5, 0.5, Math.Max(0, w - 1), Math.Max(0, h - 1)));

        DrawRow(context, "CPU", Cpu, 14, 14, w);
        DrawRow(context, "MEM", Memory, 14, 42, w);
        DrawRow(context, "PKT", Packets, 14, 70, w);
        DrawRow(context, "MODE", Mode, 14, 98, w);

        for (int i = 0; i < 24; i++)
        {
            var x = 150 + i * 7;
            var amp = 6 + 14 * (0.5 + 0.5 * Math.Sin(TimeSeconds * 2.0 + i * 0.6));
            var y = h - 16;
            context.DrawLine(new Pen(new SolidColorBrush(Color.FromArgb(135, 184, 168, 255)), 1), new Point(x, y), new Point(x, y - amp));
        }
    }

    private static void DrawRow(DrawingContext context, string label, string value, double x, double y, double w)
    {
        var labelText = new FormattedText(label, System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Typeface.Default, 12, new SolidColorBrush(Color.FromRgb(119, 200, 232)));
        context.DrawText(labelText, new Point(x, y));

        var valText = new FormattedText(value, System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Typeface.Default, 14, new SolidColorBrush(Color.FromRgb(217, 247, 255)));
        context.DrawText(valText, new Point(w - valText.Width - 14, y - 2));
    }
}
