using Avalonia;
using Avalonia.Media;

namespace WhiteBrick.NOC.Rendering.Packets;

public static class PacketRenderer
{
    public static void Draw(DrawingContext context, Rect bounds, double timeSeconds)
    {
        var w = bounds.Width;
        var h = bounds.Height;

        var nodes = new[]
        {
            new Point(w * .10, h * .50),
            new Point(w * .28, h * .32),
            new Point(w * .46, h * .58),
            new Point(w * .64, h * .36),
            new Point(w * .84, h * .50)
        };

        DrawLabel(context);
        DrawRouteLines(context, nodes);
        DrawNodes(context, nodes, timeSeconds);
        DrawPacketStreams(context, nodes, timeSeconds);
    }

    private static void DrawLabel(DrawingContext context)
    {
        var label = new FormattedText(
            "PACKET ROUTE: INTERNET → GATEWAY → WIFI → STORAGE → WB",
            System.Globalization.CultureInfo.InvariantCulture,
            FlowDirection.LeftToRight,
            Typeface.Default,
            12,
            new SolidColorBrush(Color.FromArgb(190, 217, 247, 255)));

        context.DrawText(label, new Point(12, 10));
    }

    private static void DrawRouteLines(DrawingContext context, IReadOnlyList<Point> nodes)
    {
        var glowPen = new Pen(new SolidColorBrush(Color.FromArgb(35, 36, 215, 255)), 7);
        var linePen = new Pen(new SolidColorBrush(Color.FromArgb(105, 36, 215, 255)), 2);

        for (var i = 0; i < nodes.Count - 1; i++)
        {
            context.DrawLine(glowPen, nodes[i], nodes[i + 1]);
            context.DrawLine(linePen, nodes[i], nodes[i + 1]);
        }
    }

    private static void DrawNodes(DrawingContext context, IReadOnlyList<Point> nodes, double timeSeconds)
    {
        for (var i = 0; i < nodes.Count; i++)
        {
            var pulse = 0.5 + 0.5 * Math.Sin(timeSeconds * 2.1 + i * 0.9);

            context.DrawEllipse(
                new SolidColorBrush(Color.FromArgb((byte)(55 + 35 * pulse), 36, 215, 255)),
                null,
                nodes[i],
                20 + pulse * 4,
                20 + pulse * 4);

            context.DrawEllipse(
                new SolidColorBrush(Color.FromArgb(230, 110, 255, 161)),
                null,
                nodes[i],
                5 + pulse * 2,
                5 + pulse * 2);
        }
    }

    private static void DrawPacketStreams(DrawingContext context, IReadOnlyList<Point> nodes, double timeSeconds)
    {
        for (var stream = 0; stream < 22; stream++)
        {
            var speed = stream % 3 == 0 ? 0.52 : 0.38;
            var t = (timeSeconds * speed + stream / 22.0) % 1.0;

            var segFloat = t * (nodes.Count - 1);
            var seg = Math.Min(nodes.Count - 2, (int)Math.Floor(segFloat));
            var local = segFloat - seg;

            var a = nodes[seg];
            var b = nodes[seg + 1];

            var x = a.X + (b.X - a.X) * local;
            var y = a.Y + (b.Y - a.Y) * local;

            var alpha = (byte)(95 + 120 * Math.Sin((t + .2) * Math.PI));
            var radius = stream % 5 == 0 ? 4.4 : 3.2;

            context.DrawEllipse(
                new SolidColorBrush(Color.FromArgb(alpha, 142, 248, 255)),
                null,
                new Point(x, y),
                radius,
                radius);
        }
    }
}