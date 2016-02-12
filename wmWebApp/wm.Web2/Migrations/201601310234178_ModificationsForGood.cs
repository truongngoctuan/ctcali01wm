namespace wm.Web2.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ModificationsForGood : DbMigration
    {
        public override void Up()
        {
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
                "dbo.Goods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        NameASCII = c.String(),
                        UnitId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GoodUnits", t => t.UnitId, cascadeDelete: true)
                .Index(t => t.UnitId);
            
            CreateTable(
                "dbo.GoodUnits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GoodCategoryGoods", "GoodCategoryId", "dbo.GoodCategories");
            DropForeignKey("dbo.GoodCategoryGoods", "GoodId", "dbo.Goods");
            DropForeignKey("dbo.Goods", "UnitId", "dbo.GoodUnits");
            DropIndex("dbo.Goods", new[] { "UnitId" });
            DropIndex("dbo.GoodCategoryGoods", new[] { "GoodId" });
            DropIndex("dbo.GoodCategoryGoods", new[] { "GoodCategoryId" });
            DropTable("dbo.GoodUnits");
            DropTable("dbo.Goods");
            DropTable("dbo.GoodCategoryGoods");
        }
    }
}
