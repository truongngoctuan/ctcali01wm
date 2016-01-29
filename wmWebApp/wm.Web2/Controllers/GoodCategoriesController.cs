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
    public class GoodCategoriesController : Controller
    {
        private wmContext db = new wmContext();

        // GET: GoodCategories
        public ActionResult Index()
        {
            return View(db.GoodCategories.ToList());
        }

        // GET: GoodCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoodCategory goodCategory = db.GoodCategories.Find(id);
            if (goodCategory == null)
            {
                return HttpNotFound();
            }
            return View(goodCategory);
        }

        // GET: GoodCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GoodCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] GoodCategory goodCategory)
        {
            if (ModelState.IsValid)
            {
                db.GoodCategories.Add(goodCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(goodCategory);
        }

        // GET: GoodCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoodCategory goodCategory = db.GoodCategories.Find(id);
            if (goodCategory == null)
            {
                return HttpNotFound();
            }
            return View(goodCategory);
        }

        // POST: GoodCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] GoodCategory goodCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(goodCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(goodCategory);
        }

        // GET: GoodCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoodCategory goodCategory = db.GoodCategories.Find(id);
            if (goodCategory == null)
            {
                return HttpNotFound();
            }
            return View(goodCategory);
        }

        // POST: GoodCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GoodCategory goodCategory = db.GoodCategories.Find(id);
            db.GoodCategories.Remove(goodCategory);
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
