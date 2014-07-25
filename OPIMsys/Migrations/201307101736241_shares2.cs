namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shares2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Shares", "Quantity", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Shares", "Quantity", c => c.Int(nullable: false));
        }
    }
}
