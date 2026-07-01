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

        var t = TimeSeconds;
        var atmospherePulse = 0.55 + 0.22 * Math.Sin(t * 1.2 + 0.4);
        var drift = 0.08 * Math.Sin(t * 0.7 + 0.5);
        var sweep = t * 0.86 + drift;
        var halo = new RadialGradientBrush
        {
            GradientStops =
            {
                new GradientStop(Color.FromArgb((byte)(90 + 40 * atmospherePulse), 36, 215, 255), 0),
                new GradientStop(Color.FromArgb((byte)(36 + 18 * atmospherePulse), 36, 215, 255), 0.6),
                new GradientStop(Color.FromArgb(0, 36, 215, 255), 1)
            }
        };
        context.DrawEllipse(halo, null, new Point(cx, cy), r * 1.8, r * 1.8);

        var globeFill = new RadialGradientBrush
        {
            GradientStops =
            {
                new GradientStop(Color.FromArgb(200, 58, 200, 225), 0),
                new GradientStop(Color.FromArgb(160, 36, 115, 150), 0.6),
                new GradientStop(Color.FromArgb(30, 8, 18, 26), 1)
            }
        };
        context.DrawEllipse(globeFill, null, new Point(cx - r * 0.08, cy - r * 0.08), r, r);

        var outerPen = new Pen(new SolidColorBrush(Color.FromArgb(230, 36, 215, 255)), 2.2);
        var innerPen = new Pen(new SolidColorBrush(Color.FromArgb(135, 110, 255, 245)), 1.2);
        context.DrawEllipse(null, outerPen, new Point(cx, cy), r, r);

        var edgeBreath = 0.5 + 0.28 * Math.Sin(t * 1.6 + 0.3);
        for (int g = 0; g < 3; g++)
        {
            var width = 1.8 + g * 2.4;
            var alpha = (byte)(18 * (1.0 / (g + 1)) * (0.8 + edgeBreath));
            var pen = new Pen(new SolidColorBrush(Color.FromArgb(alpha, 36, 215, 255)), width);
            context.DrawEllipse(null, pen, new Point(cx, cy), r + 1.2 + g * 2.2, r + 1.2 + g * 2.2);
        }

        for (int i = -3; i <= 3; i++)
        {
            var yRadius = r * Math.Cos(i * Math.PI / 8);
            context.DrawEllipse(null, innerPen, new Point(cx, cy), r, Math.Abs(yRadius));
        }

        for (int i = 0; i < 10; i++)
        {
            var angle = sweep * 0.52 + i * Math.PI / 10;
            var xScale = 0.72 + 0.28 * Math.Abs(Math.Cos(angle));
            var yScale = 0.92 + 0.08 * Math.Sin(angle * 0.7 + 0.5);
            context.DrawEllipse(null, innerPen, new Point(cx, cy), r * xScale, r * yScale);
        }

        DrawContinents(context, cx, cy, r);
        var undersideShade = new SolidColorBrush(Color.FromArgb(40, 0, 8, 16));
        context.DrawEllipse(undersideShade, null, new Point(cx, cy + r * 0.18), r * 0.96, r * 0.78);
        DrawOrbitalRing(context, cx, cy, r * 1.12, 0.26, sweep * 1.18, 0.15, Color.FromArgb(190, 120, 248, 255));
        DrawOrbitalRing(context, cx, cy, r * 1.28, 0.18, -sweep * 0.92 + 1.2, 0.55, Color.FromArgb(185, 110, 255, 161));
        DrawOrbitalRing(context, cx, cy, r * 1.46, 0.14, sweep * 0.72 + 2.1, 1.0, Color.FromArgb(175, 166, 126, 255));

        DrawPacketArc(context, cx, cy, r, sweep * 0.72 + 0.15, Color.FromArgb(165, 36, 215, 255));
        DrawPacketArc(context, cx, cy, r, sweep * 0.85 + 2.1, Color.FromArgb(165, 142, 248, 255));
        DrawPacketArc(context, cx, cy, r, -sweep * 0.64 + 1.4, Color.FromArgb(165, 110, 255, 161));

        // Subtle network activity: faint connection arcs, occasional packets, pulsing nodes.
        // Use multiple arcs with slightly different radii and speeds; timing uses varying frequencies
        // to avoid strict repetition while remaining deterministic and low-cost.
        var networkBaseColors = new[] { Color.FromArgb(120, 110, 255, 200), Color.FromArgb(100, 120, 220, 255), Color.FromArgb(90, 160, 200, 255) };
        int netArcs = 4;
        for (int ai = 0; ai < netArcs; ai++)
        {
            var nr = r * (1.02 + ai * 0.14);
            var nvs = 0.58 + ai * 0.06;
            var nSpeed = 0.42 + ai * 0.18;
            var nPhase = sweep * (0.6 + ai * 0.2) * (ai % 2 == 0 ? 1 : -1);

            var baseColor = networkBaseColors[ai % networkBaseColors.Length];
            var glowPen = new Pen(new SolidColorBrush(Color.FromArgb(30, baseColor.R, baseColor.G, baseColor.B)), 5.0);
            var thinPen = new Pen(new SolidColorBrush(Color.FromArgb(70, baseColor.R, baseColor.G, baseColor.B)), 1.0);

            // draw faint segmented arc
            int segs = 18;
            for (int s = 0; s < segs; s++)
            {
                var a1 = nPhase + (s / (double)segs) * Math.PI * 2 * 0.7;
                var a2 = nPhase + ((s + 1) / (double)segs) * Math.PI * 2 * 0.7;
                var p1 = new Point(cx + Math.Cos(a1) * nr, cy + Math.Sin(a1) * nr * nvs);
                var p2 = new Point(cx + Math.Cos(a2) * nr, cy + Math.Sin(a2) * nr * nvs);
                context.DrawLine(glowPen, p1, p2);
                context.DrawLine(thinPen, p1, p2);
            }

            // occasional moving packet: progress varies by time and arc index to avoid sync
            var prog = (Math.Sin(TimeSeconds * (nSpeed * 0.9 + ai * 0.07) + ai * 1.3) + 1.0) * 0.5; // 0..1
            var packetAngle = nPhase + prog * Math.PI * 2 * 0.7;
            var px = cx + Math.Cos(packetAngle) * nr;
            var py = cy + Math.Sin(packetAngle) * nr * nvs;
            var packetGlow = new SolidColorBrush(Color.FromArgb((byte)(100 + 80 * prog), baseColor.R, baseColor.G, baseColor.B));
            context.DrawEllipse(new SolidColorBrush(Color.FromArgb((byte)(180 * prog), 235, 255, 250)), null, new Point(px, py), 3.2, 3.2);
            context.DrawEllipse(packetGlow, null, new Point(px, py), 6 + 2 * prog, 6 + 2 * prog);

            // a few pulsing nodes along the arc
            int nodes = 3;
            for (int ni = 0; ni < nodes; ni++)
            {
                var npos = ni / (double)nodes;
                var nangle = nPhase + npos * Math.PI * 2 * 0.7 + Math.Sin(TimeSeconds * (0.6 + ni * 0.2) + ai) * 0.18;
                var nx = cx + Math.Cos(nangle) * nr;
                var ny = cy + Math.Sin(nangle) * nr * nvs;
                var pulse = 0.4 + 0.6 * Math.Abs(Math.Sin(TimeSeconds * (1.2 + ni * 0.3) + ai * 0.9 + ni));
                var nodeBrush = new SolidColorBrush(Color.FromArgb((byte)(60 + 120 * pulse), baseColor.R, baseColor.G, baseColor.B));
                context.DrawEllipse(nodeBrush, null, new Point(nx, ny), 2.0 + 1.6 * pulse, 2.0 + 1.6 * pulse);
            }
        }

        var beaconPulse = 0.55 + 0.45 * Math.Sin(t * 3.2 + 0.4);

        // Telemetry data rings (Sprint 4): very faint orbital data rings with slow rotation
        // and occasional tiny telemetry indicators traveling along them. High transparency
        // and low geometry count keep CPU usage minimal.
        int telemetryRings = 3;
        var telemetryColors = new[] { Color.FromArgb(28, 180, 220, 255), Color.FromArgb(24, 160, 210, 230), Color.FromArgb(20, 140, 200, 220) };
        for (int ri = 0; ri < telemetryRings; ri++)
        {
            var tr = r * (1.06 + ri * 0.12);
            var tvs = 0.20 + ri * 0.06; // flattened ring
            var dir = ri % 2 == 0 ? 1.0 : -1.0;
            var speed = 0.08 + ri * 0.03; // very slow
            var seed = ri * 1.37;
            var ringAngle = dir * (t * speed + Math.Sin(t * (0.05 + ri * 0.02) + seed) * 0.02);

            // Draw faint single-stroke ring
            var ringPen = new Pen(new SolidColorBrush(telemetryColors[ri % telemetryColors.Length]), 1.0);
            context.DrawEllipse(null, ringPen, new Point(cx, cy), tr, tr * tvs);

            // Draw 1-2 tiny telemetry indicators traveling along the ring. Alpha varies so they
            // appear and fade gently rather than pop in/out abruptly.
            int indicators = 1 + (ri % 2);
            for (int ind = 0; ind < indicators; ind++)
            {
                var localOffset = ind * (Math.PI * 0.9 / Math.Max(1, indicators));
                var indSpeed = speed * (1.0 + 0.2 * ind) * (0.8 + 0.15 * Math.Sin(seed + ind));
                var indProg = (t * indSpeed + seed * 0.23) % (Math.PI * 2);
                var ia = ringAngle + localOffset + indProg;
                var ix = cx + Math.Cos(ia) * tr;
                var iy = cy + Math.Sin(ia) * tr * tvs;

                // occasional subtle pulsing visibility
                var vis = 0.35 + 0.65 * Math.Abs(Math.Sin(t * (0.9 + ri * 0.15) + ind * 0.4 + seed));
                var indBrush = new SolidColorBrush(Color.FromArgb((byte)(40 + 120 * vis), 225, 245, 255));
                context.DrawEllipse(indBrush, null, new Point(ix, iy), 2.2, 2.2);
                context.DrawEllipse(new SolidColorBrush(Color.FromArgb((byte)(30 * vis), 180, 220, 255)), null, new Point(ix, iy), 5.0 * vis, 2.0 * vis);
            }
        }
        var beaconDrift = 0.6 + 0.4 * Math.Sin(t * 1.1 + 0.2);
        var beaconCenter = new Point(cx + r * (0.24 + 0.02 * Math.Sin(t * 0.8)), cy - r * (0.18 + 0.01 * Math.Cos(t * 0.9)));
        var beaconHalo = new SolidColorBrush(Color.FromArgb((byte)(70 + 70 * beaconPulse), 110, 255, 161));
        context.DrawEllipse(beaconHalo, null, beaconCenter, 12 + beaconPulse * 8, 12 + beaconPulse * 8);

        var beaconBrush = new SolidColorBrush(Color.FromArgb((byte)(145 + 85 * beaconPulse * beaconDrift), 110, 255, 161));
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
