using FusionHUD.Overlay.Interfaces;
using FusionHUD.Overlay.ViewModels;
using System.Windows;

namespace FusionHUD.Overlay
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel ViewModel, IOverlayWindowService WindowService)
        {
            InitializeComponent();

            DataContext = ViewModel;

            WindowService.Initialize(this);
        }
    }

}