using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace wmWebApp.Controllers
{
    public class Prototype01Controller : Controller
    {
        // GET: Prototype01
        public ActionResult QuanLyAgencies()
        {
            return View();
        }

        public ActionResult QuanLyEmployees()
        {
            return View();
        }

        public ActionResult QuanLyGood()
        {
            return View();
        }

        public ActionResult ChiNhanhOrderChiNhanh()
        {
            return View();
        }


        public ActionResult KhoTongOrderChiNhanh()
        {
            return View();
        }
        public ActionResult KhoTongOrderKhoTong()
        {
            return View();
        }


        public ActionResult BepTongOrder()
        {
            return View();
        }


        public ActionResult BepTongQuanLyDinhLuong()
        {
            return View();
        }
        public ActionResult BepTongNhapThuc()
        {
            return View();
        }
    }
}