namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class groups : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyGroups",
                c => new
                    {
                        CompanyGroupId = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CompanyGroupId)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.CompanyId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        GroupId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.GroupId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.CompanyGroups", new[] { "GroupId" });
            DropIndex("dbo.CompanyGroups", new[] { "CompanyId" });
            DropForeignKey("dbo.CompanyGroups", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.CompanyGroups", "CompanyId", "dbo.Companies");
            DropTable("dbo.Groups");
            DropTable("dbo.CompanyGroups");
        }
    }
}
