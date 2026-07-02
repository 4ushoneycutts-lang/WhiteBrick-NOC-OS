using Avalonia.Media;
using WhiteBrick.NOC.Rendering.Radar;

namespace WhiteBrick.NOC.Widgets;

public sealed class RadarControl : AnimatedWidgetBase
{
    public override void Render(DrawingContext context)
    {
        base.Render(context);

        RadarRenderer.Draw(
            context,
            Bounds,
            TimeSeconds);
    }
}