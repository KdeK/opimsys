namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class peopleChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "CompanyName", c => c.String());
            AlterColumn("dbo.People", "FirstName", c => c.String());
            AlterColumn("dbo.People", "LastName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.People", "LastName", c => c.String(nullable: false));
            AlterColumn("dbo.People", "FirstName", c => c.String(nullable: false));
            DropColumn("dbo.People", "CompanyName");
        }
    }
}
