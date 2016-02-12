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
    public class GoodCategoriesController : BaseController
    {
        private IGoodCategoryService Service { get; }
        readonly IGoodService _goodService;
        readonly IGoodCategoryGoodService _goodCategoryGoodService;
        public GoodCategoriesController(ApplicationUserManager userManager, 
            IGoodCategoryService service,
            IGoodService goodService,
            IGoodCategoryGoodService goodCategoryGoodService): base(userManager)
        {
            Service = service;
            _goodService = goodService;
            _goodCategoryGoodService = goodCategoryGoodService;
        }

        // GET: GoodCategories
        public ActionResult Index()
        {
            return View(Service.GetAll());
        }
        
        // GET: GoodCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GoodCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] GoodCategory goodCategory)
        {
            if (ModelState.IsValid)
            {
                Service.Create(goodCategory);
                return RedirectToAction("Index");
            }

            return View(goodCategory);
        }

        // GET: GoodCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoodCategory goodCategory = Service.GetById((int)id);
            if (goodCategory == null)
            {
                return HttpNotFound();
            }
            return View(goodCategory);
        }

        // POST: GoodCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] GoodCategory goodCategory)
        {
            if (ModelState.IsValid)
            {
                Service.Update(goodCategory);
                return RedirectToAction("Index");
            }
            return View(goodCategory);
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult PopulateData(int? id, bool isInEx)//, PlacingOrderViewModel nnData)
        {
            var items = PopulateNnData(id, isInEx);
            return Json(items);
        }

        private IEnumerable<GoodCategoryInExItemViewModel> PopulateNnData(int? id, bool isInEx = true)
        {
            if (id == null) throw new Exception("id shouldn't be null");

            IEnumerable<GoodCategoryInExItemViewModel> viewModel;
            if (isInEx)
            {//return all, with some checked items
                var allList = _goodService.GetAll();
                var filteredList = _goodCategoryGoodService.GetByGoodCategoryId((int)id);

                //binding Data
                viewModel = allList.Select(t => new GoodCategoryInExItemViewModel
                {
                    GoodId = t.Id,
                    Name = t.Name,
                    IsChecked = filteredList.Any(s => s.GoodId == t.Id)
                });
            }
            else
            {//return only checked item
                var filteredList = _goodCategoryGoodService.GetByGoodCategoryId((int)id, "Good");

                //binding Data
                viewModel = filteredList
                    .OrderBy(t => t.Ranking)
                    .Select(t => new GoodCategoryInExItemViewModel
                    {
                        GoodId = t.GoodId,
                        Name = t.Good.Name,
                        Ranking = t.Ranking,
                        IsChecked = true
                    });
            }

            return viewModel;
        }


        //TODO: bulk update/delete/add EntityFramework.Extended
        [HttpPost]
        [AllowAnonymous]
        public ActionResult IncludeExcludeNnData(int id, GoodCategoryInExViewModel inputViewModel)
        {//only checkbox, not ranking
            var oldLinkingList = _goodCategoryGoodService.GetByGoodCategoryId((int)id);
            var newLinkingList = inputViewModel.data?.Select(t => new GoodCategoryGood
            {
                GoodCategoryId = id,
                GoodId = t.GoodId//enough Data
            }) ?? new List<GoodCategoryGood>();

            var removeList = oldLinkingList.Where(t => !newLinkingList.Any(u => u.GoodId == t.GoodId));
            var editList = oldLinkingList.Where(t => newLinkingList.Any(u => u.GoodId == t.GoodId));
            var newList = newLinkingList.Where(t => !oldLinkingList.Any(u => u.GoodId == t.GoodId));

            //remove
            foreach (var item in removeList)
            {
                _goodCategoryGoodService.Delete(item);
            }

            //edit - just re-index
            for (int i = 0; i < editList.Count(); i++)
            {
                editList.ElementAt(i).Ranking = i;
                _goodCategoryGoodService.Update(editList.ElementAt(i));
            }

            //add new item
            int nRankedItem = editList.Count();
            foreach (var itemId in newList)
            {
                itemId.Ranking = nRankedItem;
                nRankedItem++;

                _goodCategoryGoodService.Create(itemId);
            }

            //post-processing

            //service.Update(model);
            return Json(new ReturnJsonObject<int> { Status = ReturnStatus.Ok.ToString(), Data = 0 });
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult SortNnData(int id, GoodCategoryInExViewModel inputViewModel)
        {
            var filteredList = _goodCategoryGoodService.GetByGoodCategoryId((int)id);

            //remove

            //edit - update ranking
            foreach (var item in inputViewModel.data)
            {
                var editObject = filteredList.First(t => t.GoodId == item.GoodId);
                editObject.Ranking = item.Ranking;
                _goodCategoryGoodService.Update(editObject);
            }

            //add new item

            //post-processing

            return Json(new ReturnJsonObject<int> { Status = ReturnStatus.Ok.ToString(), Data = 0 });
        }

        // GET: GoodCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoodCategory goodCategory = Service.GetById((int)id);
            if (goodCategory == null)
            {
                return HttpNotFound();
            }
            return View(goodCategory);
        }

        // POST: GoodCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Service.Delete(Service.GetById(id));
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
