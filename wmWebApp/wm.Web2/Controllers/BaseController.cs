using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;
using wm.Model;

namespace wm.Web2.Controllers
{
    public class BaseController : Controller
    {
        private ApplicationUserManager _userManager;

        protected ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public BaseController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public string GetUserId()
        {
            return User.Identity.GetUserId();
        }

        public ApplicationUser GetUser()
        {
            return UserManager.FindById(User.Identity.GetUserId());
        }


        protected ActionResult OkCode()
        {
            return Json(new ReturnJsonObject<int> { status = ReturnStatus.ok.ToString(), data = 0 });
        }
        
    }
}