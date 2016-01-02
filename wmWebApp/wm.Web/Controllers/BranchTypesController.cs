using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using wm.Core.Models;
using wm.Web.Models;

namespace wm.Web.Controllers
{
    public class BranchTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BranchTypes
        public ActionResult Index()
        {
            return View(db.BrandType.ToList());
        }

        // GET: BranchTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BranchType branchType = db.BrandType.Find(id);
            if (branchType == null)
            {
                return HttpNotFound();
            }
            return View(branchType);
        }

        // GET: BranchTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BranchTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] BranchType branchType)
        {
            if (ModelState.IsValid)
            {
                db.BrandType.Add(branchType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(branchType);
        }

        // GET: BranchTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BranchType branchType = db.BrandType.Find(id);
            if (branchType == null)
            {
                return HttpNotFound();
            }
            return View(branchType);
        }

        // POST: BranchTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] BranchType branchType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(branchType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(branchType);
        }

        // GET: BranchTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BranchType branchType = db.BrandType.Find(id);
            if (branchType == null)
            {
                return HttpNotFound();
            }
            return View(branchType);
        }

        // POST: BranchTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BranchType branchType = db.BrandType.Find(id);
            db.BrandType.Remove(branchType);
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
