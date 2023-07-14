namespace OfferApp.IntegrationTests.Common
{
    internal class ErrorModel
    {
        public int Status { get; set; }
        public Dictionary<string, IEnumerable<string>> Errors { get; set; } = new Dictionary<string, IEnumerable<string>>();
    }
}
