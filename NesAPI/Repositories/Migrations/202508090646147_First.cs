namespace Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class First : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MailAttachments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        MailId = c.Guid(nullable: false),
                        FileName = c.String(nullable: false),
                        ContentType = c.String(nullable: false),
                        FileSize = c.Long(nullable: false),
                        FilePath = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MailRequests", t => t.MailId, cascadeDelete: true)
                .Index(t => t.MailId);
            
            CreateTable(
                "dbo.MailRequests",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Subject = c.String(nullable: false, maxLength: 100),
                        Body = c.String(nullable: false),
                        IsBodyHtml = c.Boolean(nullable: false),
                        To = c.String(nullable: false),
                        From = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        CorrelationId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Requests", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Type = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NotificationRequests",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                        Body = c.String(nullable: false),
                        To = c.String(nullable: false),
                        From = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        CorrelationId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Requests", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.SmsRequests",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Message = c.String(nullable: false, maxLength: 200),
                        To = c.String(nullable: false),
                        From = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        CorrelationId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Requests", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MailAttachments", "MailId", "dbo.MailRequests");
            DropForeignKey("dbo.SmsRequests", "Id", "dbo.Requests");
            DropForeignKey("dbo.NotificationRequests", "Id", "dbo.Requests");
            DropForeignKey("dbo.MailRequests", "Id", "dbo.Requests");
            DropIndex("dbo.SmsRequests", new[] { "Id" });
            DropIndex("dbo.NotificationRequests", new[] { "Id" });
            DropIndex("dbo.MailRequests", new[] { "Id" });
            DropIndex("dbo.MailAttachments", new[] { "MailId" });
            DropTable("dbo.SmsRequests");
            DropTable("dbo.NotificationRequests");
            DropTable("dbo.Requests");
            DropTable("dbo.MailRequests");
            DropTable("dbo.MailAttachments");
        }
    }
}
