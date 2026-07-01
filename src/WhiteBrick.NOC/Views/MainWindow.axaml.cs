using Avalonia.Controls;
using Avalonia.Input;
using WhiteBrick.NOC.ViewModels;
using Avalonia.Threading;
using Avalonia.Controls.Shapes;
using Avalonia.Layout;
using Avalonia;
using System.Diagnostics;

namespace WhiteBrick.NOC.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // Start a lightweight heartbeat animation for the header status dot.
        var dot = this.FindControl<Ellipse>("HeartbeatDot");
        if (dot != null)
        {
            var sw = Stopwatch.StartNew();
            var timer = new DispatcherTimer(TimeSpan.FromMilliseconds(60), DispatcherPriority.Render, (s, e) =>
            {
                var t = sw.Elapsed.TotalSeconds;
                // horizontal sweep across header width (wrap)
                var span = 200.0; // travel width in pixels (keeps dot near left area)
                var x = (Math.Abs(Math.Sin(t * 1.6)) * 0.6 + 0.2) * span;
                if (dot.RenderTransform is Avalonia.Media.TranslateTransform tt)
                {
                    tt.X = 8 + x;
                }
                // subtle pulsing scale
                var scale = 0.85 + 0.35 * Math.Abs(Math.Sin(t * 2.8));
                dot.Width = 6 * scale + 2;
                dot.Height = 6 * scale + 2;
                // gentle opacity breath
                dot.Opacity = 0.6 + 0.35 * Math.Abs(Math.Sin(t * 1.9));
            });
            timer.Start();
        }

        KeyDown += (_, e) =>
        {
            if (e.Key == Key.F11)
            {
                WindowState = WindowState == WindowState.FullScreen
                    ? WindowState.Normal
                    : WindowState.FullScreen;
            }

            if (e.Key == Key.F9 && DataContext is MainWindowViewModel modeVm)
            {
                modeVm.CycleOperatingMode();
            }

            if (e.Key == Key.F12 && DataContext is MainWindowViewModel vm)
            {
                vm.ToggleDeveloperOverlay();
            }
        };
    }
}
