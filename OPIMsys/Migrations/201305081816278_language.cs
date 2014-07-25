namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class language : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CompanyInformations", "Language_Culture", "dbo.Languages");
            DropIndex("dbo.CompanyInformations", new[] { "Language_Culture" });
            RenameColumn(table: "dbo.CompanyInformations", name: "Language_Culture", newName: "Culture");
            RenameColumn(table: "dbo.CompanyLinks", name: "Language_Culture", newName: "Culture");
            AddColumn("dbo.Companies", "Revision", c => c.Int(nullable: false));
            AddColumn("dbo.CompanyInformations", "Slogan", c => c.String());
            AddForeignKey("dbo.CompanyInformations", "Culture", "dbo.Languages", "Culture", cascadeDelete: true);
            CreateIndex("dbo.CompanyInformations", "Culture");
            DropColumn("dbo.CompanyInformations", "Revision");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CompanyInformations", "Revision", c => c.Int(nullable: false));
            DropIndex("dbo.CompanyInformations", new[] { "Culture" });
            DropForeignKey("dbo.CompanyInformations", "Culture", "dbo.Languages");
            DropColumn("dbo.CompanyInformations", "Slogan");
            DropColumn("dbo.Companies", "Revision");
            RenameColumn(table: "dbo.CompanyLinks", name: "Culture", newName: "Language_Culture");
            RenameColumn(table: "dbo.CompanyInformations", name: "Culture", newName: "Language_Culture");
            CreateIndex("dbo.CompanyInformations", "Language_Culture");
            AddForeignKey("dbo.CompanyInformations", "Language_Culture", "dbo.Languages", "Culture");
        }
    }
}
