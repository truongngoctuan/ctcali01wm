namespace wm.Web.Migrations.ApplicationDbContext
{
    using Core.Models;
    using CsvHelper;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    internal sealed class Configuration : DbMigrationsConfiguration<wm.Web.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            MigrationsDirectory = @"Migrations\ApplicationDbContext";
        }

        protected override void Seed(wm.Web.Models.ApplicationDbContext context)
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

            //Assembly assembly = Assembly.GetExecutingAssembly();
            //string resourceName = "wm.Web.Migrations.SeedData.Branches.csv";
            //using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            //{
            //    using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8))
            //    {
            //        CsvReader csvReader = new CsvReader(reader);
            //        csvReader.Configuration.WillThrowOnMissingField = false;
            //        var branches = csvReader.GetRecords<Branch>().ToArray();
            //        context.Branches.AddOrUpdate(branches);
            //    }
            //}

            SeedApplicationUser(context, "ApplicationUsers");

        }

        private static void SeedApplicationUser(Models.ApplicationDbContext context, string fileName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "wm.Web.Migrations.SeedData." + fileName + ".csv";
            var store = new UserStore<ApplicationUser>(context);
            var userManager = new ApplicationUserManager(store);
            Console.WriteLine("testing console");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    csvReader.Configuration.WillThrowOnMissingField = false;

                    while (csvReader.Read())
                    {
                        //var provinceState = csvReader.GetRecord<ProvinceState>();
                        var UserName = csvReader.GetField<string>("UserName");
                        var LastName = csvReader.GetField<string>("LastName");
                        var FirstName = csvReader.GetField<string>("FirstName");
                        var FullName = LastName + " " + FirstName;
                        var FullNameANSCII = FullName.RemoveSign4VietnameseString();
                        var Email = csvReader.GetField<string>("Email");
                        var user = new ApplicationUser
                        {
                            UserName = UserName,
                            LastName = LastName,
                            FirstName = FirstName,
                            FullName = FullName,
                            FullNameANSCII = FullNameANSCII,
                            Email = Email
                        };
                        var result = userManager.CreateAsync(user, UserName);
                        if (result.Result.Succeeded)
                        {
                            Console.WriteLine("created 1 user");
                        }
                    }
                }
            }
        }
    }
}
