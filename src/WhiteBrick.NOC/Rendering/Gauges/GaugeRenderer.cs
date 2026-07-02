using Avalonia;
using Avalonia.Media;

namespace WhiteBrick.NOC.Rendering.Gauges;

public static class GaugeRenderer
{
    public static void Draw(
        DrawingContext context,
        Rect bounds,
        double value,
        string label,
        double timeSeconds)
    {
        var cx = bounds.Width / 2;
        var cy = bounds.Height / 2;
        var r = Math.Min(bounds.Width, bounds.Height) * 0.34;
        var center = new Point(cx, cy);

        var clamped = Math.Max(0, Math.Min(100, value));
        var pulse = 0.55 + 0.45 * Math.Sin(timeSeconds * 2.0);

        var trackPen = new Pen(
            new SolidColorBrush(Color.FromArgb(65, 36, 215, 255)),
            8);

        context.DrawEllipse(null, trackPen, center, r, r);

        var glowPen = new Pen(
            new SolidColorBrush(Color.FromArgb((byte)(40 + 45 * pulse), 110, 255, 161)),
            13);

        DrawArc(context, center, r, clamped, glowPen);

        var valuePen = new Pen(
            new SolidColorBrush(Color.FromArgb(230, 110, 255, 161)),
            8);

        DrawArc(context, center, r, clamped, valuePen);

        var valueText = new FormattedText(
            $"{clamped:0}",
            System.Globalization.CultureInfo.InvariantCulture,
            FlowDirection.LeftToRight,
            Typeface.Default,
            25,
            new SolidColorBrush(Color.FromRgb(217, 247, 255)));

        context.DrawText(
            valueText,
            new Point(cx - valueText.Width / 2, cy - valueText.Height / 2 - 5));

        var labelText = new FormattedText(
            label,
            System.Globalization.CultureInfo.InvariantCulture,
            FlowDirection.LeftToRight,
            Typeface.Default,
            10,
            new SolidColorBrush(Color.FromRgb(119, 200, 232)));

        context.DrawText(
            labelText,
            new Point(cx - labelText.Width / 2, cy + 24));
    }

    private static void DrawArc(
        DrawingContext context,
        Point center,
        double radius,
        double value,
        Pen pen)
    {
        var start = -Math.PI / 2;
        var end = start + Math.PI * 2 * value / 100.0;

        var points = new List<Point>();

        for (double a = start; a <= end; a += 0.05)
            points.Add(new Point(
                center.X + Math.Cos(a) * radius,
                center.Y + Math.Sin(a) * radius));

        if (points.Count <= 1)
            return;

        var geometry = new StreamGeometry();

        using var g = geometry.Open();
        g.BeginFigure(points[0], false);

        foreach (var point in points.Skip(1))
            g.LineTo(point);

        context.DrawGeometry(null, pen, geometry);
    }
}