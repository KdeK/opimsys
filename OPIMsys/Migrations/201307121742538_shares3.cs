namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shares3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Shares", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Shares", "Quantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Shares", "Quantity", c => c.Double(nullable: false));
            AlterColumn("dbo.Shares", "Price", c => c.Double(nullable: false));
        }
    }
}
