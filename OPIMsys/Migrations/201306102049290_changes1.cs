namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changes1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "SourceId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "SourceId");
        }
    }
}
