namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shares1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shares", "StockSymbolId", c => c.Int(nullable: false));
            DropColumn("dbo.Shares", "StockQuoteId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Shares", "StockQuoteId", c => c.Int(nullable: false));
            DropColumn("dbo.Shares", "StockSymbolId");
        }
    }
}
