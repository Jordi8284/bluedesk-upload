namespace BluedeskUpload.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class database : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Upload_UploadId", "dbo.Uploads");
            DropIndex("dbo.AspNetUsers", new[] { "Upload_UploadId" });
            DropColumn("dbo.AspNetUsers", "Upload_UploadId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Upload_UploadId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Upload_UploadId");
            AddForeignKey("dbo.AspNetUsers", "Upload_UploadId", "dbo.Uploads", "UploadId");
        }
    }
}
