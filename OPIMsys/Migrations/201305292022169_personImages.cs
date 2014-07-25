namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class personImages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "ImageURL", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.People", "ImageURL");
        }
    }
}
