using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace wmWebApp.Models
{
    public class WorldContextInitializer : DropCreateDatabaseAlways<WorldContext>
    {
        protected override void Seed(WorldContext context)
        {

            if (context.Agencies.Any())
            {
                List<Agency> agencies = new List<Agency>()
                {
                    new Agency() { Name = "32 NT", Ranking = 1, Type = AgencyType.CHI_NHANH_FULL },
                    new Agency() { Name = "449 VVT", Ranking = 2, Type = AgencyType.CHI_NHANH_FULL }
                };
                context.Agencies.AddRange(agencies);
                context.SaveChanges();
            }
            base.Seed(context);
        }
    }

}