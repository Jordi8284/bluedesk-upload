namespace BluedeskUpload.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class logo2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Downloads", "Gebruiker_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Downloads", new[] { "Gebruiker_Id" });
            DropColumn("dbo.Downloads", "Gebruiker_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Downloads", "Gebruiker_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Downloads", "Gebruiker_Id");
            AddForeignKey("dbo.Downloads", "Gebruiker_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
