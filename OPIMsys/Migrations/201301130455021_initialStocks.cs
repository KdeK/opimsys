namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialStocks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StockSymbols",
                c => new
                    {
                        StockSymbolId = c.Int(nullable: false, identity: true),
                        Symbol = c.String(),
                        CompanyId = c.Int(nullable: false),
                        MarketId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StockSymbolId)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.StockHistories",
                c => new
                    {
                        StockHistoryId = c.Int(nullable: false, identity: true),
                        HistoryDate = c.DateTime(nullable: false),
                        Open = c.Decimal(nullable: false, precision: 18, scale: 2),
                        High = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Low = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Close = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Volume = c.Int(nullable: false),
                        AdjClose = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StockSymbolId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StockHistoryId)
                .ForeignKey("dbo.StockSymbols", t => t.StockSymbolId, cascadeDelete: true)
                .Index(t => t.StockSymbolId);
            
            CreateTable(
                "dbo.StockQuotes",
                c => new
                    {
                        StockQuoteId = c.Int(nullable: false, identity: true),
                        QuoteDate = c.DateTime(nullable: false),
                        TradeDate = c.DateTime(),
                        Open = c.Decimal(nullable: false, precision: 18, scale: 2),
                        High = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Low = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Change = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Volume = c.Int(nullable: false),
                        MarketCap = c.Int(),
                        EarningPerShare = c.Decimal(precision: 18, scale: 2),
                        Dividend = c.Decimal(precision: 18, scale: 2),
                        LastPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StockSymbolId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StockQuoteId)
                .ForeignKey("dbo.StockSymbols", t => t.StockSymbolId, cascadeDelete: true)
                .Index(t => t.StockSymbolId);
            
            CreateTable(
                "dbo.Markets",
                c => new
                    {
                        MarketId = c.Int(nullable: false, identity: true),
                        MarketName = c.String(),
                    })
                .PrimaryKey(t => t.MarketId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.StockQuotes", new[] { "StockSymbolId" });
            DropIndex("dbo.StockHistories", new[] { "StockSymbolId" });
            DropIndex("dbo.StockSymbols", new[] { "CompanyId" });
            DropForeignKey("dbo.StockQuotes", "StockSymbolId", "dbo.StockSymbols");
            DropForeignKey("dbo.StockHistories", "StockSymbolId", "dbo.StockSymbols");
            DropForeignKey("dbo.StockSymbols", "CompanyId", "dbo.Companies");
            DropTable("dbo.Markets");
            DropTable("dbo.StockQuotes");
            DropTable("dbo.StockHistories");
            DropTable("dbo.StockSymbols");
        }
    }
}
