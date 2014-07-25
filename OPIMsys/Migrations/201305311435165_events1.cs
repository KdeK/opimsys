namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class events1 : DbMigration
    {
        public override void Up()
        {
          /*  AddColumn("dbo.EventSources", "LanguageId", c => c.Int(nullable: false));
            AddForeignKey("dbo.EventSources", "LanguageId", "dbo.Languages", "LanguageId", cascadeDelete: true);
            CreateIndex("dbo.EventSources", "LanguageId");
           */ 
        }
        
        public override void Down()
        {
            /*
            DropIndex("dbo.EventSources", new[] { "LanguageId" });
            DropForeignKey("dbo.EventSources", "LanguageId", "dbo.Languages");
            DropColumn("dbo.EventSources", "LanguageId");
             */
        }
    }
}
