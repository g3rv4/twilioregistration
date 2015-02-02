namespace TwilioRegistration.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Roles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CodeName = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.CodeName, unique: true);
            
            CreateTable(
                "dbo.RolesPermissions",
                c => new
                    {
                        RoleId = c.Int(nullable: false),
                        PermissionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.PermissionId })
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .ForeignKey("dbo.Permissions", t => t.PermissionId)
                .Index(t => t.RoleId)
                .Index(t => t.PermissionId);
            
            CreateTable(
                "dbo.AccountsRoles",
                c => new
                    {
                        AccountId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AccountId, t.RoleId })
                .ForeignKey("dbo.Accounts", t => t.AccountId)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.AccountId)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccountsRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.AccountsRoles", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.RolesPermissions", "PermissionId", "dbo.Permissions");
            DropForeignKey("dbo.RolesPermissions", "RoleId", "dbo.Roles");
            DropIndex("dbo.AccountsRoles", new[] { "RoleId" });
            DropIndex("dbo.AccountsRoles", new[] { "AccountId" });
            DropIndex("dbo.RolesPermissions", new[] { "PermissionId" });
            DropIndex("dbo.RolesPermissions", new[] { "RoleId" });
            DropIndex("dbo.Permissions", new[] { "CodeName" });
            DropTable("dbo.AccountsRoles");
            DropTable("dbo.RolesPermissions");
            DropTable("dbo.Permissions");
            DropTable("dbo.Roles");
        }
    }
}
