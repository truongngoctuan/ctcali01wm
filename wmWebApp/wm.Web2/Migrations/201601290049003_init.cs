namespace wm.Web2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BranchGoodCategories", "GoodCategoryId", "dbo.GoodCategories");
            DropForeignKey("dbo.BranchGoodCategories", "BranchId", "dbo.Branches");
            DropIndex("dbo.BranchGoodCategories", new[] { "GoodCategoryId" });
            DropIndex("dbo.BranchGoodCategories", new[] { "BranchId" });
            DropTable("dbo.Items");
            DropTable("dbo.GoodCategories");
            DropTable("dbo.BranchGoodCategories");
            DropTable("dbo.Branches");
        }
    }
}
