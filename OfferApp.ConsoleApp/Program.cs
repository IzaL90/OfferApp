using Microsoft.Extensions.DependencyInjection;
using OfferApp.ConsoleApp;
using OfferApp.Core;

var serviceCollection = new ServiceCollection();
serviceCollection.AddCore()
        .AddSingleton<BidInteractionService>()
        .AddScoped<IConsoleView, AddBidView>()
        .AddScoped<IConsoleView, BidUpView>()
        .AddScoped<IConsoleView, DeleteBidView>()
        .AddScoped<IConsoleView, GetAllBidsView>()
        .AddScoped<IConsoleView, GetBidView>()
        .AddScoped<IConsoleView, GetPublishedBidsView>()
        .AddScoped<IConsoleView, SetPublishBidView>()
        .AddScoped<IConsoleView, UpdateBidView>();

var serviceProvider = serviceCollection.BuildServiceProvider();

var bidInteractionService = serviceProvider.GetRequiredService<BidInteractionService>();
bidInteractionService.RunApp();
