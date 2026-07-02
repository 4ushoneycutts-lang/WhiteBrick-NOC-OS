using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace WhiteBrick.NOC.Views
{
    public partial class CenterMissionPanel : UserControl
    {
        public CenterMissionPanel()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
