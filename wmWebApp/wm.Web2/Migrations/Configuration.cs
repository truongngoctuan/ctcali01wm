namespace wm.Web2.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Model.wmContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Model.wmContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.Branches.AddOrUpdate(new Model.Branch { Id = 1, Name = "branch 1" });
            context.Branches.AddOrUpdate(new Model.Branch { Id = 2, Name = "branch 2" });
            context.Branches.AddOrUpdate(new Model.Branch { Id = 3, Name = "branch 3" });
            context.Branches.AddOrUpdate(new Model.Branch { Id = 4, Name = "branch 4" });
            context.Branches.AddOrUpdate(new Model.Branch { Id = 5, Name = "branch 5" });

            context.GoodCategories.AddOrUpdate(new Model.GoodCategory { Id = 1, Name = "category 1" });
            context.GoodCategories.AddOrUpdate(new Model.GoodCategory { Id = 2, Name = "category 2" });
            context.GoodCategories.AddOrUpdate(new Model.GoodCategory { Id = 3, Name = "category 3" });
            context.GoodCategories.AddOrUpdate(new Model.GoodCategory { Id = 4, Name = "category 4" });

            context.BranchGoodCategories.AddOrUpdate(new Model.BranchGoodCategory { BranchId = 1, GoodCategoryId = 1, Ranking = 0 });
            context.BranchGoodCategories.AddOrUpdate(new Model.BranchGoodCategory { BranchId = 1, GoodCategoryId = 2, Ranking = 1 });
            context.BranchGoodCategories.AddOrUpdate(new Model.BranchGoodCategory { BranchId = 1, GoodCategoryId = 3, Ranking = 2 });
            context.BranchGoodCategories.AddOrUpdate(new Model.BranchGoodCategory { BranchId = 2, GoodCategoryId = 2, Ranking = 0 });
            context.BranchGoodCategories.AddOrUpdate(new Model.BranchGoodCategory { BranchId = 2, GoodCategoryId = 3, Ranking = 1 });

            context.GoodUnits.AddOrUpdate(new Model.GoodUnit { Id = 1, Name = "unit 1" });
            context.GoodUnits.AddOrUpdate(new Model.GoodUnit { Id = 2, Name = "unit 2" });
            context.GoodUnits.AddOrUpdate(new Model.GoodUnit { Id = 3, Name = "unit 3" });

            context.Goods.AddOrUpdate(new Model.Good { Id = 1, Name = "good 1", UnitId = 1});
            context.Goods.AddOrUpdate(new Model.Good { Id = 2, Name = "good 2", UnitId = 2 });
            context.Goods.AddOrUpdate(new Model.Good { Id = 3, Name = "good 3", UnitId = 1 });
            context.Goods.AddOrUpdate(new Model.Good { Id = 4, Name = "good 4", UnitId = 3 });
            context.Goods.AddOrUpdate(new Model.Good { Id = 5, Name = "good 5", UnitId = 1 });
            context.Goods.AddOrUpdate(new Model.Good { Id = 6, Name = "good 6", UnitId = 1 });
            context.Goods.AddOrUpdate(new Model.Good { Id = 7, Name = "good 7", UnitId = 2 });
            context.Goods.AddOrUpdate(new Model.Good { Id = 8, Name = "good 8", UnitId = 2 });

            context.GoodCategoryGoods.AddOrUpdate(new Model.GoodCategoryGood { GoodCategoryId = 1, GoodId = 1, Ranking = 0 });
            context.GoodCategoryGoods.AddOrUpdate(new Model.GoodCategoryGood { GoodCategoryId = 1, GoodId = 2, Ranking = 1 });
            context.GoodCategoryGoods.AddOrUpdate(new Model.GoodCategoryGood { GoodCategoryId = 1, GoodId = 3, Ranking = 2 });
            context.GoodCategoryGoods.AddOrUpdate(new Model.GoodCategoryGood { GoodCategoryId = 2, GoodId = 4, Ranking = 0 });
            context.GoodCategoryGoods.AddOrUpdate(new Model.GoodCategoryGood { GoodCategoryId = 2, GoodId = 5, Ranking = 1 });
            context.GoodCategoryGoods.AddOrUpdate(new Model.GoodCategoryGood { GoodCategoryId = 3, GoodId = 6, Ranking = 0 });
            context.GoodCategoryGoods.AddOrUpdate(new Model.GoodCategoryGood { GoodCategoryId = 3, GoodId = 7, Ranking = 1 });
            context.GoodCategoryGoods.AddOrUpdate(new Model.GoodCategoryGood { GoodCategoryId = 3, GoodId = 8, Ranking = 2 });

            context.Orders.AddOrUpdate(new Model.Order { Id = 1, BranchId = 1, CreatedDate = DateTime.UtcNow, OrderDay = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, Status = Model.OrderStatus.Started });
            context.Orders.AddOrUpdate(new Model.Order { Id = 2, BranchId = 2, CreatedDate = DateTime.UtcNow, OrderDay = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, Status = Model.OrderStatus.Started });
        }
    }
}
