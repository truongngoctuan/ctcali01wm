namespace wm.Web.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _01_UpdateApplicationUserRelation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MerchandiseCategoryBranchTypes", "MerchandiseCategory_Id", "dbo.MerchandiseCategories");
            DropForeignKey("dbo.MerchandiseCategoryBranchTypes", "BranchType_Id", "dbo.BranchTypes");
            DropIndex("dbo.MerchandiseCategoryBranchTypes", new[] { "MerchandiseCategory_Id" });
            DropIndex("dbo.MerchandiseCategoryBranchTypes", new[] { "BranchType_Id" });
            CreateTable(
                "dbo.Positions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "PositionId", c => c.Int());
            AddColumn("dbo.AspNetUsers", "BranchId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "PositionId");
            CreateIndex("dbo.AspNetUsers", "BranchId");
            AddForeignKey("dbo.AspNetUsers", "BranchId", "dbo.Branches", "Id");
            AddForeignKey("dbo.AspNetUsers", "PositionId", "dbo.Positions", "Id");
            DropTable("dbo.MerchandiseCategoryBranchTypes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.MerchandiseCategoryBranchTypes",
                c => new
                    {
                        MerchandiseCategory_Id = c.Int(nullable: false),
                        BranchType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MerchandiseCategory_Id, t.BranchType_Id });
            
            DropForeignKey("dbo.AspNetUsers", "PositionId", "dbo.Positions");
            DropForeignKey("dbo.AspNetUsers", "BranchId", "dbo.Branches");
            DropIndex("dbo.AspNetUsers", new[] { "BranchId" });
            DropIndex("dbo.AspNetUsers", new[] { "PositionId" });
            DropColumn("dbo.AspNetUsers", "BranchId");
            DropColumn("dbo.AspNetUsers", "PositionId");
            DropTable("dbo.Positions");
            CreateIndex("dbo.MerchandiseCategoryBranchTypes", "BranchType_Id");
            CreateIndex("dbo.MerchandiseCategoryBranchTypes", "MerchandiseCategory_Id");
            AddForeignKey("dbo.MerchandiseCategoryBranchTypes", "BranchType_Id", "dbo.BranchTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.MerchandiseCategoryBranchTypes", "MerchandiseCategory_Id", "dbo.MerchandiseCategories", "Id", cascadeDelete: true);
        }
    }
}
