namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyPage2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompanyPages", "PageId", c => c.Int(nullable: false));
            AddColumn("dbo.CompanyPages", "UserName", c => c.String(nullable: false));
            AddColumn("dbo.CompanyPages", "EditDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CompanyPages", "PublishDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.CompanyPages", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CompanyPages", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.CompanyPages", "publishDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.CompanyPages", "EditDate");
            DropColumn("dbo.CompanyPages", "UserName");
            DropColumn("dbo.CompanyPages", "PageId");
        }
    }
}
