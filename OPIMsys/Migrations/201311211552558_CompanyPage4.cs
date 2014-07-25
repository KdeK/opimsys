namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyPage4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompanyPages", "Enabled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CompanyPages", "Enabled");
        }
    }
}
