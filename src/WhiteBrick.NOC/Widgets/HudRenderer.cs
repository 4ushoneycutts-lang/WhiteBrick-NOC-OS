using Avalonia;
using Avalonia.Media;

namespace WhiteBrick.NOC.Widgets;

public sealed class HudRenderer
{
    public void Render(DrawingContext context, double cx, double cy, double radius, double time)
    {
        DrawStatusText(context, cx, cy, radius);
        DrawSystemStatusPanel(context, cx, cy, radius, time);
    }

    private static void DrawStatusText(DrawingContext context, double cx, double cy, double r)
    {
        var statusText = new FormattedText(
            "HOME BEACON / ROUTE STABLE",
            System.Globalization.CultureInfo.InvariantCulture,
            FlowDirection.LeftToRight,
            Typeface.Default,
            13,
            new SolidColorBrush(Color.FromRgb(217, 247, 255)));

        context.DrawText(
            statusText,
            new Point(cx - statusText.Width / 2, cy + r + 24));

        var subText = new FormattedText(
            "WB-CORE • HONEYCUTT • WAN LINK",
            System.Globalization.CultureInfo.InvariantCulture,
            FlowDirection.LeftToRight,
            Typeface.Default,
            10,
            new SolidColorBrush(Color.FromArgb(180, 119, 200, 232)));

        context.DrawText(
            subText,
            new Point(cx - subText.Width / 2, cy + r + 44));
    }

    private static void DrawSystemStatusPanel(DrawingContext context, double cx, double cy, double r, double time)
    {
        var panelW = r * 1.34;
        var panelH = r * 0.78;
        var x = cx - r * 2.02;
        var y = cy - r * 0.82;

        var rect = new Rect(x, y, panelW, panelH);

        context.FillRectangle(
            new SolidColorBrush(Color.FromArgb(42, 6, 12, 18)),
            rect);

        context.DrawRectangle(
            new Pen(new SolidColorBrush(Color.FromArgb(70, 80, 130, 170)), 1),
            rect);

        var health = 100;
        var traffic = 2.6 + System.Math.Abs(System.Math.Sin(time * 0.42 + 0.9)) * 0.6;
        var latency = 1.0 + System.Math.Abs(System.Math.Sin(time * 1.1 + 0.4)) * 1.2;

        var baseUptimeSeconds = (17 * 24 + 4) * 3600;
        var uptimeSeconds = baseUptimeSeconds + (int)time;
        var uptimeDays = uptimeSeconds / 86400;
        var uptimeHours = (uptimeSeconds % 86400) / 3600;

        var dimBrush = new SolidColorBrush(Color.FromArgb(160, 110, 160, 190));
        var valueBrush = new SolidColorBrush(Color.FromRgb(180, 220, 235));
        var onlineBrush = new SolidColorBrush(Color.FromArgb(220, 120, 255, 170));

        DrawText(context, "SYSTEM STATUS", x + 14, y + 12, 11, dimBrush);
        DrawText(context, "ONLINE", x + 110, y + 12, 13, onlineBrush);

        var lineY = y + 40;

        DrawLine(context, "Nodes ........", "08 / 08", x + 14, lineY, panelW, dimBrush, valueBrush);
        DrawLine(context, "Health .......", health + "%", x + 14, lineY + 18, panelW, dimBrush, valueBrush);
        DrawLine(context, "Traffic ......", traffic.ToString("0.0") + " Gbps", x + 14, lineY + 36, panelW, dimBrush, valueBrush);
        DrawLine(context, "Latency ......", latency.ToString("0.0") + " ms", x + 14, lineY + 54, panelW, dimBrush, valueBrush);
        DrawLine(context, "Alerts .......", "0", x + 14, lineY + 72, panelW, dimBrush, valueBrush);
        DrawLine(context, "Uptime .......", $"{uptimeDays}d {uptimeHours:D2}h", x + 14, lineY + 90, panelW, dimBrush, valueBrush);
    }

    private static void DrawLine(
        DrawingContext context,
        string label,
        string value,
        double x,
        double y,
        double width,
        IBrush labelBrush,
        IBrush valueBrush)
    {
        DrawText(context, label, x, y, 10, labelBrush);
        DrawText(context, value, x + width - 95, y, 11, valueBrush);
    }

    private static void DrawText(
        DrawingContext context,
        string text,
        double x,
        double y,
        double size,
        IBrush brush)
    {
        var formatted = new FormattedText(
            text,
            System.Globalization.CultureInfo.InvariantCulture,
            FlowDirection.LeftToRight,
            Typeface.Default,
            size,
            brush);

        context.DrawText(formatted, new Point(x, y));
    }
}