namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class webapikey : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserApiKey",
                c => new
                    {
                        AccountApiKeyId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        LoginToken = c.String(),
                        ApiKey = c.String(),
                    })
                .PrimaryKey(t => t.AccountApiKeyId)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserApiKey", new[] { "UserId" });
            DropForeignKey("dbo.UserApiKey", "UserId", "dbo.UserProfile");
            DropTable("dbo.UserApiKey");
        }
    }
}
