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

            // Draw translucent overlay per monitor and grid
            foreach (var m in monitors)
            {
                var rect = new Rect(m.Bounds.X - monitors.Min(mm=>mm.Bounds.X), m.Bounds.Y - monitors.Min(mm=>mm.Bounds.Y), m.Bounds.Width, m.Bounds.Height);
                var fill = new SolidColorBrush(Color.FromArgb(64, 8, 20, 40));
                context.FillRectangle(fill, rect);
                context.DrawRectangle(new Pen(Brushes.Cyan,2), rect);

                // Draw monitor info text
                var text = $"MONITOR {m.Index}\n{(int)m.Bounds.Width}x{(int)m.Bounds.Height}\nX:{(int)m.Bounds.X}\nY:{(int)m.Bounds.Y}\nW:{(int)m.Bounds.Width}\nH:{(int)m.Bounds.Height}\nDPI:{m.ScaleFactor:0.00}";
                var ft = new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Typeface.Default, 14, Brushes.White);
                context.DrawText(ft, new Point(rect.X + 12, rect.Y + 12));

                // Draw grid lines within monitor
                int spacing = 50;
                for (double x = rect.X; x <= rect.Right; x += spacing)
                {
                    context.DrawLine(new Pen(Brushes.Gray,1){DashStyle=new DashStyle(new double[]{2,2},0)}, new Point(x, rect.Y), new Point(x, rect.Bottom));
                }
                for (double y = rect.Y; y <= rect.Bottom; y += spacing)
                {
                    context.DrawLine(new Pen(Brushes.Gray,1){DashStyle=new DashStyle(new double[]{2,2},0)}, new Point(rect.X, y), new Point(rect.Right, y));
                }

                // Safe margin (e.g., 80 px)
                double safe = 80;
                var safeRect = new Rect(rect.X + safe, rect.Y + safe, Math.Max(0, rect.Width - safe*2), Math.Max(0, rect.Height - safe*2));
                context.DrawRectangle(null, new Pen(Brushes.Yellow,1), safeRect);
            }

            // Draw virtual desktop axes labels along top and left
            var originX = monitors.Min(m=>m.Bounds.X);
            var originY = monitors.Min(m=>m.Bounds.Y);
            // mouse coordinates
            var mouseInfo = $"Mouse: X:{(int)(_lastMouse.X+originX)} Y:{(int)(_lastMouse.Y+originY)}";
            var mouseText = new FormattedText(mouseInfo, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Typeface.Default, 14, Brushes.Lime);
            // Draw at bottom-right with padding
            context.DrawText(mouseText, new Point(Math.Max(12, Bounds.Width - 320), Math.Max(12, Bounds.Height - 28)));
        }
    }
}
