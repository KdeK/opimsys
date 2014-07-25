namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shares : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Shares",
                c => new
                    {
                        ShareId = c.Int(nullable: false, identity: true),
                        ClientRemoteId = c.String(),
                        StockQuoteId = c.Int(nullable: false),
                        TransactionDate = c.DateTime(nullable: false),
                        Price = c.Double(nullable: false),
                        Quantity = c.Int(nullable: false),
                        IsDrip = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ShareId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Shares");
        }
    }
}
