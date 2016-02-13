using System.IO;
using System.Reflection;
using CsvHelper;

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

            SeedFromFile(context, "Branch", (wmContext, reader) =>
            {
                Branch branch = new Branch
                {
                    Id = reader.GetField<int>("Id"),
                    Name = reader.GetField<string>("Name"),
                    BranchType = (BranchType)reader.GetField<int>("BranchType")
                };

                context.Branches.AddOrUpdate(branch);
            });

            SeedFromFile(context, "GoodCategory", (wmContext, reader) =>
            {
                var newObject = new GoodCategory
                {
                    Id = reader.GetField<int>("Id"),
                    Name = reader.GetField<string>("Name")
                };

                context.GoodCategories.AddOrUpdate(newObject);
            });

            SeedFromFile(context, "BranchGoodCategory", (wmContext, reader) =>
            {
                var newObject = new BranchGoodCategory
                {
                    BranchId = reader.GetField<int>("BranchId"),
                    GoodCategoryId = reader.GetField<int>("GoodCategoryId"),
                    Ranking = reader.GetField<int>("Ranking")
                };

                context.BranchGoodCategories.AddOrUpdate(newObject);
            });

            SeedFromFile(context, "GoodUnit", (wmContext, reader) =>
            {
                var newObject = new GoodUnit
                {
                    Id = reader.GetField<int>("Id"),
                    Name = reader.GetField<string>("Name")
                };

                context.GoodUnits.AddOrUpdate(newObject);
            });

            SeedFromFile(context, "Good", (wmContext, reader) =>
            {
                var newObject = new Good
                {
                    Id = reader.GetField<int>("Id"),
                    Name = reader.GetField<string>("Name"),
                    UnitId = reader.GetField<int>("GoodUnit.Id"),
                    GoodType = (GoodType)reader.GetField<int>("GoodType")
                };

                context.Goods.AddOrUpdate(newObject);
            });

            SeedFromFile(context, "Good", (wmContext, reader) =>
            {
                var newObject = new GoodCategoryGood
                {
                    GoodCategoryId = reader.GetField<int>("GoodCaegory.Id"),
                    GoodId = reader.GetField<int>("Id"),
                    Ranking = reader.GetField<int>("Ranking")
                };

                context.GoodCategoryGoods.AddOrUpdate(newObject);
            });

            //context.Orders.AddOrUpdate(new Order { Id = 1, BranchId = 1, CreatedDate = DateTime.UtcNow, OrderDay = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, Status = OrderStatus.Started });
            //context.Orders.AddOrUpdate(new Order { Id = 2, BranchId = 2, CreatedDate = DateTime.UtcNow, OrderDay = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, Status = OrderStatus.Started });
            //context.SaveChanges();


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

        private void SeedFromFile(wmContext context, string fileName, Action<wmContext, CsvReader> doAction)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "wm.Web2.Migrations.SeedData." + fileName + ".csv";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    csvReader.Configuration.WillThrowOnMissingField = false;

                    while (csvReader.Read())
                    {
                        doAction(context, csvReader);
                        //var FullNameANSCII = FullName.RemoveSign4VietnameseString();
                    }
                }
            }

            context.SaveChanges();
        }
    }
}
