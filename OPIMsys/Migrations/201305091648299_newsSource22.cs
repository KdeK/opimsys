namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newsSource22 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "NewsSourceIdentifier", c => c.String());
        //    DropColumn("dbo.News", "SourceUniqueId");
        }
        
        public override void Down()
        {
        //    AddColumn("dbo.News", "SourceUniqueId", c => c.String());
            DropColumn("dbo.News", "NewsSourceIdentifier");
        }
    }
}
