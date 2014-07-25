namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dividends : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StockDividends",
                c => new
                    {
                        StockDividendId = c.Int(nullable: false, identity: true),
                        RecordDate = c.DateTime(nullable: false),
                        PayableDate = c.DateTime(nullable: false),
                        ExDividendDate = c.DateTime(nullable: false),
                        Dividend = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Notes = c.String(),
                        StockSymbolId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StockDividendId)
                .ForeignKey("dbo.StockSymbols", t => t.StockSymbolId, cascadeDelete: true)
                .Index(t => t.StockSymbolId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.StockDividends", new[] { "StockSymbolId" });
            DropForeignKey("dbo.StockDividends", "StockSymbolId", "dbo.StockSymbols");
            DropTable("dbo.StockDividends");
        }
    }
}
