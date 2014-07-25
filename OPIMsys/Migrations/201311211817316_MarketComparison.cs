namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MarketComparison : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyMarketComparisons",
                c => new
                    {
                        CompanyMarketComparisonId = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        StockSymbolId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CompanyMarketComparisonId)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: false)
                .ForeignKey("dbo.StockSymbols", t => t.StockSymbolId, cascadeDelete: false)
                .Index(t => t.CompanyId)
                .Index(t => t.StockSymbolId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.CompanyMarketComparisons", new[] { "StockSymbolId" });
            DropIndex("dbo.CompanyMarketComparisons", new[] { "CompanyId" });
            DropForeignKey("dbo.CompanyMarketComparisons", "StockSymbolId", "dbo.StockSymbols");
            DropForeignKey("dbo.CompanyMarketComparisons", "CompanyId", "dbo.Companies");
            DropTable("dbo.CompanyMarketComparisons");
        }
    }
}
