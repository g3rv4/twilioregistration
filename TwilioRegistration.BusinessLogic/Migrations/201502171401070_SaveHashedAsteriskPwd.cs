namespace TwilioRegistration.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SaveHashedAsteriskPwd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Devices", "HashedPassword", c => c.String(nullable: false));
            DropColumn("dbo.Devices", "Password");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Devices", "Password", c => c.String());
            DropColumn("dbo.Devices", "HashedPassword");
        }
    }
}
