using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;

namespace WhiteBrick.NOC.Widgets;

public abstract class AnimatedWidgetBase : Control
{
    private readonly DispatcherTimer _timer;
    protected double TimeSeconds { get; private set; }

    protected AnimatedWidgetBase()
    {
        _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) };
        _timer.Tick += (_, _) =>
        {
            TimeSeconds += 0.016;
            InvalidateVisual();
        };
        AttachedToVisualTree += (_, _) => _timer.Start();
        DetachedFromVisualTree += (_, _) => _timer.Stop();
    }
}
