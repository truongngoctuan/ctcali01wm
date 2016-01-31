namespace wm.Web2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrder : DbMigration
    {
        public override void Up()
        {
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
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BranchId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        OrderDay = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderGoods", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.OrderGoods", "GoodId", "dbo.Goods");
            DropIndex("dbo.OrderGoods", new[] { "GoodId" });
            DropIndex("dbo.OrderGoods", new[] { "OrderId" });
            DropTable("dbo.Orders");
            DropTable("dbo.OrderGoods");
        }
    }
}
