using OfferApp.Core.DTO;
using OfferApp.Core.Services;

namespace OfferApp.ConsoleApp
{
    internal class BidInteractionService
    {
        private readonly IMenuService _menuService;
        private readonly IBidService _bidService;

        public BidInteractionService(IMenuService menuService, IBidService bidService)
        {
            _menuService = menuService;
            _bidService = bidService;
        }

        public void RunApp()
        {
            if (_menuService is null || _bidService is null)
            {
                throw new InvalidOperationException("Cannot run app with null IMenuService or IBidService");
            }

            var menus = _menuService.GetMenus();

            bool isProgramRunning = true;
            while (isProgramRunning)
            {
                ShowMenus(menus);
                var consoleKey = Console.ReadKey();
                Console.WriteLine();

                try
                {
                    switch (consoleKey.Key)
                    {
                        case ConsoleKey.D1:
                            var bid = CreateBid();

                            if (bid is null)
                            {
                                break;
                            }

                            bid = _bidService.AddBid(bid);
                            Console.WriteLine($"Added Bid {bid}");
                            break;
                        case ConsoleKey.D2:
                            bid = GetBid();
                            if (bid is null)
                            {
                                break;
                            }
                            Console.WriteLine($"Bid: {bid}");
                            break;
                        case ConsoleKey.D3:
                            bid = GetBid();
                            if (bid is null)
                            {
                                break;
                            }
                            ModifiedBid(bid);
                            bid = _bidService.UpdateBid(bid);
                            Console.WriteLine($"Updated Bid {bid}");
                            break;
                        case ConsoleKey.D4:
                            var bids = _bidService.GetAllBids();
                            foreach (var bidInList in bids)
                            {
                                Console.WriteLine($"Bid: {bidInList}");
                            }
                            break;
                        case ConsoleKey.D5:
                            var id = GetBidId();
                            _bidService.DeleteBid(id);
                            Console.WriteLine($"Bid with id: {id} was deleted");
                            break;
                        case ConsoleKey.D6:
                            var bidsPublished = _bidService.GetAllPublishedBids();
                            foreach (var bidInList in bidsPublished)
                            {
                                Console.WriteLine($"Bid: {bidInList}");
                            }
                            break;
                        case ConsoleKey.D7:
                            bid = GetBid();
                            if (bid is null)
                            {
                                break;
                            }
                            var publish = PublishBid(bid);
                            if (publish is null)
                            {
                                break;
                            }
                            if (publish.Value)
                            {
                                Console.WriteLine(_bidService.Published(bid.Id) ? $"Published Bid with id '{bid.Id}'" : $"Cannot published Bid with id '{bid.Id}'");
                            }
                            else
                            {
                                Console.WriteLine(_bidService.Unpublished(bid.Id) ? $"Unpublished Bid with id '{bid.Id}'" : $"Cannot unpublished Bid with id '{bid.Id}'");
                            }
                            break;
                        case ConsoleKey.D8:
                            bid = GetBid();
                            if (bid is null)
                            {
                                break;
                            }
                            var bidUpValue = BidUp();
                            if (bidUpValue is null)
                            {
                                break;
                            }
                            _bidService.BidUp(bid.Id, bidUpValue.Value);
                            break;
                        case ConsoleKey.D9:
                            isProgramRunning = false;
                            break;
                        default:
                            Console.WriteLine("Entered invalid Key");
                            break;
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
        }

        private BidDto? GetBid()
        {
            var id = GetBidId();
            var quest = _bidService.GetBidById(id);

            if (quest is null)
            {
                Console.WriteLine($"Quest with id {id} was not found");
            }
            return quest;
        }

        private static void ShowMenus(IEnumerable<MenuDto> menus)
        {
            Console.WriteLine("Please choose menu:");
            foreach (var menu in menus)
            {
                Console.WriteLine(menu);
            }
        }

        private BidDto? CreateBid()
        {
            var bid = new BidDto();
            if (!SetTitle(bid))
            {
                return null;
            }
            if (!SetDescription(bid))
            {
                return null;
            }
            if (!SetFirstPrice(bid))
            {
                return null;
            }
            return bid;
        }

        private BidDto? ModifiedBid(BidDto bid)
        {
            if (!SetTitle(bid))
            {
                return null;
            }
            if (!SetDescription(bid))
            {
                return null;
            }
            if (!SetFirstPrice(bid))
            {
                return null;
            }
            return bid;
        }

        private int GetBidId()
        {
            Console.WriteLine("Please enter Bid id");
            int.TryParse(Console.ReadLine(), out var id);
            return id;
        }

        private bool? PublishBid(BidDto bid)
        {
            Console.WriteLine($"Please choose 0 to unpublish or 1 to publish Bid with id '{bid.Id}' and name '{bid.Name}'");
            if (!int.TryParse(Console.ReadLine(), out var state))
            {
                Console.WriteLine("Entered invalid value");
                return null;
            }

            if (state < 0 || state > 1)
            {
                return null;
            }

            return state == 1;
        }

        private decimal? BidUp()
        {
            Console.WriteLine("Enter value to bid up");

            if (!decimal.TryParse(Console.ReadLine(), out var value))
            {
                Console.WriteLine("Entered invalid value");
                return null;
            }

            return value;
        }

        private bool SetTitle(BidDto bid)
        {
            Console.WriteLine("Enter Bid name");
            var name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Bid cannot be empty");
                return false;
            }
            bid.Name = name;
            return true;
        }

        private bool SetDescription(BidDto bid)
        {
            Console.WriteLine("Enter Bid description");
            var description = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(description))
            {
                Console.WriteLine("Bid cannot be empty");
                return false;
            }
            bid.Description = description;
            return true;
        }

        private bool SetFirstPrice(BidDto bid)
        {
            Console.WriteLine("Enter Bid first price");
            var description = Console.ReadLine();
            if (!decimal.TryParse(description, out var price))
            {
                Console.WriteLine("Invalid Bid price");
                return false;
            }
            bid.FirstPrice = price;
            return true;
        }
    }
}
