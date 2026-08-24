namespace FusionHUD.Interfaces
{
    public interface IStartupService
    {
        void EnableStartup();

        void DisableStartup();

        bool IsStartupEnabled();
    }

}