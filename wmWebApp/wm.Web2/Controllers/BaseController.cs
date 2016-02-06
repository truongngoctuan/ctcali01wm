using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace wm.Web2.Controllers
{
    public class BaseController : Controller
    {
        protected ActionResult OkCode()
        {
            return Json(new ReturnJsonObject<int> { status = ReturnStatus.ok.ToString(), data = 0 });
        }
        
    }
}