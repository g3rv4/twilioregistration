namespace TwilioRegistration.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FailedLogins : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "FailedLoginAttempts", c => c.Int(nullable: false));
            AddColumn("dbo.Accounts", "ReactivationTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "ReactivationTime");
            DropColumn("dbo.Accounts", "FailedLoginAttempts");
        }
    }
}
