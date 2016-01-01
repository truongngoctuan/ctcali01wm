using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using testDatabaseFirst.Models;

namespace testDatabaseFirst.Controllers
{
    public class AgentTypesController : Controller
    {
        private wmCaliEntities db = new wmCaliEntities();

        // GET: AgentTypes
        public ActionResult Index()
        {
            return View(db.AgentTypes.ToList());
        }

        // GET: AgentTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgentType agentType = db.AgentTypes.Find(id);
            if (agentType == null)
            {
                return HttpNotFound();
            }
            return View(agentType);
        }

        // GET: AgentTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AgentTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] AgentType agentType)
        {
            if (ModelState.IsValid)
            {
                db.AgentTypes.Add(agentType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(agentType);
        }

        // GET: AgentTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgentType agentType = db.AgentTypes.Find(id);
            if (agentType == null)
            {
                return HttpNotFound();
            }
            return View(agentType);
        }

        // POST: AgentTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] AgentType agentType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agentType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agentType);
        }

        // GET: AgentTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgentType agentType = db.AgentTypes.Find(id);
            if (agentType == null)
            {
                return HttpNotFound();
            }
            return View(agentType);
        }

        // POST: AgentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AgentType agentType = db.AgentTypes.Find(id);
            db.AgentTypes.Remove(agentType);
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
