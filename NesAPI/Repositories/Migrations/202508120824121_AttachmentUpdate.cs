namespace Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AttachmentUpdate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.MailAttachments", "FileSize");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MailAttachments", "FileSize", c => c.Long(nullable: false));
        }
    }
}
