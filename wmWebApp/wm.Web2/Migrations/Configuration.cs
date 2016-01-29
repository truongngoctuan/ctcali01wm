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

            context.GoodCategories.AddOrUpdate(new Model.GoodCategory { Name = "category 1" });
            context.GoodCategories.AddOrUpdate(new Model.GoodCategory { Name = "category 2" });
            context.GoodCategories.AddOrUpdate(new Model.GoodCategory { Name = "category 3" });
            context.GoodCategories.AddOrUpdate(new Model.GoodCategory { Name = "category 4" });
        }
    }
}
