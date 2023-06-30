using OfferApp.Core.Services;

namespace OfferApp.ConsoleApp
{
    internal sealed class GetAllBidsView : IConsoleView
    {
        private readonly IBidService _bidService;

        public string KeyProvider => "4";

        public GetAllBidsView(IBidService bidService)
        {
            _bidService = bidService;
        }

        public async Task GenerateView()
        {
            var bids = await _bidService.GetAllBids();
            foreach (var bidInList in bids)
            {
                Console.WriteLine($"Bid: {bidInList}");
            }
        }
    }
}
