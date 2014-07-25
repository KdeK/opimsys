namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class webapikey1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserApiKey", "CompanyId", c => c.Int(nullable: false));
            AddForeignKey("dbo.UserApiKey", "CompanyId", "dbo.Companies", "CompanyId", cascadeDelete: true);
            CreateIndex("dbo.UserApiKey", "CompanyId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserApiKey", new[] { "CompanyId" });
            DropForeignKey("dbo.UserApiKey", "CompanyId", "dbo.Companies");
            DropColumn("dbo.UserApiKey", "CompanyId");
        }
    }
}
