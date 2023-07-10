using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OfferApp.Infrastructure;
using OfferApp.IntegrationTests.Common;
using Shouldly;

namespace OfferApp.IntegrationTests.Controllers
{
    public class HealthCheckControllerTests : BaseTest
    {
        [Fact]
        public async Task ShouldGetHealthCheckStatus()
        {
            var options = GetRequiredService<IOptions<AppOptions>>();

            var response = await Client.GetAsync("api");

            ((int)response.StatusCode).ShouldBe(StatusCodes.Status200OK);
            var healthCheckStatus = await response.Content.ReadAsStringAsync();
            healthCheckStatus.ShouldBe($"Welcome to {options.Value.Name}");
        }

        public HealthCheckControllerTests(TestApplicationFactory testApplicationFactory)
            : base(testApplicationFactory)
        {
        }
    }
}