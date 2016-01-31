using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using wm.Model;

namespace wm.Web2.Controllers
{
    public class GoodUnitsController : Controller
    {
        private wmContext db = new wmContext();

        // GET: GoodUnits
        public ActionResult Index()
        {
            return View(db.GoodUnits.ToList());
        }

        // GET: GoodUnits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoodUnit goodUnit = db.GoodUnits.Find(id);
            if (goodUnit == null)
            {
                return HttpNotFound();
            }
            return View(goodUnit);
        }

        // GET: GoodUnits/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GoodUnits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] GoodUnit goodUnit)
        {
            if (ModelState.IsValid)
            {
                db.GoodUnits.Add(goodUnit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(goodUnit);
        }

        // GET: GoodUnits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoodUnit goodUnit = db.GoodUnits.Find(id);
            if (goodUnit == null)
            {
                return HttpNotFound();
            }
            return View(goodUnit);
        }

        // POST: GoodUnits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] GoodUnit goodUnit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(goodUnit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(goodUnit);
        }

        // GET: GoodUnits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoodUnit goodUnit = db.GoodUnits.Find(id);
            if (goodUnit == null)
            {
                return HttpNotFound();
            }
            return View(goodUnit);
        }

        // POST: GoodUnits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GoodUnit goodUnit = db.GoodUnits.Find(id);
            db.GoodUnits.Remove(goodUnit);
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
