using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using OfferApp.ConsoleApp;
using OfferApp.Core;
using OfferApp.Infrastructure;
using OfferApp.Infrastructure.Database;
using System.Reflection;
using System.Text.Json;

class Program : IDesignTimeDbContextFactory<OfferDbContext>
{
    public OfferDbContext CreateDbContext(string[] args)
    {
        var appsettings = GetAppSettings().GetAwaiter().GetResult();
        var serviceCollection = Setup(appsettings);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        return serviceProvider.GetRequiredService<OfferDbContext>();
    }

    public static async Task Main(string[] args)
    {
        var appsettings = await GetAppSettings();
        var serviceCollection = Setup(appsettings);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        serviceProvider.UseInfrastructure();

        var bidInteractionService = serviceProvider.GetRequiredService<BidInteractionService>();
        await bidInteractionService.RunApp();
    }

    private static IServiceCollection Setup(Dictionary<string, object> appsettings)
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

    private static async Task<Dictionary<string, object>> GetAppSettings()
    {
        var appFolder = AppContext.BaseDirectory
            ?? throw new InvalidOperationException("Cannot get Application folder directory");
        using FileStream fileStream = File.Open(appFolder + Path.DirectorySeparatorChar + "appsettings.json", FileMode.Open, FileAccess.Read);
        var settings = await JsonSerializer.DeserializeAsync<Dictionary<string, object>>(fileStream)
                ?? throw new InvalidOperationException("Cannot deserialize 'appsettings.json', please ensure that this file exists");
        return settings;
    }
}
