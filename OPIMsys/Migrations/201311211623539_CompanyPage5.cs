namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyPage5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompanyPages", "Title", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CompanyPages", "Title");
        }
    }
}
