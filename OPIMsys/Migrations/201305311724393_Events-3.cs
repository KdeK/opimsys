namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Events3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EventSources", "Company_CompanyId", "dbo.Companies");
            DropIndex("dbo.EventSources", new[] { "Company_CompanyId" });
            AddColumn("dbo.EventSources", "CompanyId", c => c.Int(nullable: false));
            AddForeignKey("dbo.EventSources", "CompanyId", "dbo.Companies", "CompanyId", cascadeDelete: false);
            CreateIndex("dbo.EventSources", "CompanyId");
            DropColumn("dbo.EventSources", "Company_CompanyId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EventSources", "Company_CompanyId", c => c.Int());
            DropIndex("dbo.EventSources", new[] { "CompanyId" });
            DropForeignKey("dbo.EventSources", "CompanyId", "dbo.Companies");
            DropColumn("dbo.EventSources", "CompanyId");
            CreateIndex("dbo.EventSources", "Company_CompanyId");
            AddForeignKey("dbo.EventSources", "Company_CompanyId", "dbo.Companies", "CompanyId");
        }
    }
}
