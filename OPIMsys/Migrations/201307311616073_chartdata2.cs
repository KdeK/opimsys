namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chartdata2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CompanyDatas", "CompanyDataTypeId", "dbo.CompanyDataTypes");
            DropIndex("dbo.CompanyDatas", new[] { "CompanyDataTypeId" });
            CreateTable(
                "dbo.CompanyDataVariables",
                c => new
                    {
                        CompanyDataVariableId = c.Int(nullable: false, identity: true),
                        CompanyDataTypeId = c.Int(nullable: false),
                        Title = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CompanyDataVariableId)
                .ForeignKey("dbo.CompanyDataTypes", t => t.CompanyDataTypeId, cascadeDelete: true)
                .Index(t => t.CompanyDataTypeId);
            
            AddColumn("dbo.CompanyDatas", "CompanyDataVariableId", c => c.Int(nullable: false));
            AddForeignKey("dbo.CompanyDatas", "CompanyDataVariableId", "dbo.CompanyDataTypes", "CompanyDataTypeId", cascadeDelete: true);
            CreateIndex("dbo.CompanyDatas", "CompanyDataVariableId");
            DropColumn("dbo.CompanyDatas", "CompanyDataTypeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CompanyDatas", "CompanyDataTypeId", c => c.Int(nullable: false));
            DropIndex("dbo.CompanyDataVariables", new[] { "CompanyDataTypeId" });
            DropIndex("dbo.CompanyDatas", new[] { "CompanyDataVariableId" });
            DropForeignKey("dbo.CompanyDataVariables", "CompanyDataTypeId", "dbo.CompanyDataTypes");
            DropForeignKey("dbo.CompanyDatas", "CompanyDataVariableId", "dbo.CompanyDataTypes");
            DropColumn("dbo.CompanyDatas", "CompanyDataVariableId");
            DropTable("dbo.CompanyDataVariables");
            CreateIndex("dbo.CompanyDatas", "CompanyDataTypeId");
            AddForeignKey("dbo.CompanyDatas", "CompanyDataTypeId", "dbo.CompanyDataTypes", "CompanyDataTypeId", cascadeDelete: true);
        }
    }
}
