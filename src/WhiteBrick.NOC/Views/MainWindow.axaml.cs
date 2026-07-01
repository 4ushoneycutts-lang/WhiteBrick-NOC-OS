using Avalonia.Controls;
using Avalonia.Input;
using WhiteBrick.NOC.ViewModels;

namespace WhiteBrick.NOC.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

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
