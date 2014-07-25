namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stockchange1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.StockQuotes", "MarketCap", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StockQuotes", "MarketCap", c => c.Double());
        }
    }
}
