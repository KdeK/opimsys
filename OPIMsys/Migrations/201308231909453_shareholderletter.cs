namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shareholderletter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompanyInformations", "Letter", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CompanyInformations", "Letter");
        }
    }
}
