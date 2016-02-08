using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wm.Model;
using wm.Service;
using wm.Web2.Models;

namespace wm.Web2.Controllers
{
    public class OrdersController : BaseController
    {
        IOrderService _service;
        IOrderService Service { get { return _service; } }
        IBranchGoodCategoryService _branchGoodCategoryService;
        public OrdersController(ApplicationUserManager userManager, 
            IOrderService Service, 
            IBranchGoodCategoryService BranchGoodCategoryService): base(userManager)
        {
            _service = Service;
            _branchGoodCategoryService = BranchGoodCategoryService;
        }

        #region StaffOrder
        // GET: Order
        public ActionResult StaffOrder(int id, int? GoodCategoryId)
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
        public ActionResult StaffPopulateData(int id, int GoodCategoryId)//, PlacingOrderViewModel nnData)
        {
            var items = Service.PopulateData(id, GoodCategoryId);
            return Json(items);
        }


        //TODO: bulk update/delete/add EntityFramework.Extended
        [HttpPost]
        [AllowAnonymous]
        public ActionResult StaffPlaceOrder(int id, OrderViewModel inputViewModel)
        {
            Service.placingOrder(id, inputViewModel.data);

            //post-processing

            //Service.Update(model);
            return Json(new ReturnJsonObject<int> { status = ReturnStatus.ok.ToString(), data = 0 });
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult StaffCreateOrder(CreateOrderByStaffViewModel inputViewModel)
        {
            var model = new Order
            {
                BranchId = inputViewModel.branchId,
                OrderDay = inputViewModel.orderDay,
                CreatedBy = inputViewModel.employeeId,
                Status = OrderStatus.Started,
                Priority = 0
            };

            Service.Create(model);
            return OkCode();
        }

        #endregion
    }
}