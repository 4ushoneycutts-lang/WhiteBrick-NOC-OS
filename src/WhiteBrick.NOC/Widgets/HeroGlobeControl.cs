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

        var atmospherePulse = 0.55 + 0.22 * Math.Sin(TimeSeconds * 1.2 + 0.4);
        var halo = new RadialGradientBrush
        {
            GradientStops =
            {
                new GradientStop(Color.FromArgb((byte)(110 + 30 * atmospherePulse), 36, 215, 255), 0),
                new GradientStop(Color.FromArgb((byte)(44 + 16 * atmospherePulse), 36, 215, 255), 0.55),
                new GradientStop(Color.FromArgb(0, 36, 215, 255), 1)
            }
        };
        context.DrawEllipse(halo, null, new Point(cx, cy), r * 1.65, r * 1.65);

        var outerPen = new Pen(new SolidColorBrush(Color.FromArgb(230, 36, 215, 255)), 2.2);
        var innerPen = new Pen(new SolidColorBrush(Color.FromArgb(135, 110, 255, 245)), 1.2);
        context.DrawEllipse(null, outerPen, new Point(cx, cy), r, r);

        for (int i = -3; i <= 3; i++)
        {
            var yRadius = r * Math.Cos(i * Math.PI / 8);
            context.DrawEllipse(null, innerPen, new Point(cx, cy), r, Math.Abs(yRadius));
        }

        for (int i = 0; i < 10; i++)
        {
            var angle = TimeSeconds * 0.45 + i * Math.PI / 10;
            var xScale = 0.7 + 0.3 * Math.Abs(Math.Cos(angle));
            context.DrawEllipse(null, innerPen, new Point(cx, cy), r * xScale, r * 0.95);
        }

        DrawContinents(context, cx, cy, r);
        DrawOrbitalRing(context, cx, cy, r * 1.12, 0.26, TimeSeconds * 1.35, 0.0, Color.FromArgb(190, 120, 248, 255));
        DrawOrbitalRing(context, cx, cy, r * 1.28, 0.18, -TimeSeconds * 1.05 + 1.2, 0.9, Color.FromArgb(185, 110, 255, 161));
        DrawOrbitalRing(context, cx, cy, r * 1.46, 0.14, TimeSeconds * 0.82 + 2.1, 1.6, Color.FromArgb(175, 166, 126, 255));

        DrawPacketArc(context, cx, cy, r, TimeSeconds * 0.68, Color.FromArgb(165, 36, 215, 255));
        DrawPacketArc(context, cx, cy, r, TimeSeconds * 0.9 + 2.1, Color.FromArgb(165, 142, 248, 255));
        DrawPacketArc(context, cx, cy, r, -TimeSeconds * 0.78 + 1.4, Color.FromArgb(165, 110, 255, 161));

        var beaconPulse = 0.55 + 0.45 * Math.Sin(TimeSeconds * 3.2);
        var beaconCenter = new Point(cx + r * 0.24, cy - r * 0.18);
        var beaconHalo = new SolidColorBrush(Color.FromArgb((byte)(70 + 70 * beaconPulse), 110, 255, 161));
        context.DrawEllipse(beaconHalo, null, beaconCenter, 12 + beaconPulse * 8, 12 + beaconPulse * 8);

        var beaconBrush = new SolidColorBrush(Color.FromArgb((byte)(145 + 85 * beaconPulse), 110, 255, 161));
        context.DrawEllipse(beaconBrush, null, beaconCenter, 6 + beaconPulse * 6, 6 + beaconPulse * 6);

        var statusText = new FormattedText("HOME BEACON / ROUTE STABLE", System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Typeface.Default, 13, new SolidColorBrush(Color.FromRgb(217, 247, 255)));
        context.DrawText(statusText, new Point(cx - statusText.Width / 2, cy + r + 24));

        var subText = new FormattedText("WB-CORE • HONEYCUTT • WAN LINK", System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Typeface.Default, 10, new SolidColorBrush(Color.FromArgb(180, 119, 200, 232)));
        context.DrawText(subText, new Point(cx - subText.Width / 2, cy + r + 44));
    }

    private void DrawContinents(DrawingContext context, double cx, double cy, double r)
    {
        var brush = new SolidColorBrush(Color.FromArgb(70, 110, 255, 161));
        var shift = Math.Sin(TimeSeconds * 0.45) * r * 0.18;
        context.DrawEllipse(brush, null, new Point(cx - r * 0.34 + shift, cy - r * 0.16), r * 0.16, r * 0.08);
        context.DrawEllipse(brush, null, new Point(cx - r * 0.11 + shift, cy + r * 0.08), r * 0.22, r * 0.11);
        context.DrawEllipse(brush, null, new Point(cx + r * 0.30 + shift, cy - r * 0.02), r * 0.19, r * 0.09);
        context.DrawEllipse(brush, null, new Point(cx + r * 0.18 + shift, cy + r * 0.28), r * 0.12, r * 0.06);
    }

    private static void DrawOrbitalRing(DrawingContext context, double cx, double cy, double radius, double verticalScale, double angle, double phase, Color color)
    {
        var ringColor = Color.FromArgb(65, color.R, color.G, color.B);
        context.DrawEllipse(null, new Pen(new SolidColorBrush(ringColor), 1), new Point(cx, cy), radius, radius * verticalScale);

        var x = cx + Math.Cos(angle + phase) * radius;
        var y = cy + Math.Sin(angle + phase) * radius * verticalScale;
        context.DrawEllipse(new SolidColorBrush(color), null, new Point(x, y), 4.3, 4.3);
    }

    private static void DrawPacketArc(DrawingContext context, double cx, double cy, double r, double phase, Color color)
    {
        var pen = new Pen(new SolidColorBrush(color), 1.6);
        var glowPen = new Pen(new SolidColorBrush(Color.FromArgb(70, color.R, color.G, color.B)), 4.2);

        for (int i = 0; i < 5; i++)
        {
            var a = phase + i * 0.10;
            var p1 = new Point(cx + Math.Cos(a) * r * 1.10, cy + Math.Sin(a) * r * 0.60);
            var p2 = new Point(cx + Math.Cos(a + 0.08) * r * 1.10, cy + Math.Sin(a + 0.08) * r * 0.60);
            context.DrawLine(glowPen, p1, p2);
            context.DrawLine(pen, p1, p2);
        }

        var packetPhase = (phase + 0.24) % (Math.PI * 2);
        var packetX = cx + Math.Cos(packetPhase) * r * 1.10;
        var packetY = cy + Math.Sin(packetPhase) * r * 0.60;
        context.DrawEllipse(new SolidColorBrush(Color.FromArgb(220, 217, 247, 255)), null, new Point(packetX, packetY), 2.8, 2.8);
    }
}
