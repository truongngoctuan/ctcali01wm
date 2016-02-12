using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using wm.Model;
using wm.Service;
using wm.Web2.Models;

namespace wm.Web2.Controllers
{
    //many-to-many with addition informations, using checkboxes
    //http://www.asp.net/mvc/overview/getting-started/getting-started-with-ef-using-mvc/updating-related-Data-with-the-entity-framework-in-an-asp-net-mvc-application
    public class BranchesController : BaseController
    {
        private IBranchService Service { get; }
        readonly IGoodCategoryService _goodCategoryService;
        readonly IBranchGoodCategoryService _branchGoodCategoryService;
        public BranchesController(ApplicationUserManager userManager, 
            IBranchService service, 
            IGoodCategoryService goodCategoryService,
            IBranchGoodCategoryService branchGoodCategoryService): base(userManager)
        {
            Service = service;
            _goodCategoryService = goodCategoryService;
            _branchGoodCategoryService = branchGoodCategoryService;
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
            var items = PopulateNnData(id, isInEx);
            return Json(items);
        }

        private IEnumerable<BranchInExItemViewModel> PopulateNnData(int? id, bool isInEx = true)
        {
            if (id == null) throw new Exception("id shouldn't be null");

            IEnumerable<BranchInExItemViewModel> viewModel;
            if (isInEx)
            {//return all, with some checked items
                var allList = _goodCategoryService.GetAll();
                var filteredList = _branchGoodCategoryService.GetByBranchId((int)id);

                //binding Data
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

                //binding Data
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


        //TODO: bulk update/delete/add EntityFramework.Extended
        [HttpPost]
        [AllowAnonymous]
        public ActionResult IncludeExcludeNNData(int id, BranchInExViewModel inputViewModel)
        {//only checkbox, not ranking
            var oldLinkingList = _branchGoodCategoryService.GetByBranchId((int)id);
            var newLinkingList = (inputViewModel.data == null) ?
                new List<BranchGoodCategory>() :
                inputViewModel.data.Select(t => new BranchGoodCategory {
                    BranchId = id,
                    GoodCategoryId = t.CategoryId//enough Data
                });

            var removeList = oldLinkingList.Where(t => !newLinkingList.Any(u => u.GoodCategoryId == t.GoodCategoryId));
            var editList = oldLinkingList.Where(t => newLinkingList.Any(u => u.GoodCategoryId == t.GoodCategoryId));
            var newList = newLinkingList.Where(t => !oldLinkingList.Any(u => u.GoodCategoryId == t.GoodCategoryId));

            //remove
            foreach (var item in removeList)
            {
                _branchGoodCategoryService.Delete(item);
            }

            //edit - just re-index
            for (int i = 0; i < editList.Count(); i++)
            {
                editList.ElementAt(i).Ranking = i;
                _branchGoodCategoryService.Update(editList.ElementAt(i));
            }

            //add new item
            int nRankedItem = editList.Count();
            foreach (var itemId in newList)
            {
                itemId.Ranking = nRankedItem;
                nRankedItem++;

                _branchGoodCategoryService.Create(itemId);
            }

            //post-processing

            //service.Update(model);
            return Json(new ReturnJsonObject<int> { Status = ReturnStatus.Ok.ToString(), Data = 0 });
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult SortNnData(int id, BranchInExViewModel inputViewModel)
        {
            var filteredList = _branchGoodCategoryService.GetByBranchId((int)id);

            //remove

            //edit - update ranking
            foreach (var item in inputViewModel.data)
            {
                var editObject = filteredList.Where(t => t.GoodCategoryId == item.CategoryId).First();
                editObject.Ranking = item.Ranking;
                _branchGoodCategoryService.Update(editObject);
            }

            //add new item

            //post-processing

            return Json(new ReturnJsonObject<int> { Status = ReturnStatus.Ok.ToString(), Data = 0 });
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
        public ActionResult Create([Bind(Include = "Id,Name,BranchType")] Branch branch)
        {
            if (ModelState.IsValid)
            {
                var result = Service.Create(branch);
                if (!result.IsSucceed)
                {
                    ModelState.AddModelError(string.Empty, result.Messages.ElementAt(0));
                    return View(branch);
                }
                
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
        public ActionResult Edit([Bind(Include = "Id,Name,BranchType")] Branch branch)
        {
            if (ModelState.IsValid)
            {
                var result = Service.Update(branch);
                if (!result.IsSucceed)
                {
                    ModelState.AddModelError(string.Empty, result.Messages.ElementAt(0));
                    return View(branch);
                }

                
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
            var branch = Service.GetById(id);
            Service.Delete(branch);
            return RedirectToAction("Index");
        }

    }
}
