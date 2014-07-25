namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chartdata : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyDatas",
                c => new
                    {
                        CompanyDataId = c.Int(nullable: false, identity: true),
                        CompanyDataTypeId = c.Int(nullable: false),
                        CompanyId = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        publishDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CompanyDataId)
                .ForeignKey("dbo.CompanyDataTypes", t => t.CompanyDataTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyDataTypeId)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.CompanyDataTypes",
                c => new
                    {
                        CompanyDataTypeId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.CompanyDataTypeId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.CompanyDatas", new[] { "CompanyId" });
            DropIndex("dbo.CompanyDatas", new[] { "CompanyDataTypeId" });
            DropForeignKey("dbo.CompanyDatas", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.CompanyDatas", "CompanyDataTypeId", "dbo.CompanyDataTypes");
            DropTable("dbo.CompanyDataTypes");
            DropTable("dbo.CompanyDatas");
        }
    }
}
