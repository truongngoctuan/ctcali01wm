using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wm.Service;

namespace wm.Web2.Controllers
{
    public class OrdersController : BaseController
    {
        IOrderService _service;
        IOrderService Service { get { return _service; } }
        IBranchGoodCategoryService _branchGoodCategoryService;
        public OrdersController(IOrderService Service, 
            IBranchGoodCategoryService BranchGoodCategoryService)
        {
            _service = Service;
            _branchGoodCategoryService = BranchGoodCategoryService;
        }
        // GET: Order
        public ActionResult Index(int id, int? GoodCategoryId)
        {
            var inputModel = Service.GetById(id);
            var filteredGoodCategoryList = _branchGoodCategoryService
                .GetByBranchId(inputModel.BranchId, "GoodCategory")
                .Select(t => t.GoodCategory);

            if (GoodCategoryId == null)
            {
                GoodCategoryId = filteredGoodCategoryList.First().Id;
            }
            //ViewBag.GoodCategories = filteredGoodCategoryList;
            ViewBag.OrderId = id;
            ViewBag.GoodCategoryId = (int)GoodCategoryId;
            return View(filteredGoodCategoryList);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult PopulateData(int id, int GoodCategoryId)//, PlacingOrderViewModel nnData)
        {
            var items = Service.PopulateData(id, GoodCategoryId);
            return Json(items);
        }

    }
}