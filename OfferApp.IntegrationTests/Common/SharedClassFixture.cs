namespace OfferApp.IntegrationTests.Common
{
    [CollectionDefinition(BaseTest.COLLECTION_TEST_NAME)]
    public class SharedClassFixture : IClassFixture<TestApplicationFactory>
    {
    }
}
