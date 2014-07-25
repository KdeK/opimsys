namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyPage3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompanyPages", "LanguageId", c => c.Int(nullable: false));
            AddForeignKey("dbo.CompanyPages", "LanguageId", "dbo.Languages", "LanguageId", cascadeDelete: true);
            CreateIndex("dbo.CompanyPages", "LanguageId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CompanyPages", new[] { "LanguageId" });
            DropForeignKey("dbo.CompanyPages", "LanguageId", "dbo.Languages");
            DropColumn("dbo.CompanyPages", "LanguageId");
        }
    }
}
