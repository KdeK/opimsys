namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class documents2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "ThumbnailLink", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "ThumbnailLink");
        }
    }
}
