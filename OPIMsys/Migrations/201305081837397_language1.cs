namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class language1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.News", "Language_Culture", "dbo.Languages");
            DropIndex("dbo.News", new[] { "Language_Culture" });
            DropColumn("dbo.News", "Language_Culture", null);
            AddColumn("dbo.News", "LanguageId", c => c.Int(nullable: false));
            

            DropForeignKey("dbo.CompanyInformations", "Culture", "dbo.Languages");
            DropIndex("dbo.CompanyInformations", new[] { "Culture" });
           DropColumn("dbo.CompanyInformations", "Culture", null);
            AddColumn("dbo.CompanyInformations", "LanguageId", c => c.Int(nullable: false));

            DropForeignKey("dbo.PeopleInformations", "Language_Culture", "dbo.Languages");
            DropIndex("dbo.PeopleInformations", new[] { "Language_Culture" });
            DropColumn("dbo.PeopleInformations", "Language_Culture", null);
            AddColumn("dbo.PeopleInformations", "LanguageId", c => c.Int(nullable: false));

            DropForeignKey("dbo.CompanyLinks", "Culture", "dbo.Languages");
            DropIndex("dbo.CompanyLinks", new[] { "Culture" });
            AddColumn("dbo.CompanyLinks", "LanguageId", c => c.Int(nullable: false));
            DropColumn("dbo.CompanyLinks", "Culture", null);
            
            DropPrimaryKey("dbo.Languages", new[] { "Culture" });
            
            AddColumn("dbo.Languages", "LanguageId", c => c.Int(nullable: false, identity: true));
            
            AlterColumn("dbo.Languages", "Culture", c => c.String(nullable: false));
            AddPrimaryKey("dbo.Languages", "LanguageId");
            
            AddForeignKey("dbo.News", "LanguageId", "dbo.Languages", "LanguageId", cascadeDelete: true);
            AddForeignKey("dbo.CompanyInformations", "LanguageId", "dbo.Languages", "LanguageId", cascadeDelete: true);
            AddForeignKey("dbo.CompanyLinks", "LanguageId", "dbo.Languages", "LanguageId", cascadeDelete: true);
            AddForeignKey("dbo.PeopleInformations", "LanguageId", "dbo.Languages", "LanguageId", cascadeDelete: true);
            CreateIndex("dbo.News", "LanguageId");
            CreateIndex("dbo.CompanyInformations", "LanguageId");
            CreateIndex("dbo.CompanyLinks", "LanguageId");
            CreateIndex("dbo.PeopleInformations", "LanguageId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.PeopleInformations", new[] { "LanguageId" });
            DropIndex("dbo.CompanyLinks", new[] { "LanguageId" });
            DropIndex("dbo.CompanyInformations", new[] { "LanguageId" });
            DropIndex("dbo.News", new[] { "LanguageId" });
            DropForeignKey("dbo.PeopleInformations", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.CompanyLinks", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.CompanyInformations", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.News", "LanguageId", "dbo.Languages");
            DropPrimaryKey("dbo.Languages", new[] { "LanguageId" });
            AddPrimaryKey("dbo.Languages", "Culture");
            AlterColumn("dbo.Languages", "Culture", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Languages", "LanguageId");
            RenameColumn(table: "dbo.PeopleInformations", name: "LanguageId", newName: "Language_Culture");
            RenameColumn(table: "dbo.CompanyLinks", name: "LanguageId", newName: "Culture");
            RenameColumn(table: "dbo.CompanyInformations", name: "LanguageId", newName: "Culture");
            RenameColumn(table: "dbo.News", name: "LanguageId", newName: "Language_Culture");
            CreateIndex("dbo.PeopleInformations", "Language_Culture");
            CreateIndex("dbo.CompanyLinks", "Culture");
            CreateIndex("dbo.CompanyInformations", "Culture");
            CreateIndex("dbo.News", "Language_Culture");
            AddForeignKey("dbo.PeopleInformations", "Language_Culture", "dbo.Languages", "Culture");
            AddForeignKey("dbo.CompanyLinks", "Culture", "dbo.Languages", "Culture", cascadeDelete: true);
            AddForeignKey("dbo.CompanyInformations", "Culture", "dbo.Languages", "Culture", cascadeDelete: true);
            AddForeignKey("dbo.News", "Language_Culture", "dbo.Languages", "Culture", cascadeDelete: true);
        }
    }
}
