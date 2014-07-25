namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Events2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EventSources", "EventSourceTypeId", "dbo.EventSources");
            DropIndex("dbo.EventSources", new[] { "EventSourceTypeId" });
            AddForeignKey("dbo.EventSources", "EventSourceTypeId", "dbo.EventSourceTypes", "EventSourceTypeId", cascadeDelete: true);
            CreateIndex("dbo.EventSources", "EventSourceTypeId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.EventSources", new[] { "EventSourceTypeId" });
            DropForeignKey("dbo.EventSources", "EventSourceTypeId", "dbo.EventSourceTypes");
            CreateIndex("dbo.EventSources", "EventSourceTypeId");
            AddForeignKey("dbo.EventSources", "EventSourceTypeId", "dbo.EventSources", "EventSourceId");
        }
    }
}
