using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using wmWebApp.Models;

namespace wmWebApp.Controllers
{
    public class AgencyTypesController : Controller
    {
        private AutoCreatedContext db = new AutoCreatedContext();

        // GET: AgencyTypes
        public ActionResult Index()
        {
            return View(db.AgencyTypes.ToList());
        }

        // GET: AgencyTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgencyType agencyType = db.AgencyTypes.Find(id);
            if (agencyType == null)
            {
                return HttpNotFound();
            }
            return View(agencyType);
        }

        // GET: AgencyTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AgencyTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,SystemType")] AgencyType agencyType)
        {
            if (ModelState.IsValid)
            {
                db.AgencyTypes.Add(agencyType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(agencyType);
        }

        // GET: AgencyTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgencyType agencyType = db.AgencyTypes.Find(id);
            if (agencyType == null)
            {
                return HttpNotFound();
            }
            return View(agencyType);
        }

        // POST: AgencyTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,SystemType")] AgencyType agencyType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agencyType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agencyType);
        }

        // GET: AgencyTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgencyType agencyType = db.AgencyTypes.Find(id);
            if (agencyType == null)
            {
                return HttpNotFound();
            }
            return View(agencyType);
        }

        // POST: AgencyTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AgencyType agencyType = db.AgencyTypes.Find(id);
            db.AgencyTypes.Remove(agencyType);
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
