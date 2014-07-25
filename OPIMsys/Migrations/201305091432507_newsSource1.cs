namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newsSource1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.News", "CompanyId", "dbo.Companies");
            DropIndex("dbo.News", new[] { "CompanyId" });
            AddColumn("dbo.News", "Company_CompanyId", c => c.Int());
            AddColumn("dbo.NewsSources", "CompanyId", c => c.Int(nullable: false));
            AddForeignKey("dbo.News", "Company_CompanyId", "dbo.Companies", "CompanyId");
            AddForeignKey("dbo.NewsSources", "CompanyId", "dbo.Companies", "CompanyId", cascadeDelete: true);
            CreateIndex("dbo.News", "Company_CompanyId");
            CreateIndex("dbo.NewsSources", "CompanyId");
            DropColumn("dbo.News", "CompanyId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.News", "CompanyId", c => c.Int(nullable: false));
            DropIndex("dbo.NewsSources", new[] { "CompanyId" });
            DropIndex("dbo.News", new[] { "Company_CompanyId" });
            DropForeignKey("dbo.NewsSources", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.News", "Company_CompanyId", "dbo.Companies");
            DropColumn("dbo.NewsSources", "CompanyId");
            DropColumn("dbo.News", "Company_CompanyId");
            CreateIndex("dbo.News", "CompanyId");
            AddForeignKey("dbo.News", "CompanyId", "dbo.Companies", "CompanyId", cascadeDelete: true);
        }
    }
}
