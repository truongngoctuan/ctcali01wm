namespace wm.Web2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitCreateWithSomeData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        BranchType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BranchGoodCategories",
                c => new
                    {
                        BranchId = c.Int(nullable: false),
                        GoodCategoryId = c.Int(nullable: false),
                        Ranking = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BranchId, t.GoodCategoryId })
                .ForeignKey("dbo.Branches", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("dbo.GoodCategories", t => t.GoodCategoryId, cascadeDelete: true)
                .Index(t => t.BranchId)
                .Index(t => t.GoodCategoryId);
            
            CreateTable(
                "dbo.GoodCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(),
                        UserName = c.String(),
                        Name = c.String(),
                        PlainPassword = c.String(),
                        Role = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branches", t => t.BranchId, cascadeDelete: true)
                .Index(t => t.BranchId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BranchId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        OrderDay = c.DateTime(nullable: false),
                        Priority = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branches", t => t.BranchId, cascadeDelete: true)
                .Index(t => t.BranchId);
            
            CreateTable(
                "dbo.OrderGoods",
                c => new
                    {
                        OrderId = c.Int(nullable: false),
                        GoodId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Note = c.String(),
                    })
                .PrimaryKey(t => new { t.OrderId, t.GoodId, t.UserId })
                .ForeignKey("dbo.Goods", t => t.GoodId, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.GoodId);
            
            CreateTable(
                "dbo.Goods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        NameASCII = c.String(),
                        UnitId = c.Int(nullable: false),
                        GoodType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GoodUnits", t => t.UnitId)
                .Index(t => t.UnitId);
            
            CreateTable(
                "dbo.GoodUnits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GoodCategoryGoods",
                c => new
                    {
                        GoodCategoryId = c.Int(nullable: false),
                        GoodId = c.Int(nullable: false),
                        Ranking = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.GoodCategoryId, t.GoodId })
                .ForeignKey("dbo.Goods", t => t.GoodId, cascadeDelete: true)
                .ForeignKey("dbo.GoodCategories", t => t.GoodCategoryId, cascadeDelete: true)
                .Index(t => t.GoodCategoryId)
                .Index(t => t.GoodId);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.GoodCategoryGoods", "GoodCategoryId", "dbo.GoodCategories");
            DropForeignKey("dbo.GoodCategoryGoods", "GoodId", "dbo.Goods");
            DropForeignKey("dbo.OrderGoods", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.OrderGoods", "GoodId", "dbo.Goods");
            DropForeignKey("dbo.Goods", "UnitId", "dbo.GoodUnits");
            DropForeignKey("dbo.Orders", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.Employees", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.BranchGoodCategories", "GoodCategoryId", "dbo.GoodCategories");
            DropForeignKey("dbo.BranchGoodCategories", "BranchId", "dbo.Branches");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.GoodCategoryGoods", new[] { "GoodId" });
            DropIndex("dbo.GoodCategoryGoods", new[] { "GoodCategoryId" });
            DropIndex("dbo.Goods", new[] { "UnitId" });
            DropIndex("dbo.OrderGoods", new[] { "GoodId" });
            DropIndex("dbo.OrderGoods", new[] { "OrderId" });
            DropIndex("dbo.Orders", new[] { "BranchId" });
            DropIndex("dbo.Employees", new[] { "BranchId" });
            DropIndex("dbo.BranchGoodCategories", new[] { "GoodCategoryId" });
            DropIndex("dbo.BranchGoodCategories", new[] { "BranchId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Items");
            DropTable("dbo.GoodCategoryGoods");
            DropTable("dbo.GoodUnits");
            DropTable("dbo.Goods");
            DropTable("dbo.OrderGoods");
            DropTable("dbo.Orders");
            DropTable("dbo.Employees");
            DropTable("dbo.GoodCategories");
            DropTable("dbo.BranchGoodCategories");
            DropTable("dbo.Branches");
        }
    }
}
