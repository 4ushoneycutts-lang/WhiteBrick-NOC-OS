using Avalonia;
using Avalonia.Media;

namespace WhiteBrick.NOC.Widgets;

public sealed class RadarControl : AnimatedWidgetBase
{
    private readonly (double X, double Y, double Phase)[] _blips =
    {
        (.22,.35,0), (.68,.28,1.2), (.42,.72,2.1), (.78,.66,3.5), (.52,.46,4.2), (.33,.56,5.4)
    };

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var w = Bounds.Width;
        var h = Bounds.Height;
        var cx = w / 2;
        var cy = h / 2;
        var r = Math.Min(w, h) * 0.42;

        var gridPen = new Pen(new SolidColorBrush(Color.FromArgb(95, 36, 215, 255)), 1);
        var sweepPen = new Pen(new SolidColorBrush(Color.FromArgb(220, 110, 255, 161)), 2);

        for (int i = 1; i <= 4; i++)
            context.DrawEllipse(null, gridPen, new Point(cx, cy), r * i / 4, r * i / 4);

        context.DrawLine(gridPen, new Point(cx - r, cy), new Point(cx + r, cy));
        context.DrawLine(gridPen, new Point(cx, cy - r), new Point(cx, cy + r));

        var angle = TimeSeconds * 2.15;
        for (int t = 0; t < 8; t++)
        {
            var a = angle - t * 0.08;
            var alpha = (byte)Math.Max(8, 105 - t * 13);
            var pen = new Pen(new SolidColorBrush(Color.FromArgb(alpha, 110, 255, 161)), 2);
            context.DrawLine(pen, new Point(cx, cy), new Point(cx + Math.Cos(a) * r, cy + Math.Sin(a) * r));
        }

        context.DrawLine(sweepPen, new Point(cx, cy), new Point(cx + Math.Cos(angle) * r, cy + Math.Sin(angle) * r));

        foreach (var b in _blips)
        {
            var pulse = 0.5 + 0.5 * Math.Sin(TimeSeconds * 2.0 + b.Phase);
            var brush = new SolidColorBrush(Color.FromArgb((byte)(120 + 100 * pulse), 110, 255, 161));
            context.DrawEllipse(brush, null, new Point(cx + (b.X - .5) * r * 1.55, cy + (b.Y - .5) * r * 1.55), 3 + pulse * 3, 3 + pulse * 3);
        }
    }
}
