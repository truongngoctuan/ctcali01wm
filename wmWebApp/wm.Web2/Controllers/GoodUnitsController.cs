using System.Net;
using System.Web.Mvc;
using wm.Model;
using wm.Service;

namespace wm.Web2.Controllers
{
    public class GoodUnitsController : BaseController
    {
        readonly IGoodUnitService _service;
        IGoodUnitService Service { get { return _service; } }
        public GoodUnitsController(ApplicationUserManager userManager, 
            IGoodUnitService Service):base(userManager)
        {
            _service = Service;
        }
        // GET: GoodUnits
        public ActionResult Index()
        {
            return View(Service.GetAll());
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
                Service.Create(goodUnit);
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
            GoodUnit goodUnit = Service.GetById((int)id);
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
                Service.Update(goodUnit);
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
            GoodUnit goodUnit = Service.GetById((int)id);
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
            GoodUnit goodUnit = Service.GetById((int)id);
            Service.Delete(goodUnit);
            return RedirectToAction("Index");
        }
    }
}
