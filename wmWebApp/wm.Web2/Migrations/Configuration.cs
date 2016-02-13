namespace wm.Web2.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Model;
    using Models;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<wmContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        private void AddRoleIfNotExist(string roleName)
        {
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new wmContext()));

            if (!rm.RoleExists(roleName))
            {
                var roleResult = rm.Create(new IdentityRole(roleName));
                if (!roleResult.Succeeded)
                    throw new ApplicationException("Creating role " + roleName + "failed with error(s): " + roleResult.Errors);
            }

        }

        private void CreateUser(wmContext context, int id, RegisterViewModel model, string systemRole)
        {
            var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
            ApplicationUserManager userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var existingUser = userManager.FindByName(model.UserName);
            if (existingUser != null)
            {
                userManager.RemovePassword(existingUser.Id);
                userManager.AddPassword(existingUser.Id, model.PlainPassword);

                userManager.AddToRole(existingUser.Id, systemRole);

                var existingEmployee = context.Employees.First(s => s.ApplicationUserId == existingUser.Id);
                existingEmployee.PlainPassword = model.PlainPassword;
                existingEmployee.UserName = user.UserName;
                existingEmployee.Name = model.FullName;
                existingEmployee.BranchId = model.BranchId;
                existingEmployee.Role = model.Role;

                context.Employees.AddOrUpdate(existingEmployee);
                context.SaveChanges();
            }
            else
            {
                var result = userManager.Create(user, model.PlainPassword);

                if (result.Succeeded)
                {
                    var indatabaseUser = userManager.FindByName(user.UserName);

                    userManager.AddToRole(indatabaseUser.Id, systemRole);

                    var employee = new Employee
                    {
                        PlainPassword = model.PlainPassword, //when user change password, this will be reset
                        Id = id,
                        ApplicationUserId = indatabaseUser.Id,
                        UserName = user.UserName,
                        Name = model.FullName,
                        BranchId = model.BranchId,
                        Role = model.Role
                    };
                    context.Employees.AddOrUpdate(employee);
                    context.SaveChanges();
                }
            }


        }
        protected override void Seed(wmContext context)
        {
            ////for debugger
            //if (System.Diagnostics.Debugger.IsAttached == false)
            //        System.Diagnostics.Debugger.Launch();

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed Data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Branches.AddOrUpdate(new Branch { Id = 1, Name = "branch 1", BranchType = BranchType.Normal });
            context.Branches.AddOrUpdate(new Branch { Id = 2, Name = "branch 2", BranchType = BranchType.Normal });
            context.Branches.AddOrUpdate(new Branch { Id = 3, Name = "branch 3", BranchType = BranchType.Normal });
            context.Branches.AddOrUpdate(new Branch { Id = 4, Name = "kitchen", BranchType = BranchType.MainKitchen });
            context.Branches.AddOrUpdate(new Branch { Id = 5, Name = "warehouse", BranchType = BranchType.MainWarehouse });

            context.GoodCategories.AddOrUpdate(new GoodCategory { Id = 1, Name = "category 1" });
            context.GoodCategories.AddOrUpdate(new GoodCategory { Id = 2, Name = "category 2" });
            context.GoodCategories.AddOrUpdate(new GoodCategory { Id = 3, Name = "category 3" });
            context.GoodCategories.AddOrUpdate(new GoodCategory { Id = 4, Name = "category 4" });

            context.SaveChanges();

            context.BranchGoodCategories.AddOrUpdate(new BranchGoodCategory { BranchId = 1, GoodCategoryId = 1, Ranking = 0 });
            context.BranchGoodCategories.AddOrUpdate(new BranchGoodCategory { BranchId = 1, GoodCategoryId = 2, Ranking = 1 });
            context.BranchGoodCategories.AddOrUpdate(new BranchGoodCategory { BranchId = 1, GoodCategoryId = 3, Ranking = 2 });
            context.BranchGoodCategories.AddOrUpdate(new BranchGoodCategory { BranchId = 2, GoodCategoryId = 2, Ranking = 0 });
            context.BranchGoodCategories.AddOrUpdate(new BranchGoodCategory { BranchId = 2, GoodCategoryId = 3, Ranking = 1 });
            context.BranchGoodCategories.AddOrUpdate(new BranchGoodCategory { BranchId = 3, GoodCategoryId = 1, Ranking = 0 });
            context.BranchGoodCategories.AddOrUpdate(new BranchGoodCategory { BranchId = 4, GoodCategoryId = 1, Ranking = 0 });
            context.BranchGoodCategories.AddOrUpdate(new BranchGoodCategory { BranchId = 4, GoodCategoryId = 2, Ranking = 1 });
            context.BranchGoodCategories.AddOrUpdate(new BranchGoodCategory { BranchId = 4, GoodCategoryId = 3, Ranking = 2 });
            context.SaveChanges();

            context.GoodUnits.AddOrUpdate(new GoodUnit { Id = 1, Name = "unit 1" });
            context.GoodUnits.AddOrUpdate(new GoodUnit { Id = 2, Name = "unit 2" });
            context.GoodUnits.AddOrUpdate(new GoodUnit { Id = 3, Name = "unit 3" });
            context.SaveChanges();

            context.Goods.AddOrUpdate(new Good { Id = 1, Name = "good 1", UnitId = 1, GoodType = GoodType.Normal });
            context.Goods.AddOrUpdate(new Good { Id = 2, Name = "good 2", UnitId = 2, GoodType = GoodType.Normal });
            context.Goods.AddOrUpdate(new Good { Id = 3, Name = "good 3", UnitId = 1, GoodType = GoodType.Normal });
            context.Goods.AddOrUpdate(new Good { Id = 4, Name = "good 4", UnitId = 3, GoodType = GoodType.Normal });
            context.Goods.AddOrUpdate(new Good { Id = 5, Name = "good 5", UnitId = 1, GoodType = GoodType.Normal });
            context.Goods.AddOrUpdate(new Good { Id = 6, Name = "good 6", UnitId = 1, GoodType = GoodType.Normal });
            context.Goods.AddOrUpdate(new Good { Id = 7, Name = "good 7", UnitId = 2, GoodType = GoodType.Normal });
            context.Goods.AddOrUpdate(new Good { Id = 8, Name = "good 8", UnitId = 2, GoodType = GoodType.Normal });

            context.Goods.AddOrUpdate(new Good { Id = 9, Name = "raw 1", UnitId = 2, GoodType = GoodType.RawKitChenGood });
            context.Goods.AddOrUpdate(new Good { Id = 10, Name = "raw 2", UnitId = 2, GoodType = GoodType.RawKitChenGood });
            context.Goods.AddOrUpdate(new Good { Id = 11, Name = "raw 3", UnitId = 1, GoodType = GoodType.RawKitChenGood });
            context.Goods.AddOrUpdate(new Good { Id = 12, Name = "raw 4", UnitId = 3, GoodType = GoodType.RawKitChenGood });

            context.Goods.AddOrUpdate(new Good { Id = 13, Name = "KitChenGood 1", UnitId = 2, GoodType = GoodType.KitChenGood });
            context.Goods.AddOrUpdate(new Good { Id = 14, Name = "KitChenGood 2", UnitId = 3, GoodType = GoodType.KitChenGood });
            context.Goods.AddOrUpdate(new Good { Id = 15, Name = "KitChenGood 3", UnitId = 1, GoodType = GoodType.KitChenGood });
            context.Goods.AddOrUpdate(new Good { Id = 16, Name = "KitChenGood 4", UnitId = 2, GoodType = GoodType.KitChenGood });
            context.SaveChanges();

            context.GoodCategoryGoods.AddOrUpdate(new GoodCategoryGood { GoodCategoryId = 1, GoodId = 1, Ranking = 0 });
            context.GoodCategoryGoods.AddOrUpdate(new GoodCategoryGood { GoodCategoryId = 1, GoodId = 2, Ranking = 1 });
            context.GoodCategoryGoods.AddOrUpdate(new GoodCategoryGood { GoodCategoryId = 1, GoodId = 3, Ranking = 2 });
            context.GoodCategoryGoods.AddOrUpdate(new GoodCategoryGood { GoodCategoryId = 2, GoodId = 4, Ranking = 0 });
            context.GoodCategoryGoods.AddOrUpdate(new GoodCategoryGood { GoodCategoryId = 2, GoodId = 5, Ranking = 1 });
            context.GoodCategoryGoods.AddOrUpdate(new GoodCategoryGood { GoodCategoryId = 3, GoodId = 6, Ranking = 0 });
            context.GoodCategoryGoods.AddOrUpdate(new GoodCategoryGood { GoodCategoryId = 3, GoodId = 7, Ranking = 1 });
            context.GoodCategoryGoods.AddOrUpdate(new GoodCategoryGood { GoodCategoryId = 3, GoodId = 8, Ranking = 2 });
            //kitchen good
            context.GoodCategoryGoods.AddOrUpdate(new GoodCategoryGood { GoodCategoryId = 1, GoodId = 13, Ranking = 3 });
            context.GoodCategoryGoods.AddOrUpdate(new GoodCategoryGood { GoodCategoryId = 1, GoodId = 14, Ranking = 4 });
            context.GoodCategoryGoods.AddOrUpdate(new GoodCategoryGood { GoodCategoryId = 1, GoodId = 15, Ranking = 5 });
            context.SaveChanges();

            context.Orders.AddOrUpdate(new Order { Id = 1, BranchId = 1, CreatedDate = DateTime.UtcNow, OrderDay = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, Status = OrderStatus.Started });
            context.Orders.AddOrUpdate(new Order { Id = 2, BranchId = 2, CreatedDate = DateTime.UtcNow, OrderDay = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, Status = OrderStatus.Started });
            context.SaveChanges();


            #region roles and pre define users
            AddRoleIfNotExist(SystemRoles.SuperUser);
            AddRoleIfNotExist(SystemRoles.Admin);
            AddRoleIfNotExist(SystemRoles.WarehouseKeeper);
            AddRoleIfNotExist(SystemRoles.Manager);
            AddRoleIfNotExist(SystemRoles.Staff);

            CreateUser(context, 1, new RegisterViewModel { UserName = "staff", Email = "tntuan0712494@gmail.com", FullName = "Staff name", BranchId = 1, PlainPassword = "asdasd", Role = EmployeeRole.StaffBranch }, SystemRoles.Staff);
            CreateUser(context, 2, new RegisterViewModel { UserName = "admin", Email = "tntuan0712494@gmail.com", FullName = "TNT", BranchId = 1, PlainPassword = "asdasd", Role = EmployeeRole.Admin }, SystemRoles.Admin);
            CreateUser(context, 3, new RegisterViewModel { UserName = "TNT", Email = "tntuan0712494@gmail.com", FullName = "TNT", BranchId = 1, PlainPassword = "qwerty", Role = EmployeeRole.SuperUser }, SystemRoles.SuperUser);
            CreateUser(context, 4, new RegisterViewModel { UserName = "manager", Email = "tntuan0712494@gmail.com", FullName = "manager name", BranchId = 1, PlainPassword = "asdasd", Role = EmployeeRole.Manager }, SystemRoles.Manager);
            #endregion

        }
    }
}
