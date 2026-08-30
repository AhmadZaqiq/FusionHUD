using FusionHUD.Overlay.Interfaces;
using FusionHUD.Overlay.Models;
using System.Runtime.InteropServices;

namespace FusionHUD.Overlay.Services
{
    public class HotkeyService : IHotkeyService
    {
        private const int WM_HOTKEY = 0x0312;

        private const int TOGGLE_VISIBILITY_ID = 9000;
        private const int CHANGE_POSITION_ID = 9001;
        private const int CHANGE_SIZE_ID = 9002;
        private const int CHANGE_COLOR_ID = 9003;

        private const uint MOD_CONTROL = 0x0002;
        private const uint MOD_SHIFT = 0x0004;

        public void Register(IntPtr Handle)
        {
            RegisterHotKey(Handle, TOGGLE_VISIBILITY_ID, MOD_CONTROL | MOD_SHIFT, (int)'O');

            RegisterHotKey(Handle, CHANGE_POSITION_ID, MOD_CONTROL | MOD_SHIFT, (int)'P');

            RegisterHotKey(Handle, CHANGE_SIZE_ID, MOD_CONTROL | MOD_SHIFT, (int)'S');

            RegisterHotKey(Handle, CHANGE_COLOR_ID, MOD_CONTROL | MOD_SHIFT, (int)'C');
        }

        public void Unregister(IntPtr Handle)
        {
            UnregisterHotKey(Handle, TOGGLE_VISIBILITY_ID);

            UnregisterHotKey(Handle, CHANGE_POSITION_ID);

            UnregisterHotKey(Handle, CHANGE_SIZE_ID);

            UnregisterHotKey(Handle, CHANGE_COLOR_ID);
        }

        public OverlayHotkeyAction? GetHotkeyAction(int Message, IntPtr WParam)
        {
            if (Message != WM_HOTKEY)
            {
                return null;
            }

            int HotkeyID = WParam.ToInt32();

            return HotkeyID switch
            {
                TOGGLE_VISIBILITY_ID => OverlayHotkeyAction.ToggleVisibility,

                CHANGE_POSITION_ID => OverlayHotkeyAction.ChangePosition,

                CHANGE_SIZE_ID => OverlayHotkeyAction.ChangeSize,

                CHANGE_COLOR_ID => OverlayHotkeyAction.ChangeColor,

                _ => null
            };
        }

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr HWnd, int ID, uint Modifier, int Key);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr HWnd, int ID);
    }

}