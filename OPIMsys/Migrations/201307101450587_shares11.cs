namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shares11 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Shares", "ClientRemoteId", c => c.String(nullable: false));
            AddForeignKey("dbo.Shares", "StockSymbolId", "dbo.StockSymbols", "StockSymbolId", cascadeDelete: true);
            CreateIndex("dbo.Shares", "StockSymbolId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Shares", new[] { "StockSymbolId" });
            DropForeignKey("dbo.Shares", "StockSymbolId", "dbo.StockSymbols");
            AlterColumn("dbo.Shares", "ClientRemoteId", c => c.String());
        }
    }
}
