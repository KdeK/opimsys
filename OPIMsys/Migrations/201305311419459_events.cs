namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class events : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventId = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        EventCategoryId = c.Int(nullable: false),
                        EventSourceId = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.EventId)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.EventCategories", t => t.EventCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.EventSources", t => t.EventSourceId, cascadeDelete: true)
                .Index(t => t.CompanyId)
                .Index(t => t.EventCategoryId)
                .Index(t => t.EventSourceId);
            
            CreateTable(
                "dbo.EventCategories",
                c => new
                    {
                        EventCategoryId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.EventCategoryId);
            
            CreateTable(
                "dbo.EventSources",
                c => new
                    {
                        EventSourceId = c.Int(nullable: false, identity: true),
                        Link = c.String(nullable: false),
                        EventSourceTypeId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        Company_CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.EventSourceId)
                .ForeignKey("dbo.EventSources", t => t.EventSourceTypeId)
                .ForeignKey("dbo.Languages", t => t.LanguageId, cascadeDelete: false)
                .ForeignKey("dbo.Companies", t => t.Company_CompanyId)
                .Index(t => t.EventSourceTypeId)
                .Index(t => t.LanguageId)
                .Index(t => t.Company_CompanyId);
            
            CreateTable(
                "dbo.EventDetails",
                c => new
                    {
                        EventDetailId = c.Int(nullable: false, identity: true),
                        EventId = c.Int(nullable: false),
                        Summary = c.String(nullable: false),
                        Description = c.String(),
                        LanguageId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EventDetailId)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .ForeignKey("dbo.Languages", t => t.LanguageId, cascadeDelete: false)
                .Index(t => t.EventId)
                .Index(t => t.LanguageId);
            
            CreateTable(
                "dbo.EventSourceTypes",
                c => new
                    {
                        EventSourceTypeId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.EventSourceTypeId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.EventDetails", new[] { "LanguageId" });
            DropIndex("dbo.EventDetails", new[] { "EventId" });
            DropIndex("dbo.EventSources", new[] { "Company_CompanyId" });
            DropIndex("dbo.EventSources", new[] { "LanguageId" });
            DropIndex("dbo.EventSources", new[] { "EventSourceTypeId" });
            DropIndex("dbo.Events", new[] { "EventSourceId" });
            DropIndex("dbo.Events", new[] { "EventCategoryId" });
            DropIndex("dbo.Events", new[] { "CompanyId" });
            DropForeignKey("dbo.EventDetails", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.EventDetails", "EventId", "dbo.Events");
            DropForeignKey("dbo.EventSources", "Company_CompanyId", "dbo.Companies");
            DropForeignKey("dbo.EventSources", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.EventSources", "EventSourceTypeId", "dbo.EventSources");
            DropForeignKey("dbo.Events", "EventSourceId", "dbo.EventSources");
            DropForeignKey("dbo.Events", "EventCategoryId", "dbo.EventCategories");
            DropForeignKey("dbo.Events", "CompanyId", "dbo.Companies");
            DropTable("dbo.EventSourceTypes");
            DropTable("dbo.EventDetails");
            DropTable("dbo.EventSources");
            DropTable("dbo.EventCategories");
            DropTable("dbo.Events");
        }
    }
}
