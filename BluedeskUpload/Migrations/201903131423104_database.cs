namespace BluedeskUpload.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class database : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Downloads",
                c => new
                    {
                        DownloadId = c.Int(nullable: false, identity: true),
                        UploadId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DownloadId)
                .ForeignKey("dbo.Uploads", t => t.UploadId, cascadeDelete: true)
                .Index(t => t.UploadId);
            
            CreateTable(
                "dbo.Uploads",
                c => new
                    {
                        UploadId = c.Int(nullable: false, identity: true),
                        Datum = c.DateTime(nullable: false),
                        Bestand = c.String(),
                        Omschrijving = c.String(),
                        Bedrijfsnaam = c.String(),
                        Naam = c.String(),
                        Email = c.String(),
                        Telefoon = c.String(),
                        Gebruiker_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UploadId)
                .ForeignKey("dbo.AspNetUsers", t => t.Gebruiker_Id)
                .Index(t => t.Gebruiker_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Downloads", "UploadId", "dbo.Uploads");
            DropForeignKey("dbo.Uploads", "Gebruiker_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Uploads", new[] { "Gebruiker_Id" });
            DropIndex("dbo.Downloads", new[] { "UploadId" });
            DropTable("dbo.Uploads");
            DropTable("dbo.Downloads");
        }
    }
}
