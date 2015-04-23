namespace TwilioRegistration.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HandlingHumanAccounts : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Accounts", new[] { "Email" });
            DropIndex("dbo.Accounts", new[] { "Prefix" });
            DropIndex("dbo.Accounts", new[] { "ServerId" });
            AddColumn("dbo.Accounts", "Username", c => c.String(nullable: false, maxLength: 255, defaultValue: ""));
            Sql("UPDATE [dbo].[Accounts] SET Username = Email");
            AddColumn("dbo.Accounts", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Accounts", "FirstName", c => c.String(maxLength: 255));
            AlterColumn("dbo.Accounts", "LastName", c => c.String(maxLength: 255));
            AlterColumn("dbo.Accounts", "Email", c => c.String(maxLength: 255));
            AlterColumn("dbo.Accounts", "Prefix", c => c.String(maxLength: 25));
            AlterColumn("dbo.Accounts", "ServerId", c => c.Int());
            CreateIndex("dbo.Accounts", "Username", unique: true);
            CreateIndex("dbo.Accounts", "ServerId");
            CreateIndex("dbo.Accounts", "Email", unique: true);
            CreateIndex("dbo.Accounts", "Prefix", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Accounts", new[] { "Prefix" });
            DropIndex("dbo.Accounts", new[] { "Email" });
            DropIndex("dbo.Accounts", new[] { "ServerId" });
            DropIndex("dbo.Accounts", new[] { "Username" });
            AlterColumn("dbo.Accounts", "ServerId", c => c.Int(nullable: false));
            AlterColumn("dbo.Accounts", "Prefix", c => c.String(nullable: false, maxLength: 25));
            AlterColumn("dbo.Accounts", "Email", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Accounts", "LastName", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Accounts", "FirstName", c => c.String(nullable: false, maxLength: 255));
            DropColumn("dbo.Accounts", "Discriminator");
            DropColumn("dbo.Accounts", "Username");
            CreateIndex("dbo.Accounts", "ServerId");
            CreateIndex("dbo.Accounts", "Prefix", unique: true);
            CreateIndex("dbo.Accounts", "Email", unique: true);
        }
    }
}
