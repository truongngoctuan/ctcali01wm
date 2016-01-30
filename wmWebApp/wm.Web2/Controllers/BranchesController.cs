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
    //many-to-many with addition informations, using checkboxes
    //http://www.asp.net/mvc/overview/getting-started/getting-started-with-ef-using-mvc/updating-related-data-with-the-entity-framework-in-an-asp-net-mvc-application
    public class BranchesController : BaseController
    {
        IBranchService _service;
        IBranchService Service { get { return _service; } }
        IGoodCategoryService _goodCategoryService;
        IBranchGoodCategoryService _branchGoodCategoryService;
        public BranchesController(IBranchService Service, 
            IGoodCategoryService GoodCategoryService,
            IBranchGoodCategoryService BranchGoodCategoryService)
        {
            _service = Service;
            _goodCategoryService = GoodCategoryService;
            _branchGoodCategoryService = BranchGoodCategoryService;
        }
        // GET: Branches
        public ActionResult Index()
        {
            return View(Service.GetAll());
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult PopulateData(int? id, bool isInEx)//, PlacingOrderViewModel nnData)
        {
            var items = PopulateNNData(id, isInEx);
            return Json(items);
        }

        private IEnumerable<BranchInExItemViewModel> PopulateNNData(int? id, bool isInEx = true)
        {
            if (id == null) throw new Exception("id shouldn't be null");

            IEnumerable<BranchInExItemViewModel> viewModel = new List<BranchInExItemViewModel>();
            if (isInEx)
            {//return all, with some checked items
                var allList = _goodCategoryService.GetAll();
                var filteredList = _branchGoodCategoryService.GetByBranchId((int)id);

                //binding data
                viewModel = allList.Select(t => new BranchInExItemViewModel
                {
                    CategoryId = t.Id,
                    Name = t.Name,
                    IsChecked = filteredList.Any(s => s.GoodCategoryId == t.Id)
                });
            }
            else
            {//return only checked item
                var filteredList = _branchGoodCategoryService.GetByBranchId((int)id, "GoodCategory");

                //binding data
                viewModel = filteredList
                    .OrderBy(t => t.Ranking)
                    .Select(t => new BranchInExItemViewModel
                {
                    CategoryId = t.GoodCategoryId,
                    Name = t.GoodCategory.Name,
                    Ranking = t.Ranking,
                    IsChecked = true
                });
            }

            return viewModel;
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult IncludeExcludeNNData(int id, BranchInExViewModel inputViewModel)
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
            int nRankedItem = model.BranchGoodCategories.Count();
            foreach (var itemId in newIds)
            {
                var newObject = new BranchGoodCategory
                {
                    Branch = model,
                    GoodCategory = _goodCategoryService.GetById(itemId),
                    Ranking = nRankedItem + 1
                };
                nRankedItem++;

                model.BranchGoodCategories.Add(newObject);
            }

            //post-processing
            //re-index ranking
            for (int i = 0; i < model.BranchGoodCategories.Count(); i++)
            {
                model.BranchGoodCategories.ElementAt(i).Ranking = i;
            }

            Service.Update(model);
            return Json(new ReturnJsonObject<int> { status = ReturnStatus.ok.ToString(), data = 0 });
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult SortNNData(int id, BranchInExViewModel inputViewModel)
        {
            var model = Service.GetById(id);

            //remove

            //edit - update ranking
            foreach (var item in inputViewModel.data)
            {
                var editObject = model.BranchGoodCategories.Where(t => t.GoodCategoryId == item.CategoryId).First();
                editObject.Ranking = item.Ranking;
            }

            //add new item

            //post-processing

            Service.Update(model);

            return Json(new ReturnJsonObject<int> { status = ReturnStatus.ok.ToString(), data = 0 });
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
        public ActionResult Edit(int? id, bool isInEx = true)
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
            ViewBag.isInEx = isInEx;
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
