namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class documents1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "PubDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "PubDate");
        }
    }
}
