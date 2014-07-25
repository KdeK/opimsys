namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stocks2 : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.StockSymbols", "MarketId", "dbo.Markets", "MarketId", cascadeDelete: true);
            CreateIndex("dbo.StockSymbols", "MarketId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.StockSymbols", new[] { "MarketId" });
            DropForeignKey("dbo.StockSymbols", "MarketId", "dbo.Markets");
        }
    }
}
