using System;
using Avalonia;
using Avalonia.Media;

namespace WhiteBrick.NOC.Widgets;

public sealed class ContinentRenderer
{
    public void Render(DrawingContext context, double cx, double cy, double radius, double time)
    {
        var rotation = time * 0.18;
        var landBrush = new SolidColorBrush(Color.FromArgb(78, 110, 255, 161));
        var glowBrush = new SolidColorBrush(Color.FromArgb(34, 180, 255, 210));

        DrawLandMass(context, cx, cy, radius, rotation, landBrush, glowBrush, -0.55, -0.18, 0.28, 0.16);
        DrawLandMass(context, cx, cy, radius, rotation, landBrush, glowBrush, -0.36, 0.18, 0.18, 0.30);
        DrawLandMass(context, cx, cy, radius, rotation, landBrush, glowBrush, 0.10, -0.10, 0.30, 0.22);
        DrawLandMass(context, cx, cy, radius, rotation, landBrush, glowBrush, 0.36, 0.04, 0.34, 0.18);
        DrawLandMass(context, cx, cy, radius, rotation, landBrush, glowBrush, 0.42, 0.33, 0.14, 0.08);
    }

    private static void DrawLandMass(
        DrawingContext context,
        double cx,
        double cy,
        double r,
        double rotation,
        IBrush landBrush,
        IBrush glowBrush,
        double baseX,
        double baseY,
        double scaleX,
        double scaleY)
    {
        var x = baseX + Math.Sin(rotation + baseX * 3.0) * 0.18;

        if (x < -0.78 || x > 0.78)
            return;

        var depth = Math.Sqrt(Math.Max(0, 1 - x * x));
        var px = cx + x * r;
        var py = cy + baseY * r;

        var sx = r * scaleX * depth;
        var sy = r * scaleY;

        context.DrawEllipse(glowBrush, null, new Point(px, py), sx * 1.12, sy * 1.12);
        context.DrawEllipse(landBrush, null, new Point(px, py), sx, sy);
        context.DrawEllipse(landBrush, null, new Point(px + sx * 0.34, py - sy * 0.26), sx * 0.46, sy * 0.45);
        context.DrawEllipse(landBrush, null, new Point(px - sx * 0.30, py + sy * 0.22), sx * 0.42, sy * 0.40);
    }
}