namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class moreInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.People",
                c => new
                    {
                        PeopleId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Email = c.String(),
                        Phone = c.String(),
                        Address = c.String(),
                        CompanyId = c.Int(nullable: false),
                        PeopleTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PeopleId)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.PeopleTypes", t => t.PeopleTypeId, cascadeDelete: true)
                .Index(t => t.CompanyId)
                .Index(t => t.PeopleTypeId);
            
            CreateTable(
                "dbo.PeopleTypes",
                c => new
                    {
                        PeopleTypeId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.PeopleTypeId);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        NewsId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        DatePosted = c.DateTime(nullable: false),
                        PDFLink = c.String(nullable: false),
                        Link = c.String(nullable: false),
                        Content = c.String(nullable: false),
                        NewsTypeId = c.Int(nullable: false),
                        Language_Culture = c.String(nullable: false, maxLength: 128),
                        Company_CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.NewsId)
                .ForeignKey("dbo.Languages", t => t.Language_Culture, cascadeDelete: true)
                .ForeignKey("dbo.NewsTypes", t => t.NewsTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Companies", t => t.Company_CompanyId)
                .Index(t => t.Language_Culture)
                .Index(t => t.NewsTypeId)
                .Index(t => t.Company_CompanyId);
            
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Culture = c.String(nullable: false, maxLength: 128),
                        Title = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Culture);
            
            CreateTable(
                "dbo.NewsTypes",
                c => new
                    {
                        NewsTypeId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.NewsTypeId);
            
            CreateTable(
                "dbo.CompanyInformations",
                c => new
                    {
                        CompanyInformationId = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        ShortDescription = c.String(),
                        LongDescription = c.String(),
                        Revision = c.Int(nullable: false),
                        Language_Culture = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CompanyInformationId)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.Languages", t => t.Language_Culture)
                .Index(t => t.CompanyId)
                .Index(t => t.Language_Culture);
            
            CreateTable(
                "dbo.CompanyLinks",
                c => new
                    {
                        CompanyLinkId = c.Int(nullable: false, identity: true),
                        CompanyLinkTypeId = c.Int(nullable: false),
                        CompanyId = c.Int(nullable: false),
                        Link = c.String(nullable: false),
                        Language_Culture = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.CompanyLinkId)
                .ForeignKey("dbo.CompanyLinkTypes", t => t.CompanyLinkTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.Languages", t => t.Language_Culture, cascadeDelete: true)
                .Index(t => t.CompanyLinkTypeId)
                .Index(t => t.CompanyId)
                .Index(t => t.Language_Culture);
            
            CreateTable(
                "dbo.CompanyLinkTypes",
                c => new
                    {
                        CompanyLinkTypeId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CompanyLinkTypeId);
            
            CreateTable(
                "dbo.PeopleInformations",
                c => new
                    {
                        PeopleInformationId = c.Int(nullable: false, identity: true),
                        PeopleId = c.Int(nullable: false),
                        Title = c.String(),
                        Bio = c.String(),
                        Language_Culture = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PeopleInformationId)
                .ForeignKey("dbo.People", t => t.PeopleId, cascadeDelete: true)
                .ForeignKey("dbo.Languages", t => t.Language_Culture)
                .Index(t => t.PeopleId)
                .Index(t => t.Language_Culture);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.PeopleInformations", new[] { "Language_Culture" });
            DropIndex("dbo.PeopleInformations", new[] { "PeopleId" });
            DropIndex("dbo.CompanyLinks", new[] { "Language_Culture" });
            DropIndex("dbo.CompanyLinks", new[] { "CompanyId" });
            DropIndex("dbo.CompanyLinks", new[] { "CompanyLinkTypeId" });
            DropIndex("dbo.CompanyInformations", new[] { "Language_Culture" });
            DropIndex("dbo.CompanyInformations", new[] { "CompanyId" });
            DropIndex("dbo.News", new[] { "Company_CompanyId" });
            DropIndex("dbo.News", new[] { "NewsTypeId" });
            DropIndex("dbo.News", new[] { "Language_Culture" });
            DropIndex("dbo.People", new[] { "PeopleTypeId" });
            DropIndex("dbo.People", new[] { "CompanyId" });
            DropForeignKey("dbo.PeopleInformations", "Language_Culture", "dbo.Languages");
            DropForeignKey("dbo.PeopleInformations", "PeopleId", "dbo.People");
            DropForeignKey("dbo.CompanyLinks", "Language_Culture", "dbo.Languages");
            DropForeignKey("dbo.CompanyLinks", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.CompanyLinks", "CompanyLinkTypeId", "dbo.CompanyLinkTypes");
            DropForeignKey("dbo.CompanyInformations", "Language_Culture", "dbo.Languages");
            DropForeignKey("dbo.CompanyInformations", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.News", "Company_CompanyId", "dbo.Companies");
            DropForeignKey("dbo.News", "NewsTypeId", "dbo.NewsTypes");
            DropForeignKey("dbo.News", "Language_Culture", "dbo.Languages");
            DropForeignKey("dbo.People", "PeopleTypeId", "dbo.PeopleTypes");
            DropForeignKey("dbo.People", "CompanyId", "dbo.Companies");
            DropTable("dbo.PeopleInformations");
            DropTable("dbo.CompanyLinkTypes");
            DropTable("dbo.CompanyLinks");
            DropTable("dbo.CompanyInformations");
            DropTable("dbo.NewsTypes");
            DropTable("dbo.Languages");
            DropTable("dbo.News");
            DropTable("dbo.PeopleTypes");
            DropTable("dbo.People");
        }
    }
}
