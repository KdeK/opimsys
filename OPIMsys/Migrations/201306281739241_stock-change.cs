namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stockchange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.StockQuotes", "MarketCap", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StockQuotes", "MarketCap", c => c.Int());
        }
    }
}
