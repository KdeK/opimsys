namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newsSourceLang3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.News", "Description", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.News", "Description", c => c.String(nullable: false));
        }
    }
}
