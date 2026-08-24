using FusionHUD.Interfaces;
using Microsoft.Win32;
using System.Reflection;

namespace FusionHUD.Services
{
    public class StartupService : IStartupService
    {
        private const string STARTUP_NAME = "FusionHUD Performance Overlay";

        private const string STARTUP_KEY = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        public void EnableStartup()
        {
            string ApplicationPath = Assembly.GetExecutingAssembly().Location;

            using RegistryKey? Key = Registry.CurrentUser.CreateSubKey(STARTUP_KEY);

            Key?.SetValue(STARTUP_NAME, ApplicationPath);
        }

        public void DisableStartup()
        {
            using RegistryKey? Key = Registry.CurrentUser.OpenSubKey(STARTUP_KEY, writable: true);

            Key?.DeleteValue(STARTUP_NAME, throwOnMissingValue: false);
        }

        public bool IsStartupEnabled()
        {
            using RegistryKey? Key = Registry.CurrentUser.OpenSubKey(STARTUP_KEY);

            return Key?.GetValue(STARTUP_NAME) is not null;
        }
    }

}