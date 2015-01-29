namespace TwilioRegistration.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Temporarily_Disabling_Accounts_On_Redis : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Accounts", "FailedLoginAttempts");
            DropColumn("dbo.Accounts", "ReactivationTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Accounts", "ReactivationTime", c => c.DateTime());
            AddColumn("dbo.Accounts", "FailedLoginAttempts", c => c.Int(nullable: false));
        }
    }
}
