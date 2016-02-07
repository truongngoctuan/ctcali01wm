using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using wm.Web2.Models;
using wm.Model;
using wm.Service;
using System.Collections.Generic;

namespace wm.Web2.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        IEmployeeService _service;
        IBranchService _branchService;
        IEmployeeService Service { get { return _service; } }


        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager,
            IEmployeeService Service, IBranchService BranchService)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _service = Service;
            _branchService = BranchService;
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

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }


        private IEnumerable<SelectListItem> getBranches()
        {
            var list = _branchService.GetAll();
            IEnumerable<SelectListItem> selectList = new List<SelectListItem>();
            selectList = list.Select(o => new SelectListItem
            {
                Value = o.Id.ToString(),
                Text = o.Name
            });

            return selectList;
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.Branches = getBranches();
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.PlainPassword);
                if (result.Succeeded)
                {
                    switch(model.Role)
                    {
                        case EmployeeRole.SuperUser:
                            {//not allow to create superuser :D
                                model.Role = EmployeeRole.Admin;
                                UserManager.AddToRole(user.Id, SystemRoles.Admin);
                                break;
                            }
                        case EmployeeRole.Admin:
                            {
                                UserManager.AddToRole(user.Id, SystemRoles.Admin);
                                break;
                            }
                        case EmployeeRole.StaffBranch:
                            {
                                UserManager.AddToRole(user.Id, SystemRoles.Staff);
                                break;
                            }
                        case EmployeeRole.Manager:
                            {
                                UserManager.AddToRole(user.Id, SystemRoles.Manager);
                                break;
                            }
                        case EmployeeRole.WarehouseKeeper:
                            {
                                UserManager.AddToRole(user.Id, SystemRoles.WarehouseKeeper);
                                break;
                            }
                        default:
                            {
                                UserManager.AddToRole(user.Id, SystemRoles.Staff);
                                break;
                            }

                    }
                    var employee = new Employee
                    {
                        PlainPassword = model.PlainPassword, //when user change password, this will be reset

                        ApplicationUserId = user.Id,
                        UserName = user.UserName,
                        Name = model.FullName,
                        BranchId = model.BranchId,
                        Role = model.Role
                    };
                    Service.Create(employee);

                    //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Employees");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            ViewBag.Branches = getBranches();
            return View(model);
        }
        
        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            ViewBag.ReturnUrl = "";
            return RedirectToAction("Login", "Account");
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("GeneralIndex", "Dashboard");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}