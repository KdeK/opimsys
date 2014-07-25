namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newsSourceLang2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.News", "LanguageId", "dbo.Languages");
            DropIndex("dbo.News", new[] { "LanguageId" });
            AddColumn("dbo.NewsSources", "LanguageId", c => c.Int(nullable: false));
            AlterColumn("dbo.News", "NewsSourceIdentifier", c => c.String(nullable: false));
            AddForeignKey("dbo.NewsSources", "LanguageId", "dbo.Languages", "LanguageId", cascadeDelete: true);
            CreateIndex("dbo.NewsSources", "LanguageId");
            DropColumn("dbo.News", "LanguageId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.News", "LanguageId", c => c.Int(nullable: false));
            DropIndex("dbo.NewsSources", new[] { "LanguageId" });
            DropForeignKey("dbo.NewsSources", "LanguageId", "dbo.Languages");
            AlterColumn("dbo.News", "NewsSourceIdentifier", c => c.String());
            DropColumn("dbo.NewsSources", "LanguageId");
            CreateIndex("dbo.News", "LanguageId");
            AddForeignKey("dbo.News", "LanguageId", "dbo.Languages", "LanguageId", cascadeDelete: true);
        }
    }
}
