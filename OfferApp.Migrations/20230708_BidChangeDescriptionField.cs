using FluentMigrator;

namespace OfferApp.Migrations
{
    [Migration(20230708223510)]
    public class BidChangeDescriptionField : Migration
    {
        public override void Down()
        {
            // API odpala skrypty w innym miejscu niż Console App
            // Z tego względu potrzebowałem odnieść się do bazowej lokalizacji
            // Oba projekty działają z tą ścieżką
            Execute.Script($"{AppDomain.CurrentDomain.BaseDirectory}bid_change_description_field_down.sql");
        }

        public override void Up()
        {
            // API odpala skrypty w innym miejscu niż Console App
            // Z tego względu potrzebowałem odnieść się do bazowej lokalizacji
            // Oba projekty działają z tą ścieżką
            Execute.Script($"{AppDomain.CurrentDomain.BaseDirectory}bid_change_description_field_up.sql");
        }
    }
}
