namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newsSourceType1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NewsSourceTypes",
                c => new
                    {
                        NewsSourceTypeId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.NewsSourceTypeId);
            
            AddColumn("dbo.NewsSources", "NewsSourceTypeId", c => c.Int(nullable: false));
            AddForeignKey("dbo.NewsSources", "NewsSourceTypeId", "dbo.NewsSourceTypes", "NewsSourceTypeId", cascadeDelete: true);
            CreateIndex("dbo.NewsSources", "NewsSourceTypeId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.NewsSources", new[] { "NewsSourceTypeId" });
            DropForeignKey("dbo.NewsSources", "NewsSourceTypeId", "dbo.NewsSourceTypes");
            DropColumn("dbo.NewsSources", "NewsSourceTypeId");
            DropTable("dbo.NewsSourceTypes");
        }
    }
}
