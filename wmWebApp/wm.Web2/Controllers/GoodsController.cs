using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using wm.Model;
using wm.Service;
using wm.Web2.Models;

namespace wm.Web2.Controllers
{
    public class GoodsController : BaseController
    {
        private IGoodService Service { get; }
        readonly IGoodUnitService _goodUnitService;
        public GoodsController(ApplicationUserManager userManager, IGoodService service,
            IGoodUnitService goodUnitService):base(userManager)
        {
            Service = service;
            _goodUnitService = goodUnitService;
        }
        // GET: Goods
        public ActionResult Index()
        {
            return View();
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
        public ActionResult Create([Bind(Include = "Id,Name,NameASCII,UnitId,GoodType")] Good good)
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
        public ActionResult Edit([Bind(Include = "Id,Name,NameASCII,UnitId,GoodType")] Good good)
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

        [AllowAnonymous]
        [HttpPost]
        public ActionResult List(DTParameters param)
        {
            try
            {
                int recordsTotal;
                int recordsFiltered;

                string paramSortOrder = param.SortOrder;
                paramSortOrder = paramSortOrder.Replace("UnitName", "Unit.Name");
                //get sorted/paginated
                var resultSet = Service.ListDatatables(param.Search.Value, paramSortOrder, param.Start, param.Length, out recordsTotal, out recordsFiltered);

                var resultViewModel = resultSet.Select(e => new GoodDatatablesListViewModel
                {
                    id = e.Id,
                    Name = e.Name,
                    UnitName = e.Unit.Name,
                    GoodType = e.GoodType.ToString()
                });

                var dtResult = new DTResult<GoodDatatablesListViewModel>
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
