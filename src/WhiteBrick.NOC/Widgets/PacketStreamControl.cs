using Avalonia;
using Avalonia.Media;

namespace WhiteBrick.NOC.Widgets;

public sealed class PacketStreamControl : AnimatedWidgetBase
{
    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var w = Bounds.Width;
        var h = Bounds.Height;
        var nodes = new[]
        {
            new Point(w * .10, h * .50),
            new Point(w * .28, h * .32),
            new Point(w * .46, h * .58),
            new Point(w * .64, h * .36),
            new Point(w * .84, h * .50)
        };

        var linePen = new Pen(new SolidColorBrush(Color.FromArgb(95, 36, 215, 255)), 2);
        for (int i = 0; i < nodes.Length - 1; i++)
            context.DrawLine(linePen, nodes[i], nodes[i + 1]);

        for (int i = 0; i < nodes.Length; i++)
        {
            context.DrawEllipse(new SolidColorBrush(Color.FromArgb(70, 36, 215, 255)), null, nodes[i], 18, 18);
            context.DrawEllipse(new SolidColorBrush(Color.FromArgb(230, 110, 255, 161)), null, nodes[i], 5, 5);
        }

        for (int stream = 0; stream < 14; stream++)
        {
            var t = (TimeSeconds * 0.38 + stream / 14.0) % 1.0;
            var segFloat = t * (nodes.Length - 1);
            var seg = Math.Min(nodes.Length - 2, (int)Math.Floor(segFloat));
            var local = segFloat - seg;

            var a = nodes[seg];
            var b = nodes[seg + 1];
            var x = a.X + (b.X - a.X) * local;
            var y = a.Y + (b.Y - a.Y) * local;

            var alpha = (byte)(110 + 100 * Math.Sin((t + .2) * Math.PI));
            context.DrawEllipse(new SolidColorBrush(Color.FromArgb(alpha, 142, 248, 255)), null, new Point(x, y), 3.5, 3.5);
        }

        var label = new FormattedText("PACKET ROUTE: INTERNET → GATEWAY → WIFI → STORAGE → WB", System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Typeface.Default, 12, new SolidColorBrush(Color.FromArgb(190, 217, 247, 255)));
        context.DrawText(label, new Point(12, 10));
    }
}
