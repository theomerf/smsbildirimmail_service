namespace Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MailRequests", "Cc", c => c.String());
            AddColumn("dbo.MailRequests", "Bcc", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MailRequests", "Bcc");
            DropColumn("dbo.MailRequests", "Cc");
        }
    }
}
