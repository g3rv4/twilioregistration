namespace TwilioRegistration.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccountName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "FirstName", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Accounts", "LastName", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "LastName");
            DropColumn("dbo.Accounts", "FirstName");
        }
    }
}
