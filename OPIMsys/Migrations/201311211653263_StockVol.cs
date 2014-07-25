namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StockVol : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.StockHistories", "Volume", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StockHistories", "Volume", c => c.Int(nullable: false));
        }
    }
}
