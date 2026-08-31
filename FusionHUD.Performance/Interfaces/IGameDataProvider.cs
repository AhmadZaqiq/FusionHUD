namespace FusionHUD.Performance.Interfaces
{
    public interface IGameDataProvider
    {
        bool IsGameActive { get; }

        string GameName { get; }

        float GetFPS();
    }

}