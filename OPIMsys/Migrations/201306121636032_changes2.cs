namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changes2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "SourceId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "SourceId");
        }
    }
}
