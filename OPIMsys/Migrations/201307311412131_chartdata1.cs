namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chartdata1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyDataTypeCompanies",
                c => new
                    {
                        CompanyDataTypeCompanyId = c.Int(nullable: false, identity: true),
                        CompanyDataTypeId = c.Int(nullable: false),
                        CompanyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CompanyDataTypeCompanyId)
                .ForeignKey("dbo.CompanyDataTypes", t => t.CompanyDataTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyDataTypeId)
                .Index(t => t.CompanyId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.CompanyDataTypeCompanies", new[] { "CompanyId" });
            DropIndex("dbo.CompanyDataTypeCompanies", new[] { "CompanyDataTypeId" });
            DropForeignKey("dbo.CompanyDataTypeCompanies", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.CompanyDataTypeCompanies", "CompanyDataTypeId", "dbo.CompanyDataTypes");
            DropTable("dbo.CompanyDataTypeCompanies");
        }
    }
}
