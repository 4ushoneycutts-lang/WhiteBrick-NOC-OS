using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using WhiteBrick.NOC.Services;
using System.Globalization;
using System.Linq;

namespace WhiteBrick.NOC.Widgets
{
    public class DisplayCalibrationControl : Control
    {
        private Point _lastMouse = new Point(0,0);

        public DisplayCalibrationControl()
        {
            PointerMoved += OnPointerMoved;
        }

        private void OnPointerMoved(object? sender, PointerEventArgs e)
        {
            var p = e.GetPosition(this);
            _lastMouse = p;
            InvalidateVisual();
        }

        public override void Render(DrawingContext context)
        {
            var monitors = DisplayLayoutService.GetMonitors();
            if (monitors == null || !monitors.Any())
                return;
            // Black background covering the entire window
            context.FillRectangle(Brushes.Black, new Rect(0, 0, Bounds.Width, Bounds.Height));

            // Determine virtual origin so monitor rectangles map to control space
            var originX = monitors.Min(m => m.Bounds.X);
            var originY = monitors.Min(m => m.Bounds.Y);

            int spacing = 50; // grid spacing

            // Draw global 50px grid across the virtual desktop
            var gridPen = new Pen(Brushes.DimGray, 1) { DashStyle = new DashStyle(new double[] { 2, 2 }, 0) };
            for (double x = 0; x <= Bounds.Width; x += spacing)
            {
                context.DrawLine(gridPen, new Point(x, 0), new Point(x, Bounds.Height));
            }
            for (double y = 0; y <= Bounds.Height; y += spacing)
            {
                context.DrawLine(gridPen, new Point(0, y), new Point(Bounds.Width, y));
            }

            // Render each monitor
            foreach (var m in monitors)
            {
                var rect = new Rect(m.Bounds.X - originX, m.Bounds.Y - originY, m.Bounds.Width, m.Bounds.Height);

                // Monitor boundary
                context.DrawRectangle(null, new Pen(Brushes.Cyan, 2), rect);

                // Center crosshair for this monitor
                var cx = rect.X + rect.Width / 2;
                var cy = rect.Y + rect.Height / 2;
                var centerPen = new Pen(Brushes.Orange, 2);
                context.DrawLine(centerPen, new Point(cx, rect.Y), new Point(cx, rect.Bottom));
                context.DrawLine(centerPen, new Point(rect.X, cy), new Point(rect.Right, cy));

                // Safe-area border (80px)
                double safe = 80;
                var safeRect = new Rect(rect.X + safe, rect.Y + safe, Math.Max(0, rect.Width - safe * 2), Math.Max(0, rect.Height - safe * 2));
                context.DrawRectangle(null, new Pen(Brushes.Yellow, 1), safeRect);

                // Monitor info block: number, resolution, working area, DPI
                var info = $"MONITOR {m.Index}\nResolution: {(int)m.Bounds.Width}x{(int)m.Bounds.Height}\nWork: {(int)m.WorkArea.Width}x{(int)m.WorkArea.Height} @ X:{(int)m.WorkArea.X},Y:{(int)m.WorkArea.Y}\nDPI: {m.ScaleFactor:0.00}";
                var ft = new FormattedText(info, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Typeface.Default, 14, Brushes.White);
                context.DrawText(ft, new Point(rect.X + 12, rect.Y + 12));
            }

            // Draw origin marker at top-left of virtual desktop
            context.DrawEllipse(Brushes.Red, null, new Point(4, 4), 4, 4);

            // Live mouse coordinates (convert to virtual desktop coords)
            var virtualMouseX = (int)(_lastMouse.X + originX);
            var virtualMouseY = (int)(_lastMouse.Y + originY);
            var mouseInfoText = $"Mouse: X:{virtualMouseX} Y:{virtualMouseY}";
            var mft = new FormattedText(mouseInfoText, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Typeface.Default, 14, Brushes.Lime);
            context.DrawText(mft, new Point(12, Bounds.Height - 28));
        }
    }
}
