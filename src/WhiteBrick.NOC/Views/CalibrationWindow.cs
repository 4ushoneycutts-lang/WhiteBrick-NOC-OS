using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using WhiteBrick.NOC.Services;
using WhiteBrick.NOC.Widgets;

namespace WhiteBrick.NOC.Views
{
    public class CalibrationWindow : Window
    {
        public CalibrationWindow()
        {
            WindowStartupLocation = WindowStartupLocation.Manual;
            SystemDecorations = SystemDecorations.Full;
            TransparencyLevelHint = new[] { WindowTransparencyLevel.Transparent };
            CanResize = true;

            var monitors = DisplayLayoutService.GetMonitors();
            // compute virtual desktop bounds
            int minX = int.MaxValue, minY = int.MaxValue, maxX = int.MinValue, maxY = int.MinValue;
            foreach (var m in monitors)
            {
                minX = Math.Min(minX, (int)m.Bounds.X);
                minY = Math.Min(minY, (int)m.Bounds.Y);
                maxX = Math.Max(maxX, (int)(m.Bounds.X + m.Bounds.Width));
                maxY = Math.Max(maxY, (int)(m.Bounds.Y + m.Bounds.Height));
            }

            if (minX == int.MaxValue)
            {
                // Fallback: use a reasonable default virtual desktop
                minX = 0; minY = 0; maxX = 1920; maxY = 1080;
            }

            var virtualWidth = Math.Max(1, maxX - minX);
            var virtualHeight = Math.Max(1, maxY - minY);

            // Position window to cover the virtual desktop
            Position = new PixelPoint(minX, minY);
            Width = virtualWidth;
            Height = virtualHeight;

            // Use a single control to render overlays for all monitors
            var calib = new DisplayCalibrationControl();
            Content = calib;
        }
    }
}
