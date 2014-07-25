namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.People", "PeopleTypeId", "dbo.PeopleTypes");
            DropIndex("dbo.People", new[] { "PeopleTypeId" });
            CreateTable(
                "dbo.PeopleJoinPeopleType",
                c => new
                    {
                        PeopleId = c.Int(nullable: false),
                        PeopleTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PeopleId, t.PeopleTypeId })
                .ForeignKey("dbo.People", t => t.PeopleId, cascadeDelete: true)
                .ForeignKey("dbo.PeopleTypes", t => t.PeopleTypeId, cascadeDelete: true)
                .Index(t => t.PeopleId)
                .Index(t => t.PeopleTypeId);
            
            DropColumn("dbo.People", "PeopleTypeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.People", "PeopleTypeId", c => c.Int(nullable: false));
            DropIndex("dbo.PeopleJoinPeopleType", new[] { "PeopleTypeId" });
            DropIndex("dbo.PeopleJoinPeopleType", new[] { "PeopleId" });
            DropForeignKey("dbo.PeopleJoinPeopleType", "PeopleTypeId", "dbo.PeopleTypes");
            DropForeignKey("dbo.PeopleJoinPeopleType", "PeopleId", "dbo.People");
            DropTable("dbo.PeopleJoinPeopleType");
            CreateIndex("dbo.People", "PeopleTypeId");
            AddForeignKey("dbo.People", "PeopleTypeId", "dbo.PeopleTypes", "PeopleTypeId", cascadeDelete: true);
        }
    }
}
