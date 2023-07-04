namespace OfferApp.Infrastructure.Database
{
    public class DatabaseOptions
    {
        public string? ConnectionString { get; set; }
        public bool RunMigrationsOnStart { get; set; } = false;
    }
}
