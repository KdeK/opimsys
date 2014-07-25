namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newsSource : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.News", "Company_CompanyId", "dbo.Companies");
            DropIndex("dbo.News", new[] { "Company_CompanyId" });
            CreateTable(
                "dbo.NewsSources",
                c => new
                    {
                        NewsSourceId = c.Int(nullable: false, identity: true),
                        Link = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.NewsSourceId);
            
            AddColumn("dbo.News", "Description", c => c.String(nullable: false));
            AddColumn("dbo.News", "PubDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.News", "CompanyId", c => c.Int(nullable: false));
            AddColumn("dbo.News", "NewsSourceId", c => c.Int(nullable: false));
            AlterColumn("dbo.News", "PDFLink", c => c.String());
            AlterColumn("dbo.News", "Content", c => c.String());
            AddForeignKey("dbo.News", "CompanyId", "dbo.Companies", "CompanyId", cascadeDelete: true);
            AddForeignKey("dbo.News", "NewsSourceId", "dbo.NewsSources", "NewsSourceId", cascadeDelete: true);
            CreateIndex("dbo.News", "CompanyId");
            CreateIndex("dbo.News", "NewsSourceId");
            DropColumn("dbo.News", "DatePosted");
            DropColumn("dbo.News", "Company_CompanyId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.News", "Company_CompanyId", c => c.Int());
            AddColumn("dbo.News", "DatePosted", c => c.DateTime(nullable: false));
            DropIndex("dbo.News", new[] { "NewsSourceId" });
            DropIndex("dbo.News", new[] { "CompanyId" });
            DropForeignKey("dbo.News", "NewsSourceId", "dbo.NewsSources");
            DropForeignKey("dbo.News", "CompanyId", "dbo.Companies");
            AlterColumn("dbo.News", "Content", c => c.String(nullable: false));
            AlterColumn("dbo.News", "PDFLink", c => c.String(nullable: false));
            DropColumn("dbo.News", "NewsSourceId");
            DropColumn("dbo.News", "CompanyId");
            DropColumn("dbo.News", "PubDate");
            DropColumn("dbo.News", "Description");
            DropTable("dbo.NewsSources");
            CreateIndex("dbo.News", "Company_CompanyId");
            AddForeignKey("dbo.News", "Company_CompanyId", "dbo.Companies", "CompanyId");
        }
    }
}
