using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using wm.Model;
using wm.Service;
using wm.Web2.Models;

namespace wm.Web2.Controllers
{
    public class MultiPurposeListController : BaseController
    {
        private IMultiPurposeListService Service { get; }
        private IMultiPurposeListGoodService _multiPurposeListGoodService { get; set; }
        public IGoodService _goodService { get; set; }
        public MultiPurposeListController(ApplicationUserManager userManager, IMultiPurposeListService service,
            IMultiPurposeListGoodService multiPurposeListGoodService, IGoodService goodService) : base(userManager)
        {
            Service = service;
            _multiPurposeListGoodService = multiPurposeListGoodService;
            _goodService = goodService;
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
        public ActionResult Create([Bind(Include = "Id,Name")] MultiPurposeList multiPurposeList)
        {
            if (ModelState.IsValid)
            {
                Service.Create(multiPurposeList);
                return RedirectToAction("Index");
            }

            return View(multiPurposeList);
        }

        // GET: GoodCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MultiPurposeList multiPurposeList = Service.GetById((int)id);
            if (multiPurposeList == null)
            {
                return HttpNotFound();
            }
            return View(multiPurposeList);
        }

        // POST: GoodCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] MultiPurposeList multiPurposeList)
        {
            if (ModelState.IsValid)
            {
                Service.Update(multiPurposeList);
                return RedirectToAction("Index");
            }
            return View(multiPurposeList);
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult PopulateData(int? id, bool isInEx)//, PlacingOrderViewModel nnData)
        {
            var items = PopulateNnData(id, isInEx);
            return Json(items);
        }

        private IEnumerable<MultiPurposeListInExItemViewModel> PopulateNnData(int? id, bool isInEx = true)
        {
            if (id == null) throw new Exception("id shouldn't be null");

            IEnumerable<MultiPurposeListInExItemViewModel> viewModel;
            if (isInEx)
            {//return all, with some checked items
                var allList = _goodService.GetAll();
                var filteredList = _multiPurposeListGoodService.Get((s => s.MultiPurposeListId == (int)id));

                //binding Data
                viewModel = allList.Select(t => new MultiPurposeListInExItemViewModel
                {
                    GoodId = t.Id,
                    Name = t.Name,
                    IsChecked = filteredList.Any(s => s.GoodId == t.Id)
                });
            }
            else
            {//return only checked item
                var filteredList = _multiPurposeListGoodService.Get((s => s.MultiPurposeListId == (int)id), null, "Good");

                //binding Data
                viewModel = filteredList
                    .OrderBy(t => t.Ranking)
                    .Select(t => new MultiPurposeListInExItemViewModel
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
        public ActionResult IncludeExcludeNnData(int id, MultiPurposeListInExViewModel inputViewModel)
        {//only checkbox, not ranking
            var oldLinkingList = _multiPurposeListGoodService.Get((s => s.MultiPurposeListId == (int)id));
            var newLinkingList = inputViewModel.data?.Select(t => new MultiPurposeListGood
            {
                MultiPurposeListId = id,
                GoodId = t.GoodId//enough Data
            }) ?? new List<MultiPurposeListGood>();

            var removeList = oldLinkingList.Where(t => !newLinkingList.Any(u => u.GoodId == t.GoodId));
            var editList = oldLinkingList.Where(t => newLinkingList.Any(u => u.GoodId == t.GoodId));
            var newList = newLinkingList.Where(t => !oldLinkingList.Any(u => u.GoodId == t.GoodId));

            //remove
            foreach (var item in removeList)
            {
                _multiPurposeListGoodService.Delete(item);
            }

            //edit - just re-index
            for (int i = 0; i < editList.Count(); i++)
            {
                editList.ElementAt(i).Ranking = i;
                _multiPurposeListGoodService.Update(editList.ElementAt(i));
            }

            //add new item
            int nRankedItem = editList.Count();
            foreach (var itemId in newList)
            {
                itemId.Ranking = nRankedItem;
                nRankedItem++;

                _multiPurposeListGoodService.Create(itemId);
            }

            //post-processing

            //service.Update(model);
            return OkCode();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult SortNnData(int id, MultiPurposeListInExViewModel inputViewModel)
        {
            var filteredList = _multiPurposeListGoodService.Get((s => s.MultiPurposeListId == id));

            //remove

            //edit - update ranking
            foreach (var item in inputViewModel.data)
            {
                var editObject = filteredList.First(t => t.GoodId == item.GoodId);
                editObject.Ranking = item.Ranking;
                _multiPurposeListGoodService.Update(editObject);
            }

            //add new item

            //post-processing

            return OkCode();
        }

        // GET: GoodCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MultiPurposeList multiPurposeList = Service.GetById((int)id);
            if (multiPurposeList == null)
            {
                return HttpNotFound();
            }
            return View(multiPurposeList);
        }

        // POST: GoodCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Service.Delete(Service.GetById(id));
            return RedirectToAction("Index");
        }
    }
}