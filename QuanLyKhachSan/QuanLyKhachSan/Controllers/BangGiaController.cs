using DataProvider.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyKhachSan.Controllers
{
    public class BangGiaController : Controller
    {
        private QuanLyKhachSanEntities db = new QuanLyKhachSanEntities();

        // GET: BangGia
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GiaPhong()
        {
            return View(db.tblLoaiPhongs.ToList());
        }

        public ActionResult GiaDichVu()
        {
            return View(db.tblDichVus.ToList());
        }
    }
}