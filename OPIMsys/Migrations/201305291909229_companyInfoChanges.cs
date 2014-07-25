namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class companyInfoChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "LogoURL", c => c.String());
            AddColumn("dbo.CompanyInformations", "Strategy", c => c.String());
            AddColumn("dbo.CompanyInformations", "Contact", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CompanyInformations", "Contact");
            DropColumn("dbo.CompanyInformations", "Strategy");
            DropColumn("dbo.Companies", "LogoURL");
        }
    }
}
