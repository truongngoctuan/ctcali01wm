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
        public ActionResult Index()
        {
            var inputModel = Service.GetById(1);
            var filteredGoodCategoryList = _branchGoodCategoryService
                .GetByBranchId(inputModel.BranchId, "GoodCategory")
                .Select(t => t.GoodCategory);

            ViewBag.GoodCategories = filteredGoodCategoryList;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult PopulateData(int id, int goodCategoryId)//, PlacingOrderViewModel nnData)
        {
            var items = Service.PopulateData(id, goodCategoryId);
            return Json(items);
        }

    }
}