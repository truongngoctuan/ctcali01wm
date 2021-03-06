﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public IBranchReadOnlyService BranchReadOnlyService { get; set; }
        public IMultiPurposeListBranchService MultiPurposeListBranchService { get; set; }
        public MultiPurposeListController(ApplicationUserManager userManager, IMultiPurposeListService service,
            IMultiPurposeListGoodService multiPurposeListGoodService, IGoodService goodService, IBranchReadOnlyService branchReadOnlyService, IMultiPurposeListBranchService multiPurposeListBranchService) : base(userManager)
        {
            Service = service;
            _multiPurposeListGoodService = multiPurposeListGoodService;
            _goodService = goodService;
            BranchReadOnlyService = branchReadOnlyService;
            MultiPurposeListBranchService = multiPurposeListBranchService;
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

        #region goods list
        [AllowAnonymous]
        [HttpPost]
        public ActionResult List(int id)
        {
            var resultViewModel = PopulateNnData(id, true);
            var dtResult = new DTResult<MultiPurposeListGoodInExItemViewModel>
            {
                draw = 0,
                data = resultViewModel.ToList(),
                recordsFiltered = resultViewModel.Count(),
                recordsTotal = resultViewModel.Count()
            };

            return Json(dtResult);
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult PopulateData(int? id, bool isInEx)//, PlacingOrderViewModel nnData)
        {
            var items = PopulateNnData(id, isInEx);
            return Json(items);
        }

        private IEnumerable<MultiPurposeListGoodInExItemViewModel> PopulateNnData(int? id, bool isInEx = true)
        {
            if (id == null) throw new Exception("id shouldn't be null");

            IEnumerable<MultiPurposeListGoodInExItemViewModel> viewModel;
            if (isInEx)
            {//return all, with some checked items
                var allList = _goodService.GetAll("Unit").ToList();
                var filteredList = _multiPurposeListGoodService.Get((s => s.MultiPurposeListId == (int)id)).ToList();

                //binding Data
                viewModel = allList.Select(t => new MultiPurposeListGoodInExItemViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    AccoutantCode = t.AccountantCode,
                    UnitName = t.Unit.Name,
                    GoodType = t.GoodType.ToString(),
                    IsChecked = filteredList.Any(s => s.GoodId == t.Id)
                });
            }
            else
            {//return only checked item
                var filteredList = _multiPurposeListGoodService.Get((s => s.MultiPurposeListId == (int)id), null, "Good,Good.Unit").ToList();

                //binding Data
                viewModel = filteredList
                    .OrderBy(t => t.Ranking)
                    .Select(t => new MultiPurposeListGoodInExItemViewModel
                    {
                        Id = t.GoodId,
                        Name = t.Good.Name,
                        Ranking = t.Ranking,
                        AccoutantCode = t.Good.AccountantCode,
                        UnitName = t.Good.Unit.Name,
                        GoodType = t.Good.GoodType.ToString(),
                        IsChecked = true
                    });
            }

            return viewModel;
        }


        //TODO: bulk update/delete/add EntityFramework.Extended
        [HttpPost]
        [AllowAnonymous]
        public ActionResult IncludeExcludeNnData(int id, MultiPurposeListGoodInExViewModel inputViewModel)
        {//only checkbox, not ranking
            var oldLinkingList = _multiPurposeListGoodService.Get((s => s.MultiPurposeListId == (int)id));
            var newLinkingList = inputViewModel.data?.Select(t => new MultiPurposeListGood
            {
                MultiPurposeListId = id,
                GoodId = t.Id//enough Data
            }) ?? new List<MultiPurposeListGood>();

            var removeList = oldLinkingList.Where(t => !newLinkingList.Any(u => u.GoodId == t.GoodId));
            var editList = oldLinkingList.Where(t => newLinkingList.Any(u => u.GoodId == t.GoodId)).OrderBy(t => t.Ranking);
            var newList = newLinkingList.Where(t => !oldLinkingList.Any(u => u.GoodId == t.GoodId));

            //remove
            foreach (var item in removeList.ToList())
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
        public ActionResult SortNnData(int id, MultiPurposeListGoodInExViewModel inputViewModel)
        {
            var filteredList = _multiPurposeListGoodService.Get((s => s.MultiPurposeListId == id));

            //remove

            //edit - update ranking
            foreach (var item in inputViewModel.data)
            {
                var editObject = filteredList.First(t => t.GoodId == item.Id);
                editObject.Ranking = item.Ranking;
                _multiPurposeListGoodService.Update(editObject);
            }

            //add new item

            //post-processing

            return OkCode();
        }
        #endregion

        #region branches list
        [AllowAnonymous]
        [HttpPost]
        public ActionResult ListBranches(int id)
        {
            var resultViewModel = PopulateNnDataBranches(id, true);
            var dtResult = new DTResult<MultiPurposeListBranchInExItemViewModel>
            {
                draw = 0,
                data = resultViewModel.ToList(),
                recordsFiltered = resultViewModel.Count(),
                recordsTotal = resultViewModel.Count()
            };

            return Json(dtResult);
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult PopulateDataBranches(int? id, bool isInEx)//, PlacingOrderViewModel nnData)
        {
            var items = PopulateNnDataBranches(id, isInEx);
            return Json(items);
        }

        private IEnumerable<MultiPurposeListBranchInExItemViewModel> PopulateNnDataBranches(int? id, bool isInEx = true)
        {
            if (id == null) throw new Exception("id shouldn't be null");

            IEnumerable<MultiPurposeListBranchInExItemViewModel> viewModel;
            if (isInEx)
            {//return all, with some checked items
                var allList = BranchReadOnlyService.GetAll().ToList();
                var filteredList = MultiPurposeListBranchService.Get((s => s.MultiPurposeListId == (int)id)).ToList();

                //binding Data
                viewModel = allList.Select(t => new MultiPurposeListBranchInExItemViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    IsChecked = filteredList.Any(s => s.BranchId == t.Id)
                });
            }
            else
            {//return only checked item
                var filteredList = MultiPurposeListBranchService.Get((s => s.MultiPurposeListId == (int)id), null, "Branch").ToList();

                //binding Data
                viewModel = filteredList
                    .OrderBy(t => t.Ranking)
                    .Select(t => new MultiPurposeListBranchInExItemViewModel
                    {
                        Id = t.BranchId,
                        Name = t.Branch.Name,
                        Ranking = t.Ranking,
                        IsChecked = true
                    });
            }

            return viewModel;
        }


        //TODO: bulk update/delete/add EntityFramework.Extended
        [HttpPost]
        [AllowAnonymous]
        public ActionResult IncludeExcludeNnDataBranches(int id, MultiPurposeListBranchInExViewModel inputViewModel)
        {//only checkbox, not ranking
            var oldLinkingList = MultiPurposeListBranchService.Get((s => s.MultiPurposeListId == (int)id));
            var newLinkingList = inputViewModel.data?.Select(t => new MultiPurposeListBranch
            {
                MultiPurposeListId = id,
                BranchId = t.Id//enough Data
            }) ?? new List<MultiPurposeListBranch>();

            var removeList = oldLinkingList.Where(t => !newLinkingList.Any(u => u.BranchId == t.BranchId));
            var editList = oldLinkingList.Where(t => newLinkingList.Any(u => u.BranchId == t.BranchId)).OrderBy(t => t.Ranking);
            var newList = newLinkingList.Where(t => !oldLinkingList.Any(u => u.BranchId == t.BranchId));

            //remove
            foreach (var item in removeList.ToList())
            {
                MultiPurposeListBranchService.Delete(item);
            }

            //edit - just re-index
            for (int i = 0; i < editList.Count(); i++)
            {
                editList.ElementAt(i).Ranking = i;
                MultiPurposeListBranchService.Update(editList.ElementAt(i));
            }

            //add new item
            int nRankedItem = editList.Count();
            foreach (var itemId in newList)
            {
                itemId.Ranking = nRankedItem;
                nRankedItem++;

                MultiPurposeListBranchService.Create(itemId);
            }

            //post-processing

            //service.Update(model);
            return OkCode();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult SortNnDataBranches(int id, MultiPurposeListBranchInExViewModel inputViewModel)
        {
            var filteredList = MultiPurposeListBranchService.Get((s => s.MultiPurposeListId == id));

            //remove

            //edit - update ranking
            foreach (var item in inputViewModel.data)
            {
                var editObject = filteredList.First(t => t.BranchId == item.Id);
                editObject.Ranking = item.Ranking;
                MultiPurposeListBranchService.Update(editObject);
            }

            //add new item

            //post-processing

            return OkCode();
        }
        #endregion

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