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
using wm.Service;
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
        public int Id { get; set; }
        public BranchGoodCategoryViewModel[] data { get; set; }
    }

    //many-to-many with addition informations, using checkboxes
    //http://www.asp.net/mvc/overview/getting-started/getting-started-with-ef-using-mvc/updating-related-data-with-the-entity-framework-in-an-asp-net-mvc-application
    public class BranchesController : BaseController
    {
        IBranchService _service;
        IBranchService Service { get { return _service; } }
        IGoodCategoryService _goodCategoryService;
        public BranchesController(IBranchService Service, IGoodCategoryService GoodCategoryService)
        {
            _service = Service;
            _goodCategoryService = GoodCategoryService;
        }
        // GET: Branches
        public ActionResult Index()
        {
            return View(Service.GetAll());
        }

        // GET: PlacingOrder
        [HttpPost]
        [AllowAnonymous]
        public ActionResult PopulateData(int? id)//, PlacingOrderViewModel nnData)
        {
            var model = (id == null) ? null : Service.GetById((int)id);
            var items = PopulateManyToManyData(model);
            return Json(items);
        }

        private IEnumerable<BranchGoodCategoryViewModel> PopulateManyToManyData(Branch filterOwner)
        {
            var allList = _goodCategoryService.GetAll();

            //get list of checked item
            var filterIdList = new HashSet<int>();
            if (filterOwner != null)
            {
                filterIdList = new HashSet<int>(filterOwner.BranchGoodCategories.Select(g => g.GoodCategoryId));
            }

            //binding data
            var viewModel = allList.Select(t => new BranchGoodCategoryViewModel
            {
                CategoryId = t.Id,
                Name = t.Name,
                IsChecked = filterIdList.Contains(t.Id)
            });

            return viewModel;
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult IncludeExcludeNNData(int id, PlacingOrderViewModel inputViewModel)
        {//only checkbox, not ranking
            var model = Service.GetById(id);
            var oldLinkingIdList = model.BranchGoodCategories.Select(t => t.GoodCategoryId);
            var newLinkingIdList = (inputViewModel.data == null) ? new List<int>() : inputViewModel.data.Select(t => t.CategoryId);

            var removeIds = oldLinkingIdList.Where(t => !newLinkingIdList.Contains(t)).ToList();
            var editIds = oldLinkingIdList.Where(t => newLinkingIdList.Contains(t));
            var newIds = newLinkingIdList.Where(t => !oldLinkingIdList.Contains(t));

            //remove
            foreach (var itemId in removeIds)
            {
                var removeObject = model.BranchGoodCategories.Where(t => t.GoodCategoryId == itemId).First();
                model.BranchGoodCategories.Remove(removeObject);
            }

            //edit - do nothing

            //add new item
            foreach (var itemId in newIds)
            {
                var newObject = new BranchGoodCategory
                {
                    Branch = model,
                    GoodCategory = _goodCategoryService.GetById(itemId)
                };

                model.BranchGoodCategories.Add(newObject);
            }

            Service.Update(model);

            return Json("ok");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ReOrderNNData(int id, PlacingOrderViewModel inputViewModel)
        {
            return View();
        }

        //[HttpPost]
        //[AllowAnonymous]
        //public ActionResult PlacingOrder(int id, PlacingOrderViewModel inputViewModel)
        //{
        //    var model = Service.GetById(id);
        //    var oldLinking = model.BranchGoodCategories;
        //    var newLinking = inputViewModel.data.Select(t => new BranchGoodCategory
        //    {
        //        Branch = Service.GetById(model.Id),
        //        GoodCategory = _goodCategoryService.GetById(t.CategoryId),
        //        Ranking = t.Ranking
        //    });

        //    foreach(var item in newLinking)
        //    {
        //        var match = oldLinking.Where(t => t.GoodCategoryId == item.GoodCategoryId).First();
        //        if(match == null)
        //        {
        //            //model.BranchGoodCategories.Add()
        //        }
        //    }

        //    return Json(model);
        //}

        // GET: Branches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Branch branch = Service.GetById((int)id);
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
                Service.Create(branch);
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
            Branch branch = Service.GetById((int)id);
            if (branch == null)
            {
                return HttpNotFound();
            }
            return View(branch);
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
                Service.AddOrUpdate(branch);
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
            Branch branch = Service.GetById((int)id);
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
            Branch branch = Service.GetById((int)id);
            Service.Delete(branch);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
