namespace wm.Web.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        Phone = c.String(),
                        BranchTypeId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BranchTypes", t => t.BranchTypeId)
                .Index(t => t.BranchTypeId);
            
            CreateTable(
                "dbo.BranchTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MerchandiseCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ParentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MerchandiseCategories", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.Merchandises",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameUnicode = c.String(),
                        NameAnsi = c.String(),
                        Measurement = c.String(),
                        MerchandiseType = c.Int(nullable: false),
                        MerchandiseCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MerchandiseCategories", t => t.MerchandiseCategoryId, cascadeDelete: true)
                .Index(t => t.MerchandiseCategoryId);
            
            CreateTable(
                "dbo.MerchandiseAccoutantHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameAccouting = c.String(),
                        TimeCreated = c.DateTime(nullable: false),
                        MerchandiseId = c.Int(nullable: false),
                        ApplicationUserCreatedId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserCreatedId)
                .ForeignKey("dbo.Merchandises", t => t.MerchandiseId, cascadeDelete: true)
                .Index(t => t.MerchandiseId)
                .Index(t => t.ApplicationUserCreatedId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Phone = c.String(),
                        Address = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.MerchandiseCategoryBranchTypes",
                c => new
                    {
                        MerchandiseCategory_Id = c.Int(nullable: false),
                        BranchType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MerchandiseCategory_Id, t.BranchType_Id })
                .ForeignKey("dbo.MerchandiseCategories", t => t.MerchandiseCategory_Id, cascadeDelete: true)
                .ForeignKey("dbo.BranchTypes", t => t.BranchType_Id, cascadeDelete: true)
                .Index(t => t.MerchandiseCategory_Id)
                .Index(t => t.BranchType_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Branches", "BranchTypeId", "dbo.BranchTypes");
            DropForeignKey("dbo.MerchandiseCategories", "ParentId", "dbo.MerchandiseCategories");
            DropForeignKey("dbo.Merchandises", "MerchandiseCategoryId", "dbo.MerchandiseCategories");
            DropForeignKey("dbo.MerchandiseAccoutantHistories", "MerchandiseId", "dbo.Merchandises");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.MerchandiseAccoutantHistories", "ApplicationUserCreatedId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.MerchandiseCategoryBranchTypes", "BranchType_Id", "dbo.BranchTypes");
            DropForeignKey("dbo.MerchandiseCategoryBranchTypes", "MerchandiseCategory_Id", "dbo.MerchandiseCategories");
            DropIndex("dbo.MerchandiseCategoryBranchTypes", new[] { "BranchType_Id" });
            DropIndex("dbo.MerchandiseCategoryBranchTypes", new[] { "MerchandiseCategory_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.MerchandiseAccoutantHistories", new[] { "ApplicationUserCreatedId" });
            DropIndex("dbo.MerchandiseAccoutantHistories", new[] { "MerchandiseId" });
            DropIndex("dbo.Merchandises", new[] { "MerchandiseCategoryId" });
            DropIndex("dbo.MerchandiseCategories", new[] { "ParentId" });
            DropIndex("dbo.Branches", new[] { "BranchTypeId" });
            DropTable("dbo.MerchandiseCategoryBranchTypes");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.MerchandiseAccoutantHistories");
            DropTable("dbo.Merchandises");
            DropTable("dbo.MerchandiseCategories");
            DropTable("dbo.BranchTypes");
            DropTable("dbo.Branches");
        }
    }
}
