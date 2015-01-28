namespace TwilioRegistration.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 255),
                        HashedPassword = c.String(nullable: false),
                        Prefix = c.String(nullable: false, maxLength: 25),
                        CreatedAt = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        ServerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Servers", t => t.ServerId)
                .Index(t => t.Email, unique: true)
                .Index(t => t.Prefix, unique: true)
                .Index(t => t.ServerId);
            
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 25),
                        Password = c.String(),
                        Status = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        AccountId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.AccountId)
                .Index(t => new { t.Username, t.AccountId }, unique: true, name: "IX_UsernameAccount");
            
            CreateTable(
                "dbo.Servers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ip = c.String(nullable: false, maxLength: 15),
                        AcceptsRegistrations = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Ip, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Accounts", "ServerId", "dbo.Servers");
            DropForeignKey("dbo.Devices", "AccountId", "dbo.Accounts");
            DropIndex("dbo.Servers", new[] { "Ip" });
            DropIndex("dbo.Devices", "IX_UsernameAccount");
            DropIndex("dbo.Accounts", new[] { "ServerId" });
            DropIndex("dbo.Accounts", new[] { "Prefix" });
            DropIndex("dbo.Accounts", new[] { "Email" });
            DropTable("dbo.Servers");
            DropTable("dbo.Devices");
            DropTable("dbo.Accounts");
        }
    }
}
