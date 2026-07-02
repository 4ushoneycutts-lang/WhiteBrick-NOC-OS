using Avalonia;
using Avalonia.Media;
using WhiteBrick.NOC.Rendering.Gauges;

namespace WhiteBrick.NOC.Widgets;

public sealed class GaugeRingControl : AnimatedWidgetBase
{
    public static readonly StyledProperty<double> ValueProperty =
        AvaloniaProperty.Register<GaugeRingControl, double>(nameof(Value), 75);

    public static readonly StyledProperty<string> LabelProperty =
        AvaloniaProperty.Register<GaugeRingControl, string>(nameof(Label), "GAUGE");

    public double Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public string Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        GaugeRenderer.Draw(
            context,
            Bounds,
            Value,
            Label,
            TimeSeconds);
    }
}