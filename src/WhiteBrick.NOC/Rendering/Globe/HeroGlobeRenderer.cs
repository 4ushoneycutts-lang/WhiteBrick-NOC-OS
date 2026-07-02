using System;
using Avalonia;
using Avalonia.Media;


namespace WhiteBrick.NOC.Widgets;

public sealed class HeroGlobeRenderer
{
    private readonly AtmosphereRenderer _atmosphere = new();
    private readonly GridRenderer _grid = new();
    private readonly NetworkRenderer _network = new();
    private readonly TelemetryRenderer _telemetry = new();
    private readonly GlobeEffectsRenderer _effects = new();
    private readonly HudRenderer _hud = new();

    public void Render(DrawingContext context, Rect bounds, double time)
    {
        var w = bounds.Width;
        var h = bounds.Height;

        if (w <= 0 || h <= 0)
            return;

        var cx = w / 2;
        var cy = h / 2;
        var r = Math.Min(w, h) * 0.43;

        _atmosphere.Render(context, cx, cy, r, time);

        DrawGlobeBody(context, cx, cy, r);
        _grid.Render(context, cx, cy, r, time);
        DrawContinents(context, cx, cy, r, time);
        DrawUndersideShade(context, cx, cy, r);

        _telemetry.Render(context, cx, cy, r, time);
        _network.Render(context, cx, cy, r, time);
        _effects.Render(context, cx, cy, r, time);
        _hud.Render(context, cx, cy, r, time);
    }

    private static void DrawGlobeBody(DrawingContext context, double cx, double cy, double r)
    {
        var globeFill = new RadialGradientBrush
        {
            GradientStops =
            {
                new GradientStop(Color.FromArgb(210, 58, 200, 225), 0),
                new GradientStop(Color.FromArgb(165, 36, 115, 150), 0.56),
                new GradientStop(Color.FromArgb(34, 8, 18, 26), 1)
            }
        };

        context.DrawEllipse(globeFill, null, new Point(cx - r * 0.08, cy - r * 0.08), r, r);

        context.DrawEllipse(
            null,
            new Pen(new SolidColorBrush(Color.FromArgb(70, 36, 215, 255)), 6.0),
            new Point(cx, cy),
            r + 1.5,
            r + 1.5);

        context.DrawEllipse(
            null,
            new Pen(new SolidColorBrush(Color.FromArgb(235, 36, 215, 255)), 2.2),
            new Point(cx, cy),
            r,
            r);
    }

    private static void DrawContinents(DrawingContext context, double cx, double cy, double r, double time)
    {
        var brush = new SolidColorBrush(Color.FromArgb(72, 110, 255, 161));
        var hotBrush = new SolidColorBrush(Color.FromArgb(42, 180, 255, 210));
        var shift = Math.Sin(time * 0.45) * r * 0.18;

        context.DrawEllipse(brush, null, new Point(cx - r * 0.34 + shift, cy - r * 0.16), r * 0.16, r * 0.08);
        context.DrawEllipse(brush, null, new Point(cx - r * 0.11 + shift, cy + r * 0.08), r * 0.22, r * 0.11);
        context.DrawEllipse(brush, null, new Point(cx + r * 0.30 + shift, cy - r * 0.02), r * 0.19, r * 0.09);
        context.DrawEllipse(brush, null, new Point(cx + r * 0.18 + shift, cy + r * 0.28), r * 0.12, r * 0.06);

        context.DrawEllipse(hotBrush, null, new Point(cx - r * 0.21 + shift, cy - r * 0.03), r * 0.045, r * 0.025);
        context.DrawEllipse(hotBrush, null, new Point(cx + r * 0.36 + shift, cy + r * 0.11), r * 0.04, r * 0.022);
    }

    private static void DrawUndersideShade(DrawingContext context, double cx, double cy, double r)
    {
        context.DrawEllipse(
            new SolidColorBrush(Color.FromArgb(42, 0, 8, 16)),
            null,
            new Point(cx, cy + r * 0.18),
            r * 0.96,
            r * 0.78);
    }
}