using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using OfferApp.Infrastructure.Database;

namespace OfferApp.IntegrationTests.Common
{
    public class TestApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(SetupEFCoreInMemory);
            builder.UseEnvironment("test");
        }

        public override async ValueTask DisposeAsync()
        {
            var scope = Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<OfferDbContext>();
            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
            scope.Dispose();
            await base.DisposeAsync();
        }

        private static void SetupEFCoreInMemory(IServiceCollection services)
        {
            // usunięcie aktualnej implementacji DbContext a następnie dodanie InMemory DbContext wraz z nowym DbInitializerem
            var contextDb = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(OfferDbContext));
            if (contextDb != null)
            {
                services.Remove(contextDb);
                var options = services.Where(r => (r.ServiceType == typeof(DbContextOptions))
                  || (r.ServiceType.IsGenericType && r.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>))).ToList();

                foreach (var option in options)
                {
                    services.Remove(option);
                }
            }
            services.AddDbContext<OfferDbContext>(options => options.UseInMemoryDatabase(nameof(OfferDbContext)));
            services.AddTransient<IDbInitializer, EFInMemoryDbInitializer>();
        }
    }
}
