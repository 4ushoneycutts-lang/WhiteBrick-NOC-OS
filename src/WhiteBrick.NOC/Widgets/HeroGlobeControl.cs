using Avalonia;
using Avalonia.Media;

namespace WhiteBrick.NOC.Widgets;

public sealed class HeroGlobeControl : AnimatedWidgetBase
{
    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var w = Bounds.Width;
        var h = Bounds.Height;
        var cx = w / 2;
        var cy = h / 2;
        var r = Math.Min(w, h) * 0.34;

        var halo = new RadialGradientBrush
        {
            GradientStops =
            {
                new GradientStop(Color.FromArgb(96, 36, 215, 255), 0),
                new GradientStop(Color.FromArgb(25, 36, 215, 255), 0.55),
                new GradientStop(Color.FromArgb(0, 36, 215, 255), 1)
            }
        };
        context.DrawEllipse(halo, null, new Point(cx, cy), r * 1.55, r * 1.55);

        var outerPen = new Pen(new SolidColorBrush(Color.FromArgb(230, 36, 215, 255)), 2);
        var innerPen = new Pen(new SolidColorBrush(Color.FromArgb(125, 110, 255, 245)), 1);
        context.DrawEllipse(null, outerPen, new Point(cx, cy), r, r);

        for (int i = -3; i <= 3; i++)
        {
            var yRadius = r * Math.Cos(i * Math.PI / 8);
            context.DrawEllipse(null, innerPen, new Point(cx, cy), r, Math.Abs(yRadius));
        }

        for (int i = 0; i < 14; i++)
        {
            var angle = TimeSeconds * 0.55 + i * Math.PI / 14;
            var xScale = Math.Abs(Math.Cos(angle));
            context.DrawEllipse(null, innerPen, new Point(cx, cy), r * xScale, r);
        }

        DrawContinents(context, cx, cy, r);
        DrawOrbit(context, cx, cy, r * 1.18, TimeSeconds * 1.4, Color.FromArgb(185, 120, 248, 255));
        DrawOrbit(context, cx, cy, r * 1.33, -TimeSeconds * 1.1 + 1.5, Color.FromArgb(175, 110, 255, 161));
        DrawOrbit(context, cx, cy, r * 1.52, TimeSeconds * 0.9 + 2.2, Color.FromArgb(165, 166, 126, 255));

        DrawPacketArc(context, cx, cy, r, TimeSeconds * 0.7);
        DrawPacketArc(context, cx, cy, r, TimeSeconds * 0.9 + 2.4);

        var beaconPulse = 0.55 + 0.45 * Math.Sin(TimeSeconds * 3.2);
        var beaconBrush = new SolidColorBrush(Color.FromArgb((byte)(145 + 85 * beaconPulse), 110, 255, 161));
        context.DrawEllipse(beaconBrush, null, new Point(cx + r * 0.24, cy - r * 0.18), 6 + beaconPulse * 6, 6 + beaconPulse * 6);

        var text = new FormattedText("HONEYCUTT HOME BEACON", System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Typeface.Default, 15, new SolidColorBrush(Color.FromRgb(217, 247, 255)));
        context.DrawText(text, new Point(cx - text.Width / 2, cy + r + 28));
    }

    private void DrawContinents(DrawingContext context, double cx, double cy, double r)
    {
        var brush = new SolidColorBrush(Color.FromArgb(55, 110, 255, 161));
        var shift = Math.Sin(TimeSeconds * 0.45) * r * 0.18;
        context.DrawEllipse(brush, null, new Point(cx - r * 0.34 + shift, cy - r * 0.16), r * 0.16, r * 0.08);
        context.DrawEllipse(brush, null, new Point(cx - r * 0.10 + shift, cy + r * 0.08), r * 0.22, r * 0.11);
        context.DrawEllipse(brush, null, new Point(cx + r * 0.30 + shift, cy - r * 0.02), r * 0.19, r * 0.09);
        context.DrawEllipse(brush, null, new Point(cx + r * 0.18 + shift, cy + r * 0.28), r * 0.12, r * 0.06);
    }

    private static void DrawOrbit(DrawingContext context, double cx, double cy, double radius, double angle, Color color)
    {
        context.DrawEllipse(null, new Pen(new SolidColorBrush(Color.FromArgb(65, color.R, color.G, color.B)), 1), new Point(cx, cy), radius, radius * 0.28);
        var x = cx + Math.Cos(angle) * radius;
        var y = cy + Math.Sin(angle) * radius * 0.28;
        context.DrawEllipse(new SolidColorBrush(color), null, new Point(x, y), 4.5, 4.5);
    }

    private static void DrawPacketArc(DrawingContext context, double cx, double cy, double r, double phase)
    {
        var pen = new Pen(new SolidColorBrush(Color.FromArgb(160, 36, 215, 255)), 2);
        for (int i = 0; i < 5; i++)
        {
            var a = phase + i * 0.12;
            var p1 = new Point(cx + Math.Cos(a) * r * 1.08, cy + Math.Sin(a) * r * 0.58);
            var p2 = new Point(cx + Math.Cos(a + 0.08) * r * 1.08, cy + Math.Sin(a + 0.08) * r * 0.58);
            context.DrawLine(pen, p1, p2);
        }
    }
}
