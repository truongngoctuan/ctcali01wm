using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using wm.Model;
using wm.Service;
using wm.Web2.Models;

namespace wm.Web2.Controllers
{
    public class EmployeesController : BaseController
    {
        IEmployeeService _service;
        IEmployeeService Service { get { return _service; } }

        IBranchService _branchService;

        public EmployeesController(ApplicationUserManager userManager,
            IEmployeeService Service, IBranchService BranchService) : base(userManager)
        {
            _service = Service;
            _branchService = BranchService;
        }

        // GET: Employees
        public ActionResult Index()
        {
            var employees = Service.GetAll("Branch");
            return View(employees.ToList());
        }

        // GET: Employees/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = Service.GetByApplicationId(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.BranchId = new SelectList(_branchService.GetAll(), "Id", "Name");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Role,BranchId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                Service.Create(employee);
                return RedirectToAction("Index");
            }

            ViewBag.BranchId = new SelectList(_branchService.GetAll(), "Id", "Name", employee.BranchId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = Service.GetById((int)id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.BranchId = new SelectList(_branchService.GetAll(), "Id", "Name", employee.BranchId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ApplicationUserId,Name,Role,BranchId,PlainPassword,UserName")] Employee employee)
        {

            if (ModelState.IsValid)
            {
                if (employee.PlainPassword != string.Empty)
                {
                    UserManager.RemovePassword(employee.ApplicationUserId);
                    var result = UserManager.AddPassword(employee.ApplicationUserId, employee.PlainPassword);
                }
                if (employee.Role == EmployeeRole.SuperUser) employee.Role = EmployeeRole.Admin;
                var employeeIndatabase = Service.GetByApplicationId(employee.ApplicationUserId);
                switch (employeeIndatabase.Role)
                {
                    case EmployeeRole.Admin:
                        {
                            UserManager.RemoveFromRole(employee.ApplicationUserId, SystemRoles.Admin);
                            break;
                        }
                    case EmployeeRole.StaffBranch:
                        {
                            UserManager.RemoveFromRole(employee.ApplicationUserId, SystemRoles.Staff);
                            break;
                        }
                    case EmployeeRole.Manager:
                        {
                            UserManager.RemoveFromRole(employee.ApplicationUserId, SystemRoles.Manager);
                            break;
                        }
                    case EmployeeRole.WarehouseKeeper:
                        {
                            UserManager.RemoveFromRole(employee.ApplicationUserId, SystemRoles.WarehouseKeeper);
                            break;
                        }
                    default:
                        {
                            UserManager.RemoveFromRole(employee.ApplicationUserId, SystemRoles.Staff);
                            break;
                        }
                }

                switch (employee.Role)
                {
                    case EmployeeRole.Admin:
                        {
                            UserManager.AddToRole(employee.ApplicationUserId, SystemRoles.Admin);
                            break;
                        }
                    case EmployeeRole.StaffBranch:
                        {
                            UserManager.AddToRole(employee.ApplicationUserId, SystemRoles.Staff);
                            break;
                        }
                    case EmployeeRole.Manager:
                        {
                            UserManager.AddToRole(employee.ApplicationUserId, SystemRoles.Manager);
                            break;
                        }
                    case EmployeeRole.WarehouseKeeper:
                        {
                            UserManager.AddToRole(employee.ApplicationUserId, SystemRoles.WarehouseKeeper);
                            break;
                        }
                    default:
                        {
                            UserManager.AddToRole(employee.ApplicationUserId, SystemRoles.Staff);
                            break;
                        }

                }


                Service.Update(employee);
                return RedirectToAction("Index");
            }
            ViewBag.BranchId = new SelectList(_branchService.GetAll(), "Id", "Name", employee.BranchId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = Service.GetByApplicationId(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {

            var user = UserManager.FindById(id);
            var result = UserManager.Delete(user);
            if (result.Succeeded)
            {
                Employee employee = Service.GetByApplicationId(id);
                Service.Delete(employee);
            }
            else
            {

            }


            return RedirectToAction("Index");
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult List(DTParameters param)
        {
            try
            {
                int RecordsTotal = 0;
                int RecordsFiltered = 0;


                string paramSortOrder = param.SortOrder;
                paramSortOrder = paramSortOrder.Replace("BranchName", "Branch.Name");
                paramSortOrder = paramSortOrder.Replace("RoleName", "Role");
                //get sorted/paginated
                var resultSet = Service.ListDatatables(param.Search.Value, paramSortOrder, param.Start, param.Length, out RecordsTotal, out RecordsFiltered);

                var resultViewModel = resultSet.Select(e => new EmployeeDatatablesListViewModel
                {
                    id = e.Id,
                    Name = e.Name,
                    BranchName = e.Branch.Name,
                    RoleName = e.Role.ToString()
                });

                DTResult<EmployeeDatatablesListViewModel> DTResult = new DTResult<EmployeeDatatablesListViewModel>
                {
                    draw = param.Draw,
                    data = resultViewModel.ToList(),
                    recordsFiltered = RecordsFiltered,
                    recordsTotal = RecordsTotal
                };

                return Json(DTResult);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    error = ex.Message
                });
            }

        }
    }
}
