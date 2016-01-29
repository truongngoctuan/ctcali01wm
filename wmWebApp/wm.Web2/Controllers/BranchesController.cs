using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using wm.Model;
using wm.Web2.Models;

namespace wm.Web2.Controllers
{
    public class OrderItemViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public uint Quantity { get; set; }
        public uint RecommendedQuantity { get; set; }
    }

    public class PlacingOrderViewModel
    {
        public OrderItemViewModel[] data { get; set; }
    }

    //many-to-many with addition informations, using checkboxes
    //http://www.asp.net/mvc/overview/getting-started/getting-started-with-ef-using-mvc/updating-related-data-with-the-entity-framework-in-an-asp-net-mvc-application
    public class BranchesController : Controller
    {
        private wmContext db = new wmContext();

        // GET: Branches
        public ActionResult Index()
        {
            return View(db.Branches.ToList());
        }

        // GET: PlacingOrder
        [HttpPost]
        [AllowAnonymous]
        public ActionResult CreateEditBranchGoodCategory()
        {
            IEnumerable<BranchGoodCategoryViewModel> Items = new List<BranchGoodCategoryViewModel>()
            {
                new BranchGoodCategoryViewModel { CategoryId = 1, Name = "Item 1", IsChecked = true, Ranking = 1},
                new BranchGoodCategoryViewModel { CategoryId = 2, Name = "Item 2", IsChecked = true, Ranking = 2},
                new BranchGoodCategoryViewModel { CategoryId = 3, Name = "Item 3", IsChecked = false, Ranking = 3},
                new BranchGoodCategoryViewModel { CategoryId = 4, Name = "Item 4", IsChecked = true, Ranking = 4},
                new BranchGoodCategoryViewModel { CategoryId = 5, Name = "Item 5", IsChecked = false, Ranking = 5}
            };
            return Json(Items);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult PlacingOrder(PlacingOrderViewModel model)
        {
            return Json(model);
        }

        // GET: Branches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Branch branch = db.Branches.Find(id);
            if (branch == null)
            {
                return HttpNotFound();
            }
            return View(branch);
        }

        // GET: Branches/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Branches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Branch branch)
        {
            if (ModelState.IsValid)
            {
                db.Branches.Add(branch);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(branch);
        }

        // GET: Branches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Branch branch = db.Branches.Find(id);
            if (branch == null)
            {
                return HttpNotFound();
            }

            PopulateBranchGoodCategoryData(null);
            return View(branch);
        }

        private void PopulateBranchGoodCategoryData(Branch filterOwner)
        {
            var allList = db.GoodCategories;
            var filterId = new HashSet<int>();
            if (filterOwner != null)
            {
                filterId = new HashSet<int>(filterOwner.BranchGoodCategories.Select(g => g.GoodCategoryId));
            }
            
            var viewModel = new List<BranchGoodCategoryViewModel>();
            foreach (var item in allList)
            {
                viewModel.Add(new BranchGoodCategoryViewModel
                {
                    CategoryId = item.Id,
                    Name = item.Name,
                    IsChecked = filterId.Contains(item.Id)
                });
            }
            ViewBag.BranchGoodCategories = viewModel;
        }

        // POST: Branches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Branch branch)
        {
            if (ModelState.IsValid)
            {
                db.Entry(branch).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(branch);
        }

        // GET: Branches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Branch branch = db.Branches.Find(id);
            if (branch == null)
            {
                return HttpNotFound();
            }
            return View(branch);
        }

        // POST: Branches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Branch branch = db.Branches.Find(id);
            db.Branches.Remove(branch);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
