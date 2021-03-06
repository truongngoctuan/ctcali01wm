﻿using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using wm.Model;
using wm.Service;
using wm.Web2.Models;

namespace wm.Web2.Controllers
{
    public class EmployeesController : BaseController
    {
        public IEmployeeCrudService ServiceCrud { get; set; }

        readonly IBranchReadOnlyService _branchReadOnlyService;

        public EmployeesController(ApplicationUserManager userManager,
            IEmployeeCrudService serviceCrud,
            IBranchReadOnlyService branchReadOnlyService) : base(userManager)
        {
            ServiceCrud = serviceCrud;
            _branchReadOnlyService = branchReadOnlyService;
        }

        // GET: Employees
        public ActionResult Index()
        {
            var employees = ServiceCrud.GetAll("Branch");
            return View(employees.ToList());
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = ServiceCrud.GetById((int)id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.BranchId = new SelectList(_branchReadOnlyService.GetAll(), "Id", "Name", employee.BranchId);
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
                    UserManager.AddPassword(employee.ApplicationUserId, employee.PlainPassword);
                }
                if (employee.Role == EmployeeRole.SuperUser) employee.Role = EmployeeRole.Admin;
                var employeeIndatabase = ServiceCrud.GetByApplicationId(employee.ApplicationUserId);
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


                ServiceCrud.Update(employee);
                return RedirectToAction("Index");
            }
            ViewBag.BranchId = new SelectList(_branchReadOnlyService.GetAll(), "Id", "Name", employee.BranchId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = ServiceCrud.GetByApplicationId(id);
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
                Employee employee = ServiceCrud.GetByApplicationId(id);
                ServiceCrud.Delete(employee);
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
                int recordsTotal;
                int recordsFiltered;


                var paramSortOrder = param.SortOrder;
                paramSortOrder = paramSortOrder.Replace("BranchName", "Branch.Name");
                paramSortOrder = paramSortOrder.Replace("RoleName", "Role");
                //get sorted/paginated
                string SearchValue = param.Search.Value;
                var resultSet = ServiceCrud.ListDatatables(s => SearchValue == null || s.Name.Contains(SearchValue),
                                                 paramSortOrder, param.Start, param.Length, out recordsTotal, out recordsFiltered);

                var resultViewModel = resultSet.Select(e => new EmployeeDatatablesListViewModel
                {
                    id = e.Id,
                    Name = e.Name,
                    BranchName = e.Branch.Name,
                    RoleName = e.Role.ToString()
                });

                var dtResult = new DTResult<EmployeeDatatablesListViewModel>
                {
                    draw = param.Draw,
                    data = resultViewModel.ToList(),
                    recordsFiltered = recordsFiltered,
                    recordsTotal = recordsTotal
                };

                return Json(dtResult);
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
