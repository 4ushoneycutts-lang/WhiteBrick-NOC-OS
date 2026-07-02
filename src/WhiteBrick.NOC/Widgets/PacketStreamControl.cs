using Avalonia.Media;
using WhiteBrick.NOC.Rendering.Packets;

namespace WhiteBrick.NOC.Widgets;

public sealed class PacketStreamControl : AnimatedWidgetBase
{
    public override void Render(DrawingContext context)
    {
        base.Render(context);

        PacketRenderer.Draw(
            context,
            Bounds,
            TimeSeconds);
    }
}