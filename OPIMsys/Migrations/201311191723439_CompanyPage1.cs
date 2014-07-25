namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyPage1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyPages",
                c => new
                    {
                        CompanyPageId = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        PageName = c.String(nullable: false),
                        Revision = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        publishDate = c.DateTime(nullable: false),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.CompanyPageId)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.CompanyPages", new[] { "CompanyId" });
            DropForeignKey("dbo.CompanyPages", "CompanyId", "dbo.Companies");
            DropTable("dbo.CompanyPages");
        }
    }
}
