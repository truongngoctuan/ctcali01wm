using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wm.Core.CRUDOperators;
using wm.Core.Models;
using wm.Core.Repositories;
using wm.Web.CRUDOperators.ViewModels;
using wm.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

namespace wm.Web.CRUDOperators.Controllers
{
    [AllowAnonymous]
    [Route(Name = "CRUDOperators")]
    public class ApplicationUserController : CRUDController
    {
        private IApplicationUserRepository _repository = new ApplicationUserRepository(new ApplicationDbContext());
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationUserController()
        {
        }

        public ApplicationUserController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
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

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult List(DTParameters param)
        {
            try
            {
                var resultSet = new DataTablesUtilApplicationUser(_repository);
                IEnumerable<ApplicationUser> result = resultSet.GetListFiltered(param);
                //TODO: auto mapper
                var resultViewModel = result.Select(e => new ApplicationUserListDatatableViewModel
                {
                    UserName = e.UserName,
                    FullName = e.FullName,
                    Branch = (e.Branch == null) ? "unknown" : e.Branch.Name,
                    Position = (e.Position == null) ? "unknown" : e.Position.Name,
                    ToolboxLinks = this.CreateToolboxLinks(e.Id)
                });

                var DTResult = resultSet.GetListDTResult<ApplicationUserListDatatableViewModel>(resultViewModel);
                return Json(DTResult);

                //    return Json(new Dictionary<string, List<BranchListViewModel>>()
                //{ { "data" , customListBranches.ToList() } }
                //    );
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    error = ex.Message
                });
            }

        }

        public async Task<ActionResult> Details(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            return View(user);
        }

        public async Task<ActionResult> Edit(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            return View(user);
        }
    }
}