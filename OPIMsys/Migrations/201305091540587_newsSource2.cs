namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newsSource2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "SourceUniqueId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.News", "SourceUniqueId");
        }
    }
}
