using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using wm.Core.Models;
using wm.Core.Repositories;
using wm.Web.CRUDOperators.Controllers;
using wm.Web.Models;

namespace wm.Web.Controllers
{
    public class BranchesController : CRUDController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private IBranchRepository _repostory = new BranchRepository(new ApplicationDbContext());

        // GET: Branches
        public ActionResult Index()
        {
            var branches = _repostory.GetListWithInclude("");
            var customListBranches = branches.Select(item => new BranchListViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                BranchTypeName = item.BranchType.Name
            });

            return View(customListBranches);
        }

        [HttpPost]
        public ActionResult List()
        {
            var branches = _repostory.GetListWithInclude("");
            var customListBranches = branches.Select(item => new BranchListViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                BranchTypeName = item.BranchType.Name,
                ToolboxLinks = this.CreateToolboxLinks(item.Id)
            });

            //ArrayList customListBranches = new ArrayList(branches.Count());
            //foreach (var item in branches)
            //{
            //    customListBranches.Add(new ArrayList() { item.Name, item.BranchType.Name,
            //        Url.Action("Details", "Branches", new { id = item.Id}),
            //        Url.Action("Edit", "Branches", new { id = item.Id}),
            //        Url.Action("Delete", "Branches", new { id = item.Id}),
            //    });
            //}

            //return Json(customListBranches);
            return Json(new Dictionary<string, List<BranchListViewModel>>()
            { { "data" , customListBranches.ToList() } }
            );
        }

        // GET: Branches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Branch branch = db.Branches.Find(id);
            if (branch == null)
            {
                return HttpNotFound();
            }
            return View(branch);
        }

        // GET: Branches/Create
        public ActionResult Create()
        {
            ViewBag.BranchTypeId = new SelectList(db.BranchTypes, "Id", "Name");
            return View();
        }

        // POST: Branches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Address,Phone,BranchTypeId")] Branch branch)
        {
            if (ModelState.IsValid)
            {
                db.Branches.Add(branch);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BranchTypeId = new SelectList(db.BranchTypes, "Id", "Name", branch.BranchTypeId);
            return View(branch);
        }

        // GET: Branches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Branch branch = db.Branches.Find(id);
            if (branch == null)
            {
                return HttpNotFound();
            }
            ViewBag.BranchTypeId = new SelectList(db.BranchTypes, "Id", "Name", branch.BranchTypeId);
            return View(branch);
        }

        // POST: Branches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Address,Phone,BranchTypeId")] Branch branch)
        {
            if (ModelState.IsValid)
            {
                db.Entry(branch).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BranchTypeId = new SelectList(db.BranchTypes, "Id", "Name", branch.BranchTypeId);
            return View(branch);
        }

        // GET: Branches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Branch branch = db.Branches.Find(id);
            if (branch == null)
            {
                return HttpNotFound();
            }
            return View(branch);
        }

        // POST: Branches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Branch branch = db.Branches.Find(id);
            db.Branches.Remove(branch);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
