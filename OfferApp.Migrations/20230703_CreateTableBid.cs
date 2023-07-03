using FluentMigrator;

namespace OfferApp.Migrations
{
    [Migration(20230703191215)]
    public class CreateTableBid : Migration
    {
        public override void Down()
        {
            Execute.Script("delete_bid_table.sql");
        }

        public override void Up()
        {
            Execute.Script("create_bid_table.sql");
        }
    }
}
