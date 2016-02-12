using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using wm.Model;
using wm.Service;
using wm.Web2.Controllers.OrderStrategy;
using wm.Web2.Models;

namespace wm.Web2.Controllers
{
    [Authorize]
    public class OrdersController : BaseController
    {
        private IOrderService Service { get; }
        readonly IBranchGoodCategoryService _branchGoodCategoryService;

        #region strategy

        private IEmployeeService EmployeeService { get; set; }
        OrderControllerStrategyBase _strategyBase;
        private OrderControllerStrategyBase StrategyBase
        {
            get
            {
                if (_strategyBase != null) return _strategyBase;

                var employee = EmployeeService.GetByApplicationId(GetUserId());
                //_calendarEventService.Role = employee.Role;
                _strategyBase = GetAssociateStrategy(employee.Role);
                return _strategyBase;
            }
        }

        private OrderControllerStrategyBase GetAssociateStrategy(EmployeeRole role)
        {
            switch (role)
            {
                case EmployeeRole.Manager:
                    {
                        return new StaffOrderControllerStrategy(Service);
                    }
                case EmployeeRole.StaffBranch:
                    {
                        return new StaffOrderControllerStrategy(Service);
                    }
                case EmployeeRole.WarehouseKeeper:
                    {
                        return new StaffOrderControllerStrategy(Service);
                    }
                case EmployeeRole.Admin:
                    {
                        return new StaffOrderControllerStrategy(Service);
                    }
                case EmployeeRole.SuperUser:
                    {
                        return new StaffOrderControllerStrategy(Service);
                    }
            }
            return new StaffOrderControllerStrategy(Service);
        }
        #endregion
        public OrdersController(ApplicationUserManager userManager,
            IOrderService service,
            IBranchGoodCategoryService branchGoodCategoryService, IEmployeeService employeeService) : base(userManager)
        {
            Service = service;
            _branchGoodCategoryService = branchGoodCategoryService;
            EmployeeService = employeeService;
        }

        int? GetAssociateGoodCategories(int orderId, 
            out IEnumerable<GoodCategory> filteredGoodCategoryList,
            int? goodCategoryId)
        {
            var inputModel = Service.GetById(orderId);
            filteredGoodCategoryList = _branchGoodCategoryService
                .GetByBranchId(inputModel.BranchId, "GoodCategory")
                .Select(t => t.GoodCategory);

            return goodCategoryId ?? filteredGoodCategoryList.First().Id;
        }
        // GET: Order(index page, no care, load template, the same for each role)
        public ActionResult StaffEditOrder(int id, int? goodCategoryId)
        {
            IEnumerable<GoodCategory> filteredGoodCategoryList;
            goodCategoryId = GetAssociateGoodCategories(id, out filteredGoodCategoryList, goodCategoryId);

            ViewBag.OrderId = id;
            ViewBag.GoodCategoryId = (int)goodCategoryId;
            return View(filteredGoodCategoryList);
        }

        public ActionResult StaffDetailsOrder(int id, int? goodCategoryId)
        {
            IEnumerable<GoodCategory> filteredGoodCategoryList;
            goodCategoryId = GetAssociateGoodCategories(id, out filteredGoodCategoryList, goodCategoryId);

            ViewBag.OrderId = id;
            ViewBag.GoodCategoryId = (int)goodCategoryId;
            return View(filteredGoodCategoryList);
        }

        public ActionResult ManagerEditOrder(int id, int? goodCategoryId)
        {
            IEnumerable<GoodCategory> filteredGoodCategoryList;
            goodCategoryId = GetAssociateGoodCategories(id, out filteredGoodCategoryList, goodCategoryId);

            ViewBag.OrderId = id;
            ViewBag.GoodCategoryId = (int)goodCategoryId;
            return View(filteredGoodCategoryList);
        }

        public ActionResult ManagerDetailsOrder(int id, int? goodCategoryId)
        {
            IEnumerable<GoodCategory> filteredGoodCategoryList;
            goodCategoryId = GetAssociateGoodCategories(id, out filteredGoodCategoryList, goodCategoryId);

            ViewBag.OrderId = id;
            ViewBag.GoodCategoryId = (int)goodCategoryId;
            return View(filteredGoodCategoryList);
        }


        [HttpPost]
        public ActionResult PopulateData(int orderId, int goodCategoryId)//, PlacingOrderViewModel nnData)
        {
            var items = StrategyBase.PopulateData(orderId, goodCategoryId);
            return Json(items);
        }


        //TODO: bulk update/delete/add EntityFramework.Extended
        [HttpPost]
        public ActionResult Place(int id, OrderViewModel inputViewModel)
        {
            Service.Place(id, inputViewModel.data);
            return OkCode();
        }

        [HttpPost]
        public ActionResult Create(CreateOrderByStaffViewModel inputViewModel)
        {
            Order newOrder = StrategyBase.Create(inputViewModel.orderDay, inputViewModel.employeeId, inputViewModel.branchId);
            return Json(new ReturnJsonObject<int> {Status = ReturnStatus.Ok.ToString(), Result = newOrder.Id});
        }

        public ActionResult Confirm(int id)
        {
            StrategyBase.Confirm(id);
            return RedirectToAction("GeneralIndex", "Dashboard");
        }
    }
}