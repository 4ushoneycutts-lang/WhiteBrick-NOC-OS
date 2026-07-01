using Avalonia;
using Avalonia.Media;

namespace WhiteBrick.NOC.Widgets;

public sealed class GaugeRingControl : AnimatedWidgetBase
{
    public static readonly StyledProperty<double> ValueProperty =
        AvaloniaProperty.Register<GaugeRingControl, double>(nameof(Value), 75);

    public static readonly StyledProperty<string> LabelProperty =
        AvaloniaProperty.Register<GaugeRingControl, string>(nameof(Label), "GAUGE");

    public double Value { get => GetValue(ValueProperty); set => SetValue(ValueProperty, value); }
    public string Label { get => GetValue(LabelProperty); set => SetValue(LabelProperty, value); }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var cx = Bounds.Width / 2;
        var cy = Bounds.Height / 2;
        var r = Math.Min(Bounds.Width, Bounds.Height) * .34;
        var center = new Point(cx, cy);

        context.DrawEllipse(null, new Pen(new SolidColorBrush(Color.FromArgb(65, 36, 215, 255)), 8), center, r, r);

        var clamped = Math.Max(0, Math.Min(100, Value));
        var start = -Math.PI / 2;
        var end = start + Math.PI * 2 * clamped / 100.0;
        var points = new List<Point>();
        for (double a = start; a <= end; a += 0.05)
            points.Add(new Point(cx + Math.Cos(a) * r, cy + Math.Sin(a) * r));

        if (points.Count > 1)
        {
            var geo = new StreamGeometry();
            using var g = geo.Open();
            g.BeginFigure(points[0], false);
            foreach (var p in points.Skip(1)) g.LineTo(p);
            context.DrawGeometry(null, new Pen(new SolidColorBrush(Color.FromArgb(230, 110, 255, 161)), 8), geo);
        }

        var val = new FormattedText($"{clamped:0}", System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Typeface.Default, 25, new SolidColorBrush(Color.FromRgb(217, 247, 255)));
        context.DrawText(val, new Point(cx - val.Width/2, cy - val.Height/2 - 5));

        var lab = new FormattedText(Label, System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Typeface.Default, 10, new SolidColorBrush(Color.FromRgb(119, 200, 232)));
        context.DrawText(lab, new Point(cx - lab.Width/2, cy + 24));
    }
}
