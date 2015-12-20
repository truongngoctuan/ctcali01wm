using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wmWebApp.Models;

namespace wmWebApp.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            //using (var ctx = new WorldContext())
            //{
            //    Agency agent = new Agency() { Name = "asd" };
            //    ctx.Agencies.Add(agent);
            //    ctx.SaveChanges();

            //}
            return View();
        }
    }
}