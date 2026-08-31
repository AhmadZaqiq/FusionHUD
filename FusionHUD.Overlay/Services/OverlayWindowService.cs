using FusionHUD.Overlay.Interfaces;
using FusionHUD.Overlay.Models;
using System.Windows;
using System.Windows.Interop;

namespace FusionHUD.Overlay.Services
{
    public sealed class OverlayWindowService : IOverlayWindowService
    {
        private readonly IOverlaySettingsService _SettingsService;

        private readonly IOverlayPresentationService _PresentationService;

        private readonly IHotkeyService _HotkeyService;

        private readonly IOverlayClickThroughService _ClickThroughService;

        private Window? _Window;

        private HwndSource? _HwndSource;

        public OverlayWindowService(IOverlaySettingsService SettingsService, IOverlayPresentationService PresentationService,
                                    IHotkeyService HotkeyService, IOverlayClickThroughService ClickThroughService)
        {
            _SettingsService = SettingsService;

            _PresentationService = PresentationService;

            _HotkeyService = HotkeyService;

            _ClickThroughService = ClickThroughService;
        }

        public void Initialize(Window Window)
        {
            if (_Window is not null)
            {
                return;
            }

            _Window = Window;

            _Window.SourceInitialized += Window_SourceInitialized;
            _Window.Closed += Window_Closed;

            ApplySettings();
        }

        public void Show()
        {
            _Window?.Show();
        }

        public void Hide()
        {
            _Window?.Hide();
        }

        public void ToggleVisibility()
        {
            if (_Window is null)
            {
                return;
            }

            if (_Window.Visibility == Visibility.Visible)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        public void ApplySettings()
        {
            if (_Window is null)
            {
                return;
            }

            _PresentationService.ApplySettings(_Window, _SettingsService.Settings);
        }

        public void Dispose()
        {
            if (_Window is null)
            {
                return;
            }

            if (_HwndSource is not null)
            {
                _HwndSource.RemoveHook(HwndHook); // Remove the Win32 message hook before releasing the HwndSource reference.

                _HwndSource = null;
            }

            WindowInteropHelper Helper = new(_Window);

            if (Helper.Handle != IntPtr.Zero)
            {
                _HotkeyService.Unregister(Helper.Handle);
            }

            _Window.SourceInitialized -= Window_SourceInitialized;
            _Window.Closed -= Window_Closed;

            _Window = null;
        }

        private void Window_SourceInitialized(object? Sender, EventArgs e)
        {
            if (_Window is null)
            {
                return;
            }

            WindowInteropHelper Helper = new(_Window); // The HWND is available after source initialization, allowing Win32 interop to be configured.

            _HwndSource = HwndSource.FromHwnd(Helper.Handle);

            _HwndSource?.AddHook(HwndHook);

            _HotkeyService.Register(Helper.Handle);

            _ClickThroughService.Apply(Helper.Handle);
        }

        private void Window_Closed(object? Sender, EventArgs e)
        {
            Dispose();
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
                    _SettingsService.MoveToNextPosition();
                    ApplySettings();
                    break;

                case OverlayHotkeyAction.ChangeSize:
                    _SettingsService.MoveToNextSize();
                    ApplySettings();
                    break;

                case OverlayHotkeyAction.ChangeColor:
                    _SettingsService.MoveToNextColor();
                    ApplySettings();
                    break;
            }
        }
    }
}
