using Avalonia;
using Avalonia.Media;
using System.Collections;

namespace WhiteBrick.NOC.Widgets;

public sealed class TimelineStreamControl : AnimatedWidgetBase
{
    public static readonly StyledProperty<IEnumerable?> EventsProperty =
        AvaloniaProperty.Register<TimelineStreamControl, IEnumerable?>(nameof(Events));

    public IEnumerable? Events
    {
        get => GetValue(EventsProperty);
        set => SetValue(EventsProperty, value);
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var w = Bounds.Width;
        var h = Bounds.Height;

        context.FillRectangle(new SolidColorBrush(Color.FromArgb(95, 5, 8, 17)), Bounds);
        context.DrawRectangle(new Pen(new SolidColorBrush(Color.FromArgb(85, 36, 215, 255)), 1), new Rect(0.5, 0.5, Math.Max(0, w - 1), Math.Max(0, h - 1)));

        var title = new FormattedText("SYSTEM TIMELINE", System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Typeface.Default, 12, new SolidColorBrush(Color.FromRgb(189, 238, 255)));
        context.DrawText(title, new Point(10, 8));

        var linePen = new Pen(new SolidColorBrush(Color.FromArgb(90, 36, 215, 255)), 1);
        var xLine = 18.0;
        context.DrawLine(linePen, new Point(xLine, 32), new Point(xLine, h - 10));

        var items = new List<string>();
        if (Events is not null)
        {
            foreach (var item in Events)
            {
                if (item is not null)
                    items.Add(item.ToString() ?? string.Empty);
            }
        }

        var maxItems = Math.Min(7, items.Count);
        for (int i = 0; i < maxItems; i++)
        {
            var y = 38 + i * 21;
            var pulse = 0.5 + 0.5 * Math.Sin(TimeSeconds * 2.2 + i * 0.8);
            var dot = new SolidColorBrush(Color.FromArgb((byte)(130 + 80 * pulse), 110, 255, 161));
            context.DrawEllipse(dot, null, new Point(xLine, y + 4), 3.5, 3.5);

            var itemColor = items[i].Contains("WARN", StringComparison.OrdinalIgnoreCase)
                ? Color.FromRgb(255, 200, 87)
                : items[i].Contains("WB", StringComparison.OrdinalIgnoreCase)
                    ? Color.FromRgb(184, 168, 255)
                    : Color.FromRgb(217, 247, 255);

            var text = new FormattedText(items[i], System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Typeface.Default, 11, new SolidColorBrush(Color.FromArgb(220, itemColor.R, itemColor.G, itemColor.B)));
            context.DrawText(text, new Point(30, y - 3));
        }
    }
}
