using OfferApp.Core.Services;

namespace OfferApp.ConsoleApp
{
    internal sealed class GetPublishedBidsView : IConsoleView
    {
        private readonly IBidService _bidService;

        public string KeyProvider => "6";

        public GetPublishedBidsView(IBidService bidService)
        {
            _bidService = bidService;
        }

        public async Task GenerateView()
        {
            var bidsPublished = await _bidService.GetAllPublishedBids();
            foreach (var bidInList in bidsPublished)
            {
                Console.WriteLine($"Bid: {bidInList}");
            }
        }
    }
}
