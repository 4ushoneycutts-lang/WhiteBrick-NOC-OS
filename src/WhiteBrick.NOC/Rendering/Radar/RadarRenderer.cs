using Avalonia;
using Avalonia.Media;

namespace WhiteBrick.NOC.Rendering.Radar;

public static class RadarRenderer
{
    private static readonly (double X, double Y, double Phase)[] Blips =
    {
        (.22, .35, 0),
        (.68, .28, 1.2),
        (.42, .72, 2.1),
        (.78, .66, 3.5),
        (.52, .46, 4.2),
        (.33, .56, 5.4)
    };

    public static void Draw(DrawingContext context, Rect bounds, double timeSeconds)
    {
        var w = bounds.Width;
        var h = bounds.Height;
        var cx = w / 2;
        var cy = h / 2;
        var r = Math.Min(w, h) * 0.42;

        var center = new Point(cx, cy);

        var outerGlow = new RadialGradientBrush
        {
            GradientStops =
            {
                new GradientStop(Color.FromArgb(45, 36, 215, 255), 0),
                new GradientStop(Color.FromArgb(18, 36, 215, 255), 0.55),
                new GradientStop(Color.FromArgb(0, 36, 215, 255), 1)
            }
        };

        context.DrawEllipse(outerGlow, null, center, r * 1.18, r * 1.18);

        var gridPen = new Pen(new SolidColorBrush(Color.FromArgb(95, 36, 215, 255)), 1);
        var sweepPen = new Pen(new SolidColorBrush(Color.FromArgb(220, 110, 255, 161)), 2);

        for (var i = 1; i <= 4; i++)
            context.DrawEllipse(null, gridPen, center, r * i / 4, r * i / 4);

        context.DrawLine(gridPen, new Point(cx - r, cy), new Point(cx + r, cy));
        context.DrawLine(gridPen, new Point(cx, cy - r), new Point(cx, cy + r));

        var angle = timeSeconds * 2.15;

        for (var trail = 0; trail < 10; trail++)
        {
            var a = angle - trail * 0.075;
            var alpha = (byte)Math.Max(8, 118 - trail * 12);
            var pen = new Pen(new SolidColorBrush(Color.FromArgb(alpha, 110, 255, 161)), 2);
            context.DrawLine(pen, center, new Point(cx + Math.Cos(a) * r, cy + Math.Sin(a) * r));
        }

        context.DrawLine(
            sweepPen,
            center,
            new Point(cx + Math.Cos(angle) * r, cy + Math.Sin(angle) * r));

        foreach (var blip in Blips)
        {
            var pulse = 0.5 + 0.5 * Math.Sin(timeSeconds * 2.0 + blip.Phase);
            var x = cx + (blip.X - 0.5) * r * 1.55;
            var y = cy + (blip.Y - 0.5) * r * 1.55;

            var glowBrush = new SolidColorBrush(Color.FromArgb((byte)(42 + 80 * pulse), 110, 255, 161));
            var coreBrush = new SolidColorBrush(Color.FromArgb((byte)(120 + 100 * pulse), 110, 255, 161));

            context.DrawEllipse(glowBrush, null, new Point(x, y), 8 + pulse * 7, 8 + pulse * 7);
            context.DrawEllipse(coreBrush, null, new Point(x, y), 3 + pulse * 3, 3 + pulse * 3);
        }
    }
}