namespace OfferApp.ConsoleApp
{
    public interface IConsoleView
    {
        string KeyProvider { get; }

        Task GenerateView();
    }
}
