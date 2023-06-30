using Microsoft.Extensions.DependencyInjection;
using OfferApp.ConsoleApp;
using OfferApp.Core;
using System.Reflection;

IServiceCollection Setup()
{
    var serviceCollection = new ServiceCollection();
    serviceCollection.AddCore()
            .AddSingleton<BidInteractionService>();

    Assembly.GetExecutingAssembly().GetTypes()
        .AsParallel()
        .Where(t => typeof(IConsoleView).IsAssignableFrom(t) && t != typeof(IConsoleView))
        .ToList()
        .ForEach(t =>
        {
            serviceCollection.AddScoped(typeof(IConsoleView), t);
        });
    return serviceCollection;
}


var serviceCollection = Setup();
var serviceProvider = serviceCollection.BuildServiceProvider();

var bidInteractionService = serviceProvider.GetRequiredService<BidInteractionService>();
bidInteractionService.RunApp();
