using Avalonia;
using Avalonia.Media;

namespace WhiteBrick.NOC.Widgets;

public sealed class ParticleFieldControl : AnimatedWidgetBase
{
    private readonly List<(double X, double Y, double Speed, double Size, double Phase, double Drift)> _particles = new();
    private readonly Random _random = new(42);

    public ParticleFieldControl()
    {
        for (var i = 0; i < 230; i++)
            _particles.Add((_random.NextDouble(), _random.NextDouble(), 0.012 + _random.NextDouble() * 0.055, 0.7 + _random.NextDouble() * 2.5, _random.NextDouble() * 10, -0.018 + _random.NextDouble() * 0.036));
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var b = Bounds;
        context.FillRectangle(new SolidColorBrush(Color.FromRgb(5, 8, 17)), b);

        for (int i = 0; i < 9; i++)
        {
            var y = ((TimeSeconds * 18 + i * 95) % Math.Max(1, b.Height));
            var pen = new Pen(new SolidColorBrush(Color.FromArgb(18, 36, 215, 255)), 1);
            context.DrawLine(pen, new Point(0, y), new Point(b.Width, y + 55));
        }

        foreach (var p in _particles)
        {
            var x = (p.X * b.Width + Math.Sin(TimeSeconds * 0.25 + p.Phase) * 18 + TimeSeconds * p.Drift * b.Width) % Math.Max(1, b.Width);
            if (x < 0) x += b.Width;
            var y = ((p.Y + TimeSeconds * p.Speed) % 1.0) * Math.Max(1, b.Height);
            var opacity = (byte)(45 + 115 * (0.5 + 0.5 * Math.Sin(TimeSeconds + p.Phase)));
            var brush = new SolidColorBrush(Color.FromArgb(opacity, 35, 216, 255));
            context.DrawEllipse(brush, null, new Point(x, y), p.Size, p.Size);
        }
    }
}
