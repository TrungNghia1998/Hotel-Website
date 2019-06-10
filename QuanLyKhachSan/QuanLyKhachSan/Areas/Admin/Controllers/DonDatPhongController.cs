using DataProvider.Model;
using Newtonsoft.Json;
using QuanLyKhachSan.Areas.Admin.Models;
using QuanLyKhachSan.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace QuanLyKhachSan.Areas.Admin.Controllers
{
    public class DonDatPhongController : Controller
    {
        private QuanLyKhachSanEntities db = new QuanLyKhachSanEntities();

        // GET: Admin/DonDatPhong
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DatOffline()
        {
            return View();
        }

        public ActionResult SelectRoom(string dateE)
        {
            if (dateE == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.ma_kh = new SelectList(db.tblKhachHangs, "ma_kh", "ho_ten");
            DateTime ngay_ra = (DateTime.Parse(dateE)).AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute);
            ViewBag.ngay_ra = ngay_ra;
            var s = db.tblPhongs.Where(t => !(db.tblPhieuDatPhongs.Where(m => (m.ma_tinh_trang == 1 || m.ma_tinh_trang == 2) && (m.ngay_ra > DateTime.Now && m.ngay_ra < ngay_ra))).Select(m => m.ma_phong).ToList().Contains(t.ma_phong) && t.ma_tinh_trang == 1);
            ViewBag.ma_phong = new SelectList(s, "ma_phong", "so_phong");
            ViewBag.ma_tinh_trang = new SelectList(db.tblTinhTrangPhieuDatPhongs, "ma_tinh_trang", "tinh_trang");
            return View();
        }

        private void AutoHuyPhieuDatPhong()
        {
            var datenow = DateTime.Now;
            var tblPhieuDatPhongs = db.tblPhieuDatPhongs.Where(u => u.ma_tinh_trang == 1).Include(t => t.tblKhachHang).Include(t => t.tblPhong).Include(t => t.tblTinhTrangPhieuDatPhong).ToList();
            foreach (var item in tblPhieuDatPhongs)
            {
                System.Diagnostics.Debug.WriteLine((item.ngay_vao - datenow).Value.Days);
                if ((item.ngay_vao - datenow).Value.Days < 0)
                {
                    item.ma_tinh_trang = 3;
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        public ActionResult DatOnline()
        {
            AutoHuyPhieuDatPhong();
            var tblPhieuDatPhongs = db.tblPhieuDatPhongs.Where(t => t.ma_tinh_trang == 1 && t.ngay_dat.Value.Day == DateTime.Now.Day && t.ngay_dat.Value.Month == DateTime.Now.Month && t.ngay_dat.Value.Year == DateTime.Now.Year).Include(t => t.tblKhachHang).Include(t => t.tblPhong).Include(t => t.tblTinhTrangPhieuDatPhong);
            return View(tblPhieuDatPhongs.ToList());
        }

        [HttpGet]
        public JsonResult XacNhanDonDatPHong(int idPhieuDatPhong, int idPhong)
        {
            try
            {
                bool status = true;

                db.Configuration.ProxyCreationEnabled = false;

                tblPhong phong = new tblPhong();
                phong = db.tblPhongs.Find(idPhong);

                tblLoaiPhong loaiphong = new tblLoaiPhong();
                loaiphong = db.tblLoaiPhongs.Find(phong.loai_phong);

                tblPhieuDatPhong phieuDatPhong = new tblPhieuDatPhong();
                phieuDatPhong = db.tblPhieuDatPhongs.Find(idPhieuDatPhong);
          
                if(phong.ma_tinh_trang != 1)
                {
                    status = false;
                    return Json(new
                    {                  
                        status = status
                    }, JsonRequestBehavior.AllowGet);
                }

                phong.ma_tinh_trang = 2;
                //string = Session[ThongSoCoDinh.USERSESSION].ToString();
                tblHoaDon newHoaDon = new tblHoaDon();
                newHoaDon.ma_nv = (int)Session["NV"];
                newHoaDon.ma_pdp = idPhieuDatPhong;
                newHoaDon.ma_tinh_trang = 1;
                newHoaDon.tien_phong = loaiphong.gia;
                db.tblHoaDons.Add(newHoaDon);

                phieuDatPhong.ma_tinh_trang = 2;

                db.SaveChanges();
                
                return Json(new
                {                 
                    status = true,
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                return Json(new { success = false, message = error.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string radSelect, [Bind(Include = "ma_pdp,ma_kh,ngay_dat,ngay_vao,ngay_ra,ma_phong,ma_tinh_trang")] tblPhieuDatPhong tblPhieuDatPhong, [Bind(Include = "hoten,socmt,tuoi,sodt")] KhachHang kh)
        {
            System.Diagnostics.Debug.WriteLine("SS :" + radSelect);
            if (radSelect.Equals("rad2"))
            {
                tblPhieuDatPhong.ma_kh = null;
                List<KhachHang> likh = new List<KhachHang>();
                likh.Add(kh);
                string ttkh = JsonConvert.SerializeObject(likh);
                tblPhieuDatPhong.thong_tin_khach_thue = ttkh;
            }

            tblPhieuDatPhong.ma_tinh_trang = 1;
            tblPhieuDatPhong.ngay_vao = DateTime.Now;
            tblPhieuDatPhong.ngay_dat = DateTime.Now;
           
            db.tblPhieuDatPhongs.Add(tblPhieuDatPhong);
            db.SaveChanges();
            int ma = tblPhieuDatPhong.ma_pdp;
            return RedirectToAction("ThemMoi", "HoaDon", new { id = ma });

            ViewBag.ma_kh = new SelectList(db.tblKhachHangs, "ma_kh", "ma_kh", tblPhieuDatPhong.ma_kh);
            ViewBag.ma_phong = new SelectList(db.tblPhongs, "ma_phong", "so_phong", tblPhieuDatPhong.ma_phong);
            ViewBag.ma_tinh_trang = new SelectList(db.tblTinhTrangPhieuDatPhongs, "ma_tinh_trang", "tinh_trang", tblPhieuDatPhong.ma_tinh_trang);
            return View(tblPhieuDatPhong);
        }

        public ActionResult LoadDetailDonDatPhong(int id)
        {
            using (QuanLyKhachSanEntities db = new QuanLyKhachSanEntities())
            {
                ThongTinChiTietDonDatPhong model = (from phieudatphong in db.tblPhieuDatPhongs
                            join phong in db.tblPhongs on phieudatphong.ma_phong equals phong.ma_phong
                            join loaiphong in db.tblLoaiPhongs on phong.loai_phong equals loaiphong.loai_phong
                            join khachhang in db.tblKhachHangs on phieudatphong.ma_kh equals khachhang.ma_kh
                            where phieudatphong.ma_pdp == id
                            select new ThongTinChiTietDonDatPhong
                            {
                                MaPhieuDatPhong = phieudatphong.ma_pdp,
                                MaPhong = phong.ma_phong,
                                NgayDen = phieudatphong.ngay_vao,
                                NgayDat = phieudatphong.ngay_dat,
                                NgayDi = phieudatphong.ngay_ra,
                                TenPhong = phong.so_phong,
                                LoaiPhong = loaiphong.mo_ta,
                                GiaPhong = loaiphong.gia,
                                TenKhachHang = khachhang.ho_ten,
                                NgaySinh = khachhang.ngay_sinh,
                                GioiTinh = khachhang.gioi_tinh,
                                DiaChi = khachhang.dia_chi,
                                SoDienThoai = khachhang.sdt,
                                Email = khachhang.mail,
                                CMND = khachhang.cmt
                            }).FirstOrDefault();

                DateTime ngaySinh = (DateTime)model.NgaySinh;
                string ngaySinhString = String.Format("{0:dd/MM/yyyy}", ngaySinh);

                model.NgaySinhNotTime = ngaySinhString;

                DateTime ngayDen = (DateTime)model.NgayDen;
                string ngayDenString = String.Format("{0:dd/MM/yyy hh:mm}", ngayDen);

                model.NgayDenFormat = ngayDenString;

                DateTime ngayDi = (DateTime)model.NgayDi;
                string ngayDiString = String.Format("{0:dd/MM/yyy hh:mm}", ngayDi);

                model.NgayDiFormat = ngayDiString;

                DateTime ngayDat = (DateTime)model.NgayDat;
                string ngayDatString = String.Format("{0:dd/MM/yyy hh:mm}", ngayDat);

                model.NgayDatFormat = ngayDatString;

                return View(model);
            }        
        }

        [HttpGet]
        public JsonResult LoadData(string name, int page, int pageSize = 5)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                //var model = db.tblDichVus.Where(x=>x.da_duoc_xoa == false).OrderBy(x=>x.ten_dv).Skip((page - 1) * pageSize).Take(pageSize);
                IQueryable<PhieuDatPhongKhachHangTinhTrang> model = from phieudatphong in db.tblPhieuDatPhongs
                                                                    join khachhang in db.tblKhachHangs on phieudatphong.ma_kh equals khachhang.ma_kh
                                                                    join tinhtrang in db.tblTinhTrangPhieuDatPhongs on phieudatphong.ma_tinh_trang equals tinhtrang.ma_tinh_trang
                                                                    select new PhieuDatPhongKhachHangTinhTrang
                                                                    {
                                                                        TenKhachHang = khachhang.ho_ten,
                                                                        NgayDen = phieudatphong.ngay_vao,
                                                                        NgayDi = phieudatphong.ngay_ra,
                                                                        NgayDat = phieudatphong.ngay_dat,
                                                                        TinhTrang = tinhtrang.tinh_trang,
                                                                        MaPhieuDatPhong = phieudatphong.ma_pdp,
                                                                        NgayDenString = phieudatphong.ngay_vao.ToString(),
                                                                        NgayDatString = phieudatphong.ngay_dat.ToString(),
                                                                        NgayDiString = phieudatphong.ngay_ra.ToString()
                                                                    };

                if (!string.IsNullOrEmpty(name))
                {
                    //model = model.Where(x=>(x.Name.Contains(name)) || x.Type.Contains(name) || x.ID.Equals(name) || x.Level.Contains(name));
                    model = model.Where(x => (x.TenKhachHang.Contains(name)));
                }

                int totalRow = model.Count();

                model = model.OrderBy(x => x.NgayDen).Skip((page - 1) * pageSize).Take(pageSize);

                return Json(new
                {
                    data = model.Select(x => new
                    {
                        TenKhachHang = x.TenKhachHang,
                        NgayDen = x.NgayDenString,
                        NgayDi = x.NgayDiString,
                        NgayDat = x.NgayDatString,
                        TinhTrang = x.TinhTrang,
                        MaPhieuDatPhong = x.MaPhieuDatPhong
                    }),
                    total = totalRow,
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                return Json(new { success = false, message = error.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}