using Microsoft.Extensions.DependencyInjection;

namespace OfferApp.IntegrationTests.Common
{
    [Collection(COLLECTION_TEST_NAME)]
    public abstract class BaseTest : IDisposable
    {
        public const string COLLECTION_TEST_NAME = "integration-offer-testing";

        protected HttpClient Client;
        private readonly TestApplicationFactory Fixture;
        private IServiceScope? Scope;

        public BaseTest(TestApplicationFactory testApplicationFactory)
        {
            Fixture = testApplicationFactory;
            Client = testApplicationFactory.CreateClient();
        }

        protected T? GetService<T>()
        {
            Scope ??= Fixture.Services.CreateScope();
            return Scope.ServiceProvider.GetService<T>();
        }

        protected T GetRequiredService<T>() where T : notnull
        {
            Scope ??= Fixture.Services.CreateScope();
            return Scope.ServiceProvider.GetRequiredService<T>();
        }

        public virtual void Dispose()
        {
            Scope?.Dispose();
        }
    }
}
