using OfferApp.ConsoleApp;
using OfferApp.Core;

var menuService = Extensions.CreateMenuService();
var bidService = Extensions.CreateBidService();
var bidInteractionService = new BidInteractionService(menuService, bidService);
bidInteractionService.RunApp();
