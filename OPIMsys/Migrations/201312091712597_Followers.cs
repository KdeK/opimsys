namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Followers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyFollowers",
                c => new
                    {
                        CompanyFollowerId = c.Int(nullable: false, identity: true),
                        EmailAddress = c.String(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        CompanyName = c.String(),
                        CompanyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CompanyFollowerId)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.CompanyFollowers", new[] { "CompanyId" });
            DropForeignKey("dbo.CompanyFollowers", "CompanyId", "dbo.Companies");
            DropTable("dbo.CompanyFollowers");
        }
    }
}
