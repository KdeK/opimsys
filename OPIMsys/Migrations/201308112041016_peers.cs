namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class peers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyPeers",
                c => new
                    {
                        CompanyPeerId = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        PeerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CompanyPeerId)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.CompanyPeers", new[] { "CompanyId" });
            DropForeignKey("dbo.CompanyPeers", "CompanyId", "dbo.Companies");
            DropTable("dbo.CompanyPeers");
        }
    }
}
