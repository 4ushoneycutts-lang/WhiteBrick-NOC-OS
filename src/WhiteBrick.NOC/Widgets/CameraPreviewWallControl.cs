using Avalonia;
using Avalonia.Media;

namespace WhiteBrick.NOC.Widgets;

public sealed class CameraPreviewWallControl : AnimatedWidgetBase
{
    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var w = Bounds.Width;
        var h = Bounds.Height;
        context.FillRectangle(new SolidColorBrush(Color.FromArgb(92, 5, 8, 17)), Bounds);
        context.DrawRectangle(new Pen(new SolidColorBrush(Color.FromArgb(85, 36, 215, 255)), 1), new Rect(0.5, 0.5, Math.Max(0, w - 1), Math.Max(0, h - 1)));

        DrawLabel(context, "CAMERA WALL READY", 10, 8, 12, Color.FromRgb(189, 238, 255));

        var pad = 10.0;
        var top = 30.0;
        var cellW = (w - pad * 3) / 2;
        var cellH = (h - top - pad * 3) / 2;
        var labels = new[] { "RING FRONT", "RING DRIVE", "OFFICE", "FUTURE" };

        for (int i = 0; i < 4; i++)
        {
            var col = i % 2;
            var row = i / 2;
            var x = pad + col * (cellW + pad);
            var y = top + row * (cellH + pad);
            DrawCameraCell(context, new Rect(x, y, cellW, cellH), labels[i], i);
        }
    }

    private void DrawCameraCell(DrawingContext context, Rect rect, string label, int index)
    {
        context.FillRectangle(new SolidColorBrush(Color.FromArgb(140, 8, 15, 30)), rect);
        context.DrawRectangle(new Pen(new SolidColorBrush(Color.FromArgb(100, 31, 106, 141)), 1), rect);

        for (int scan = 0; scan < 4; scan++)
        {
            var y = rect.Y + ((TimeSeconds * 18 + scan * 22 + index * 7) % Math.Max(1, rect.Height));
            context.DrawLine(new Pen(new SolidColorBrush(Color.FromArgb(35, 142, 248, 255)), 1), new Point(rect.X, y), new Point(rect.Right, y));
        }

        var pulse = 0.55 + 0.45 * Math.Sin(TimeSeconds * 1.8 + index);
        context.DrawEllipse(new SolidColorBrush(Color.FromArgb((byte)(120 + 80 * pulse), 110, 255, 161)), null, new Point(rect.X + 11, rect.Y + 12), 3, 3);
        DrawLabel(context, label, rect.X + 20, rect.Y + 5, 10, Color.FromRgb(217, 247, 255));
        DrawLabel(context, "STAGED", rect.X + 8, rect.Bottom - 18, 9, Color.FromRgb(119, 200, 232));
    }

    private static void DrawLabel(DrawingContext context, string text, double x, double y, double size, Color color)
    {
        var formatted = new FormattedText(text, System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Typeface.Default, size, new SolidColorBrush(color));
        context.DrawText(formatted, new Point(x, y));
    }
}
