using DataProvider.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace QuanLyKhachSan.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        private QuanLyKhachSanEntities db = new QuanLyKhachSanEntities();

        // GET: Admin/Home
        public ActionResult Index()
        {
            LoadThongKe();
            DateTime datenow = DateTime.Parse(DateTime.Now.ToShortDateString());
            ViewBag.title_char1 = "Biểu đồ doanh thu 15 ngày gần nhất";
            Chart1(datenow.AddDays(-14), datenow);
            Chart2(datenow);
            Chart3();
            return View();
        }

        [HttpPost]
        public ActionResult Index(String start, String end)
        {
            if (start == "" || end == "")
                return RedirectToAction("Index", "Home");
            LoadThongKe();
            DateTime datenow = DateTime.Parse(DateTime.Now.ToShortDateString());
            DateTime dateS = DateTime.Parse(start);
            DateTime dateE = DateTime.Parse(end);
            ViewBag.title_char1 = "Biểu đồ doanh thu từ ngày " + start + " đến ngày " + end;
            Chart1(dateS, dateE);
            Chart2(datenow);
            Chart3();
            return View();
        }

        public ActionResult DatPhong()
        {
            return View();
        }

        public ActionResult TraPhong(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPhieuDatPhong phieudatphong = new tblPhieuDatPhong();
            phieudatphong = db.tblPhieuDatPhongs.Where(x => x.ma_phong == id && x.ma_tinh_trang == 2).SingleOrDefault();

            tblHoaDon MaHoaDon = db.tblHoaDons.Where(x => x.ma_pdp == phieudatphong.ma_pdp && x.ma_tinh_trang == 1).SingleOrDefault();

            return RedirectToAction("ThanhToan", "HoaDon", new { id = MaHoaDon.ma_hd });
        }

        public ActionResult ListPhongDangHoatDong()
        {
            var list = db.tblHoaDons.Where(u => u.ma_tinh_trang == 1).Include(t => t.tblNhanVien).Include(t => t.tblPhieuDatPhong).Include(t => t.tblTinhTrangHoaDon);
            return View(list.ToList());
        }

        public ActionResult DanhSachPhongGoiDichVu()
        {
            var list = db.tblHoaDons.Where(u => u.ma_tinh_trang == 1).Include(t => t.tblNhanVien).Include(t => t.tblPhieuDatPhong).Include(t => t.tblTinhTrangHoaDon);
            return View(list.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CaNhan([Bind(Include = "ma_nv,ho_ten,ngay_sinh,dia_chi,sdt,tai_khoan,mat_khau,ma_chuc_vu")] tblNhanVien tblNhanVien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblNhanVien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("/");
            }
            ViewBag.ma_chuc_vu = new SelectList(db.tblChucVus, "ma_chuc_vu", "chuc_vu", tblNhanVien.ma_chuc_vu);
            return View(tblNhanVien);
        }

        public ActionResult CaNhan()
        {
            tblNhanVien nv = (tblNhanVien)Session["NhanVien"];
            if (nv != null)
            {
                nv = db.tblNhanViens.Find(nv.ma_nv);
                ViewBag.ma_chuc_vu = new SelectList(db.tblChucVus, "ma_chuc_vu", "chuc_vu", nv.ma_chuc_vu);
                return View(nv);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult DonPhongXong(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPhong p = db.tblPhongs.Where(u => u.ma_phong == id).First();
            p.ma_tinh_trang = 1;
            db.Entry(p).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("TatCaPhong", "Home");
        }

        public ActionResult TatCaPhong()
        {
            int so_phong_trong = 0, so_phong_sd = 0, so_phong_don = 0;
            var listPhongs = db.tblPhongs.Where(t => t.ma_tinh_trang < 5).ToList();
            foreach (var item in listPhongs)
            {
                if (item.ma_tinh_trang == 1)
                    so_phong_trong++;
                else if (item.ma_tinh_trang == 2)
                    so_phong_sd++;
                else
                    so_phong_don++;
            }
            ViewBag.so_phong_trong = so_phong_trong;
            ViewBag.so_phong_sd = so_phong_sd;
            ViewBag.so_phong_don = so_phong_don;
            return View(listPhongs);
        }

        private void LoadThongKe()
        {
            DateTime date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var tong = db.tblHoaDons.Where(t => t.ma_tinh_trang == 2 && t.ngay_tra_phong >= firstDayOfMonth && t.ngay_tra_phong <= lastDayOfMonth).Sum(t => t.tong_tien);
            if (tong != null)
                ViewBag.tien_ht = String.Format("{0:0,0.00}", tong);
            else
                ViewBag.tien_ht = "0";

            ViewBag.so_hoa_don = db.tblHoaDons.Count();
            ViewBag.so_phieu_dat_phong = db.tblPhieuDatPhongs.Where(u => u.ma_tinh_trang == 1).Count();
            ViewBag.so_phong_dang_dat = db.tblPhongs.Where(u => u.ma_tinh_trang == 2).Count();
            ViewBag.so_dich_vu = db.tblDichVus.Count();
        }

        private void Chart1(DateTime start, DateTime end)
        {
            List<Double> C1sl = new List<Double>();
            List<String> C1name = new List<String>();
            int num = (end - start).Days;
            double tong = 0;
            for (int i = 0; i <= num; i++)
            {
                DateTime f1 = end.AddDays(-num + i);
                DateTime f2 = f1.AddDays(1);
                var q = db.tblHoaDons.Where(t => t.ma_tinh_trang == 2 && t.ngay_tra_phong > f1 && t.ngay_tra_phong < f2).Sum(t => t.tong_tien);
                if (q == null)
                    q = 0;
                tong += (double)q;
                C1sl.Add((Double)q);
                C1name.Add(f1.Day.ToString() + " / " + f1.Month.ToString());
            }
            ViewBag.tong_tien = "Tổng doanh thu từ ngày " + start.ToShortDateString() + " tới ngày " + end.ToShortDateString() + " là " + String.Format("{0:0,0.00}", tong) + " VND";
            ViewBag.C1sl = C1sl;
            ViewBag.C1name = C1name;
        }

        private void Chart2(DateTime end)
        {
            List<int> C2sl = new List<int>();
            List<String> C2name = new List<String>();
            System.Diagnostics.Debug.WriteLine("DAY 2: " + end);
            for (int i = 1; i <= 7; i++)
            {
                DateTime f1 = end.AddDays(-7 + i);
                DateTime f2 = f1.AddDays(1);
                var q = db.tblHoaDons.Where(t => t.tblPhieuDatPhong.ngay_vao >= f1 && t.tblPhieuDatPhong.ngay_vao < f2).Count();
                C2sl.Add(q);
                C2name.Add(f1.Day.ToString() + " / " + f1.Month.ToString());
            }
            ViewBag.C2sl = C2sl;
            ViewBag.C2name = C2name;
        }

        private void Chart3()
        {
            var s = db.tblDichVuDaDats.GroupBy(t => t.tblDichVu.ten_dv).Select(t => new { ten_dv = t.Key, total = t.Sum(i => i.so_luong) });
            List<String> name = new List<String>();
            List<int> total = new List<int>();
            foreach (var group in s)
            {
                System.Diagnostics.Debug.WriteLine("Ma Dv: " + group.ten_dv + " | SL: " + group.total);
                name.Add((String)group.ten_dv);
                total.Add((int)group.total);
            }
            ViewBag.name = name;
            ViewBag.total = total;
        }
    }
}