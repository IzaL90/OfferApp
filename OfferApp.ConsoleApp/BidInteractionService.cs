using OfferApp.Core.DTO;
using OfferApp.Core.Services;

namespace OfferApp.ConsoleApp
{
    internal class BidInteractionService
    {
        private readonly IMenuService _menuService;
        private readonly IBidService _bidService;
        private IEnumerable<IConsoleView> views = new List<IConsoleView>();

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
                InitializeViews();

                try
                {
                    switch (consoleKey.Key)
                    {
                        case ConsoleKey.D1:
                        case ConsoleKey.D2:
                        case ConsoleKey.D3:
                        case ConsoleKey.D4:
                        case ConsoleKey.D5:
                        case ConsoleKey.D6:
                        case ConsoleKey.D7:
                        case ConsoleKey.D8:
                            InvokeView(consoleKey);
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
                finally
                {
                    DisposeViews();
                }
            }
        }

        private static void ShowMenus(IEnumerable<MenuDto> menus)
        {
            Console.WriteLine("Please choose menu:");
            foreach (var menu in menus)
            {
                Console.WriteLine(menu);
            }
        }

        private void InvokeView(ConsoleKeyInfo key)
        {
            var view = GetView(key.KeyChar.ToString());
            view.GenerateView();
        }

        private IConsoleView GetView(string keyCharacter)
        {
            var view = views.SingleOrDefault(v=> v.KeyProvider == keyCharacter) 
                ?? throw new InvalidOperationException($"There is no view for key {keyCharacter}");
            return view;
        }

        private void InitializeViews()
        {
            views = new List<IConsoleView>
            {
                new AddBidView(_bidService),
                new BidUpView(_bidService),
                new DeleteBidView(_bidService),
                new GetAllBidsView(_bidService),
                new GetBidView(_bidService),
                new GetPublishedBidsView(_bidService),
                new SetPublishBidView(_bidService),
                new UpdateBidView(_bidService)
            };
        }

        private void DisposeViews()
        {
            views = new List<IConsoleView>();
        }
    }
}
