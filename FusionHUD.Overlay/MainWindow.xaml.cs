using FusionHUD.Overlay.Interfaces;
using FusionHUD.Overlay.Models;
using FusionHUD.Overlay.ViewModels;
using System.Windows;
using System.Windows.Interop;

namespace FusionHUD.Overlay
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _ViewModel;

        private readonly IOverlaySettingsService _SettingsService;

        private readonly IOverlayPresentationService _PresentationService;

        private readonly IHotkeyService _HotkeyService;


        private readonly IOverlayClickThroughService _ClickThroughService;

        private HwndSource? _HwndSource;

        public MainWindow(MainViewModel ViewModel, IOverlaySettingsService SettingsService,
                          IOverlayPresentationService PresentationService, IHotkeyService HotkeyService,
                          IOverlayClickThroughService ClickThroughService)
        {
            InitializeComponent();

            _ViewModel = ViewModel;

            _SettingsService = SettingsService;

            _HotkeyService = HotkeyService;

            _PresentationService = PresentationService;

            _ClickThroughService = ClickThroughService;

            DataContext = _ViewModel;

            ApplySettings();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            WindowInteropHelper Helper = new WindowInteropHelper(this);

            _HwndSource = HwndSource.FromHwnd(Helper.Handle);

            _HwndSource?.AddHook(HwndHook);

            _HotkeyService.Register(Helper.Handle);

            _ClickThroughService.Apply(Helper.Handle);
        }

        protected override void OnClosed(EventArgs e)
        {
            if (_HwndSource is not null)
            {
                _HwndSource.RemoveHook(HwndHook);
            }

            WindowInteropHelper Helper = new WindowInteropHelper(this);

            _HotkeyService.Unregister(Helper.Handle);

            base.OnClosed(e);
        }

        private IntPtr HwndHook(IntPtr HWnd, int Message, IntPtr WParam, IntPtr LParam, ref bool Handled)
        {
            OverlayHotkeyAction? Action = _HotkeyService.GetHotkeyAction(Message, WParam);

            if (Action is null)
            {
                return IntPtr.Zero;
            }

            HandleHotkeyAction(Action.Value);

            Handled = true;

            return IntPtr.Zero;
        }

        private void HandleHotkeyAction(OverlayHotkeyAction Action)
        {
            switch (Action)
            {
                case OverlayHotkeyAction.ToggleVisibility:
                    ToggleVisibility();
                    break;

                case OverlayHotkeyAction.ChangePosition:
                    ChangePosition();
                    break;

                case OverlayHotkeyAction.ChangeSize:
                    ChangeSize();
                    break;

                case OverlayHotkeyAction.ChangeColor:
                    ChangeColor();
                    break;
            }
        }

        private void ToggleVisibility()
        {
            Visibility = Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        }

        private void ChangePosition()
        {
            _SettingsService.MoveToNextPosition();

            _PresentationService.ApplyPosition(this, _SettingsService.Settings.Position);

            _PresentationService.ApplyAlignment(this, _SettingsService.Settings.Position);
        }

        private void ChangeSize()
        {
            _SettingsService.MoveToNextSize();

            _PresentationService.ApplySize(this, _SettingsService.Settings.Size);

            _PresentationService.ApplyPosition(this, _SettingsService.Settings.Position);
        }

        private void ChangeColor()
        {
            _SettingsService.MoveToNextColor();

            _PresentationService.ApplyColor(this, _SettingsService.Settings.Color);
        }

        private void ApplySettings()
        {
            _PresentationService.ApplySettings(this, _SettingsService.Settings);
        }
    }

}