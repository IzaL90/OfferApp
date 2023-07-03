using Microsoft.Extensions.DependencyInjection;
using OfferApp.ConsoleApp;
using OfferApp.Core;
using OfferApp.Infrastructure;
using System.Reflection;
using System.Text.Json;

IServiceCollection Setup(Dictionary<string, object> appsettings)
{
    var serviceCollection = new ServiceCollection();
    serviceCollection.AddCore()
            .AddInfrastructure(appsettings)
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

async Task<Dictionary<string, object>> GetAppsettings()
{
    var appFolder = AppContext.BaseDirectory
        ?? throw new InvalidOperationException("Cannot get Application folder directory");
    using FileStream fileStream = File.Open(appFolder + Path.DirectorySeparatorChar + "appsettings.json", FileMode.Open, FileAccess.Read);
    var settings = await JsonSerializer.DeserializeAsync<Dictionary<string, object>>(fileStream)
            ?? throw new InvalidOperationException("Cannot deserialize 'appsettings.json', please ensure that this file exists");
    return settings;
}

var appsettings = await GetAppsettings();
var serviceCollection = Setup(appsettings);
var serviceProvider = serviceCollection.BuildServiceProvider();
serviceProvider.UseInfrastructure();

var bidInteractionService = serviceProvider.GetRequiredService<BidInteractionService>();
await bidInteractionService.RunApp();
