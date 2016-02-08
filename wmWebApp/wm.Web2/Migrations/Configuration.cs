namespace wm.Web2.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Model;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Host.SystemWeb;
    using System.Net.Http;
    internal sealed class Configuration : DbMigrationsConfiguration<Model.wmContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected void AddRoleIfNotExist(string roleName)
        {
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new wmContext()));

            if (!rm.RoleExists(roleName))
            {
                var roleResult = rm.Create(new IdentityRole(roleName));
                if (!roleResult.Succeeded)
                    throw new ApplicationException("Creating role " + roleName + "failed with error(s): " + roleResult.Errors);
            }

        }

        protected void CreateUser(Model.wmContext context, int id, RegisterViewModel model, string systemRole)
        {
            var user = new Model.ApplicationUser { UserName = model.UserName, Email = model.Email };
            ApplicationUserManager userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var existingUser = userManager.FindByName(model.UserName);
            if (existingUser != null)
            {
                userManager.RemovePassword(existingUser.Id);
                userManager.AddPassword(existingUser.Id, model.PlainPassword);

                userManager.AddToRole(existingUser.Id, systemRole);

                var existingEmployee = context.Employees.Where(s => s.ApplicationUserId == existingUser.Id).First();
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
                    var IndatabaseUser = userManager.FindByName(user.UserName);

                    userManager.AddToRole(IndatabaseUser.Id, systemRole);

                    var employee = new Employee
                    {
                        PlainPassword = model.PlainPassword, //when user change password, this will be reset
                        Id = id,
                        ApplicationUserId = IndatabaseUser.Id,
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
        protected override void Seed(Model.wmContext context)
        {
            ////for debugger
            //if (System.Diagnostics.Debugger.IsAttached == false)
            //        System.Diagnostics.Debugger.Launch();

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

            context.Branches.AddOrUpdate(new Model.Branch { Id = 1, Name = "branch 1", BranchType = BranchType.Normal });
            context.Branches.AddOrUpdate(new Model.Branch { Id = 2, Name = "branch 2", BranchType = BranchType.Normal });
            context.Branches.AddOrUpdate(new Model.Branch { Id = 3, Name = "branch 3", BranchType = BranchType.Normal });
            context.Branches.AddOrUpdate(new Model.Branch { Id = 4, Name = "kitchen", BranchType = BranchType.MainKitchen });
            context.Branches.AddOrUpdate(new Model.Branch { Id = 5, Name = "warehouse", BranchType = BranchType.MainWarehouse });

            context.GoodCategories.AddOrUpdate(new Model.GoodCategory { Id = 1, Name = "category 1" });
            context.GoodCategories.AddOrUpdate(new Model.GoodCategory { Id = 2, Name = "category 2" });
            context.GoodCategories.AddOrUpdate(new Model.GoodCategory { Id = 3, Name = "category 3" });
            context.GoodCategories.AddOrUpdate(new Model.GoodCategory { Id = 4, Name = "category 4" });

            context.SaveChanges();

            context.BranchGoodCategories.AddOrUpdate(new Model.BranchGoodCategory { BranchId = 1, GoodCategoryId = 1, Ranking = 0 });
            context.BranchGoodCategories.AddOrUpdate(new Model.BranchGoodCategory { BranchId = 1, GoodCategoryId = 2, Ranking = 1 });
            context.BranchGoodCategories.AddOrUpdate(new Model.BranchGoodCategory { BranchId = 1, GoodCategoryId = 3, Ranking = 2 });
            context.BranchGoodCategories.AddOrUpdate(new Model.BranchGoodCategory { BranchId = 2, GoodCategoryId = 2, Ranking = 0 });
            context.BranchGoodCategories.AddOrUpdate(new Model.BranchGoodCategory { BranchId = 2, GoodCategoryId = 3, Ranking = 1 });
            context.SaveChanges();

            context.GoodUnits.AddOrUpdate(new Model.GoodUnit { Id = 1, Name = "unit 1" });
            context.GoodUnits.AddOrUpdate(new Model.GoodUnit { Id = 2, Name = "unit 2" });
            context.GoodUnits.AddOrUpdate(new Model.GoodUnit { Id = 3, Name = "unit 3" });
            context.SaveChanges();

            context.Goods.AddOrUpdate(new Model.Good { Id = 1, Name = "good 1", UnitId = 1, GoodType = GoodType.Normal });
            context.Goods.AddOrUpdate(new Model.Good { Id = 2, Name = "good 2", UnitId = 2, GoodType = GoodType.Normal });
            context.Goods.AddOrUpdate(new Model.Good { Id = 3, Name = "good 3", UnitId = 1, GoodType = GoodType.Normal });
            context.Goods.AddOrUpdate(new Model.Good { Id = 4, Name = "good 4", UnitId = 3, GoodType = GoodType.Normal });
            context.Goods.AddOrUpdate(new Model.Good { Id = 5, Name = "good 5", UnitId = 1, GoodType = GoodType.Normal });
            context.Goods.AddOrUpdate(new Model.Good { Id = 6, Name = "good 6", UnitId = 1, GoodType = GoodType.Normal });
            context.Goods.AddOrUpdate(new Model.Good { Id = 7, Name = "good 7", UnitId = 2, GoodType = GoodType.Normal });
            context.Goods.AddOrUpdate(new Model.Good { Id = 8, Name = "good 8", UnitId = 2, GoodType = GoodType.Normal });

            context.Goods.AddOrUpdate(new Model.Good { Id = 9, Name = "raw 1", UnitId = 2, GoodType = GoodType.RawKitChenGood });
            context.Goods.AddOrUpdate(new Model.Good { Id = 10, Name = "raw 2", UnitId = 2, GoodType = GoodType.RawKitChenGood });
            context.Goods.AddOrUpdate(new Model.Good { Id = 11, Name = "raw 3", UnitId = 1, GoodType = GoodType.RawKitChenGood });
            context.Goods.AddOrUpdate(new Model.Good { Id = 12, Name = "raw 4", UnitId = 3, GoodType = GoodType.RawKitChenGood });

            context.Goods.AddOrUpdate(new Model.Good { Id = 13, Name = "KitChenGood 1", UnitId = 2, GoodType = GoodType.KitChenGood });
            context.Goods.AddOrUpdate(new Model.Good { Id = 14, Name = "KitChenGood 2", UnitId = 3, GoodType = GoodType.KitChenGood });
            context.Goods.AddOrUpdate(new Model.Good { Id = 15, Name = "KitChenGood 3", UnitId = 1, GoodType = GoodType.KitChenGood });
            context.Goods.AddOrUpdate(new Model.Good { Id = 16, Name = "KitChenGood 4", UnitId = 2, GoodType = GoodType.KitChenGood });
            context.SaveChanges();

            context.GoodCategoryGoods.AddOrUpdate(new Model.GoodCategoryGood { GoodCategoryId = 1, GoodId = 1, Ranking = 0 });
            context.GoodCategoryGoods.AddOrUpdate(new Model.GoodCategoryGood { GoodCategoryId = 1, GoodId = 2, Ranking = 1 });
            context.GoodCategoryGoods.AddOrUpdate(new Model.GoodCategoryGood { GoodCategoryId = 1, GoodId = 3, Ranking = 2 });
            context.GoodCategoryGoods.AddOrUpdate(new Model.GoodCategoryGood { GoodCategoryId = 2, GoodId = 4, Ranking = 0 });
            context.GoodCategoryGoods.AddOrUpdate(new Model.GoodCategoryGood { GoodCategoryId = 2, GoodId = 5, Ranking = 1 });
            context.GoodCategoryGoods.AddOrUpdate(new Model.GoodCategoryGood { GoodCategoryId = 3, GoodId = 6, Ranking = 0 });
            context.GoodCategoryGoods.AddOrUpdate(new Model.GoodCategoryGood { GoodCategoryId = 3, GoodId = 7, Ranking = 1 });
            context.GoodCategoryGoods.AddOrUpdate(new Model.GoodCategoryGood { GoodCategoryId = 3, GoodId = 8, Ranking = 2 });
            context.SaveChanges();

            context.Orders.AddOrUpdate(new Model.Order { Id = 1, BranchId = 1, CreatedDate = DateTime.UtcNow, OrderDay = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, Status = Model.OrderStatus.Started });
            context.Orders.AddOrUpdate(new Model.Order { Id = 2, BranchId = 2, CreatedDate = DateTime.UtcNow, OrderDay = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, Status = Model.OrderStatus.Started });
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
            #endregion

        }
    }
}
