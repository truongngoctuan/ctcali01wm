using System.Linq;
using System.Web.Mvc;
using wm.Model;
using wm.Service;
using wm.Web2.Models;

namespace wm.Web2.Controllers
{
    public class OrdersController : BaseController
    {
        private IOrderService Service { get; }
        readonly IBranchGoodCategoryService _branchGoodCategoryService;
        public OrdersController(ApplicationUserManager userManager, 
            IOrderService service, 
            IBranchGoodCategoryService branchGoodCategoryService): base(userManager)
        {
            Service = service;
            _branchGoodCategoryService = branchGoodCategoryService;
        }

        #region StaffOrder
        // GET: Order(index page, no care, load template, the same for each role)
        public ActionResult StaffOrder(int id, int? goodCategoryId)
        {
            var inputModel = Service.GetById(id);
            var filteredGoodCategoryList = _branchGoodCategoryService
                .GetByBranchId(inputModel.BranchId, "GoodCategory")
                .Select(t => t.GoodCategory);

            if (goodCategoryId == null)
            {
                goodCategoryId = filteredGoodCategoryList.First().Id;
            }
            //ViewBag.GoodCategories = filteredGoodCategoryList;
            ViewBag.OrderId = id;
            ViewBag.GoodCategoryId = (int)goodCategoryId;
            return View(filteredGoodCategoryList);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult StaffPopulateData(int id, int goodCategoryId)//, PlacingOrderViewModel nnData)
        {
            var items = Service.PopulateData(id, goodCategoryId);
            return Json(items);
        }


        //TODO: bulk update/delete/add EntityFramework.Extended
        [HttpPost]
        [AllowAnonymous]
        public ActionResult StaffPlaceOrder(int id, OrderViewModel inputViewModel)
        {
            Service.placingOrder(id, inputViewModel.data);

            //post-processing

            //service.Update(model);
            return Json(new ReturnJsonObject<int> { Status = ReturnStatus.Ok.ToString(), Data = 0 });
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

        [AllowAnonymous]
        public ActionResult StaffConfirmOrder(int id)
        {
            Service.ChangeStatus(id, OrderStatus.StaffConfirmed);

            return RedirectToAction("GeneralIndex", "Dashboard");
        }

        #endregion
    }
}