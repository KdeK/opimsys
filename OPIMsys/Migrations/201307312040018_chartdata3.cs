namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chartdata3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CompanyDatas", "CompanyDataVariableId", "dbo.CompanyDataTypes");
            DropIndex("dbo.CompanyDatas", new[] { "CompanyDataVariableId" });
            AddForeignKey("dbo.CompanyDatas", "CompanyDataVariableId", "dbo.CompanyDataVariables", "CompanyDataVariableId", cascadeDelete: true);
            CreateIndex("dbo.CompanyDatas", "CompanyDataVariableId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CompanyDatas", new[] { "CompanyDataVariableId" });
            DropForeignKey("dbo.CompanyDatas", "CompanyDataVariableId", "dbo.CompanyDataVariables");
            CreateIndex("dbo.CompanyDatas", "CompanyDataVariableId");
            AddForeignKey("dbo.CompanyDatas", "CompanyDataVariableId", "dbo.CompanyDataTypes", "CompanyDataTypeId", cascadeDelete: true);
        }
    }
}
