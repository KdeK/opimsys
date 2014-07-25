namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stocks3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Markets", "MarketOpen", c => c.String());
            AddColumn("dbo.Markets", "MarketClose", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Markets", "MarketClose");
            DropColumn("dbo.Markets", "MarketOpen");
        }
    }
}
