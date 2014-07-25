namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dividends1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.StockDividends", "Dividend", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StockDividends", "Dividend", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
