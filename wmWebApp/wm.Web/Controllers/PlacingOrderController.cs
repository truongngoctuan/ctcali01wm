using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wm.Web.Models;

namespace wm.Web.Controllers
{
    public class PlacingOrderController : Controller
    {
        // GET: PlacingOrder
        public ActionResult Index()
        {
            return View();
        }

        // GET: PlacingOrder
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetOrderByDay()
        {
            IEnumerable<OrderItemViewModel> Items = new List<OrderItemViewModel>()
            {
                new OrderItemViewModel { Id = 1, Name = "Item 1", RecommendedQuantity = 10 },
                new OrderItemViewModel { Id = 2, Name = "Item 2", RecommendedQuantity = 8 },
                new OrderItemViewModel { Id = 3, Name = "Item 3", RecommendedQuantity = 50 },
                new OrderItemViewModel { Id = 4, Name = "Item 4", RecommendedQuantity = 15 },
                new OrderItemViewModel { Id = 5, Name = "Item 5", RecommendedQuantity = 1 }
            };
            return Json(Items);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult PlacingOrder(PlacingOrderViewModel model)
        {
            return Json(model);
        }
    }
}