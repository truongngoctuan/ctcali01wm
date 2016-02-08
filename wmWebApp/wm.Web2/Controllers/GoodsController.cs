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

namespace wm.Web2.Controllers
{
    public class GoodsController : BaseController
    {
        IGoodService _service;
        IGoodService Service { get { return _service; } }
        IGoodUnitService _goodUnitService;
        public GoodsController(ApplicationUserManager userManager, IGoodService Service,
            IGoodUnitService GoodUnitService):base(userManager)
        {
            _service = Service;
            _goodUnitService = GoodUnitService;
        }
        // GET: Goods
        public ActionResult Index()
        {
            var goods = Service.GetAllInclude();
            return View(goods.ToList());
        }

        // GET: Goods/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Good good = Service.GetById((int)id);
            if (good == null)
            {
                return HttpNotFound();
            }
            return View(good);
        }

        // GET: Goods/Create
        public ActionResult Create()
        {
            ViewBag.UnitId = new SelectList(_goodUnitService.GetAll(), "Id", "Name");
            return View();
        }

        // POST: Goods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,NameASCII,UnitId")] Good good)
        {
            if (ModelState.IsValid)
            {
                Service.Create(good);
                return RedirectToAction("Index");
            }

            ViewBag.UnitId = new SelectList(_goodUnitService.GetAll(), "Id", "Name", good.UnitId);
            return View(good);
        }

        // GET: Goods/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Good good = Service.GetById((int)id);
            if (good == null)
            {
                return HttpNotFound();
            }
            ViewBag.UnitId = new SelectList(_goodUnitService.GetAll(), "Id", "Name", good.UnitId);
            return View(good);
        }

        // POST: Goods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,NameASCII,UnitId")] Good good)
        {
            if (ModelState.IsValid)
            {
                Service.Update(good);
                return RedirectToAction("Index");
            }
            ViewBag.UnitId = new SelectList(_goodUnitService.GetAll(), "Id", "Name", good.UnitId);
            return View(good);
        }

        // GET: Goods/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Good good = Service.GetById((int)id);
            if (good == null)
            {
                return HttpNotFound();
            }
            return View(good);
        }

        // POST: Goods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Service.Delete(Service.GetById(id));
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }
    }
}
