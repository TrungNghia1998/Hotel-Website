using DataProvider.Model;
using Newtonsoft.Json;
using QuanLyKhachSan.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace QuanLyKhachSan.Areas.Admin.Controllers
{
    public class HoaDonController : BaseController
    {
        private QuanLyKhachSanEntities db = new QuanLyKhachSanEntities();

        // GET: Admin/HoaDon
        public ActionResult Index()
        {
            var tblHoaDons = db.tblHoaDons.Where(t => t.ma_tinh_trang == 2).Include(t => t.tblNhanVien).Include(t => t.tblPhieuDatPhong)
                .Include(t => t.tblTinhTrangHoaDon);
            double tong = 0;
            foreach (var item in tblHoaDons.ToList())
            {
                if (item.ma_tinh_trang == 2)
                {
                    tong += (double)item.tong_tien;
                }
            }
            ViewBag.tong_doanh_thu = String.Format("{0:0,0.00}", tong);
            return View(tblHoaDons.ToList());
        }

        public ActionResult TaoHoaDon(string ma_pdp, string hoten1, string hoten2, string hoten3, string hoten4, string tuoi1, string tuoi2, string tuoi3, string tuoi4)
        {
            if (ma_pdp == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {

                List<KhachHang> likh;
                tblPhieuDatPhong pt = db.tblPhieuDatPhongs.Find(Int32.Parse(ma_pdp));
                if (pt.thong_tin_khach_thue == null)
                {
                    likh = new List<KhachHang>();
                    likh.Add(new KhachHang("", ""));
                }
                else
                {
                    likh = JsonConvert.DeserializeObject<List<KhachHang>>(pt.thong_tin_khach_thue);
                }
                if (!hoten1.Equals(""))
                    likh.Add(new KhachHang(hoten1, tuoi1));
                if (!hoten2.Equals(""))
                    likh.Add(new KhachHang(hoten2, tuoi2));
                if (!hoten3.Equals(""))
                    likh.Add(new KhachHang(hoten3, tuoi3));
                if (!hoten4.Equals(""))
                    likh.Add(new KhachHang(hoten4, tuoi4));
                pt.thong_tin_khach_thue = JsonConvert.SerializeObject(likh);
                db.Entry(pt).State = EntityState.Modified;
                db.SaveChanges();

                tblHoaDon hd = new tblHoaDon();
                hd.ma_pdp = Int32.Parse(ma_pdp);
                hd.ma_tinh_trang = 1;
                try
                {
                    db.tblHoaDons.Add(hd);
                    tblPhieuDatPhong tgd = db.tblPhieuDatPhongs.Find(Int32.Parse(ma_pdp));
                    if (tgd == null)
                    {
                        return HttpNotFound();
                    }
                    tblPhong p = db.tblPhongs.Find(tgd.ma_phong);
                    if (p == null)
                    {
                        return HttpNotFound();
                    }
                    tgd.ma_tinh_trang = 2;
                    db.Entry(tgd).State = EntityState.Modified;
                    p.ma_tinh_trang = 2;
                    db.Entry(p).State = EntityState.Modified;
                    ViewBag.ngay_ra = tgd.ngay_ra;
                    db.SaveChanges();
                    ViewBag.Result = "success";
                }
                catch
                {
                    ViewBag.Result = "error";
                }
            }
            return View();
        }

        public ActionResult Add(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPhieuDatPhong tblPhieuDatPhong = db.tblPhieuDatPhongs.Find(id);
            if (tblPhieuDatPhong == null)
            {
                return HttpNotFound();
            }
            return View(tblPhieuDatPhong);
        }

        public ActionResult GoiDichVu(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoiDichVu model = new GoiDichVu();
            model.DanhSachDichVu = db.tblDichVus.Where(t => t.ton_kho > 0 && t.da_duoc_xoa == false && t.trang_thai == true).ToList();
            model.DanhSachDichVuDaDat = db.tblDichVuDaDats.Where(u => u.ma_hd == id).ToList();
                
            ViewBag.ma_hd = id;
            ViewBag.so_phong = db.tblHoaDons.Find(id).tblPhieuDatPhong.tblPhong.so_phong;

            return View(model);
        }

        public ActionResult ThemMoi(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPhieuDatPhong tblPhieuDatPhong = db.tblPhieuDatPhongs.Find(id);
            if (tblPhieuDatPhong == null)
            {
                return HttpNotFound();
            }
            return View(tblPhieuDatPhong);
        }

        public ActionResult XoaDichVu(string ma_hd, string del_id)
        {
            int soluong = 0;
            tblDichVuDaDat dichVuDaDat = db.tblDichVuDaDats.Find(Int32.Parse(del_id));
            soluong = (int)dichVuDaDat.so_luong;

            tblDichVu dichVu = db.tblDichVus.Where(x => x.ma_dv == dichVuDaDat.ma_dv).SingleOrDefault();
            dichVu.ton_kho = dichVu.ton_kho + soluong;

            db.tblDichVuDaDats.Remove(dichVuDaDat);
            db.SaveChanges();
            return RedirectToAction("GoiDichVu", "HoaDon", new { id = ma_hd });
        }

        public ActionResult ResultDoiPhong(string ma_pdp, string ma_phong_cu, string ma_phong_moi)
        {
            if (ma_pdp == null || ma_phong_cu == null || ma_phong_moi == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                tblPhieuDatPhong pdp = db.tblPhieuDatPhongs.Find(Int32.Parse(ma_pdp));
                tblPhong p = db.tblPhongs.Find(pdp.tblPhong.ma_phong);      // lấy thông tin phòng cũ
                p.ma_tinh_trang = 3;                                        // set phòng cũ về đang dọn
                db.Entry(p).State = EntityState.Modified;
                pdp.ma_phong = Int32.Parse(ma_phong_moi);                   // đổi phòng cũ sang mới
                p = db.tblPhongs.Find(Int32.Parse(ma_phong_moi));           // lấy thông tin phòng mới
                p.ma_tinh_trang = 2;                                        // set phòng mới về đang sd
                db.Entry(p).State = EntityState.Modified;
                db.Entry(pdp).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.result = "success";
            }
            catch (Exception e)
            {
                ViewBag.result = "error: " + e;
            }
            return View();
        }

        public ActionResult DoiPhong(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblHoaDon tblHoaDon = db.tblHoaDons.Find(id);
            if (tblHoaDon == null)
            {
                return HttpNotFound();
            }
            tblPhieuDatPhong pdp = db.tblPhieuDatPhongs.Find(tblHoaDon.ma_pdp);

            var li = db.tblPhongs.Where(t => t.ma_tinh_trang == 1 && !(db.tblPhieuDatPhongs.Where(m => (m.ma_tinh_trang == 1 || m.ma_tinh_trang == 2) && m.ngay_ra > DateTime.Now && m.ngay_vao < pdp.ngay_ra)).Select(m => m.ma_phong).ToList().Contains(t.ma_phong));
            ViewBag.ma_phong_moi = new SelectList(li, "ma_phong", "so_phong");
            return View(pdp);
        }

        public ActionResult Result(String ma_pdp, String hoten1, String hoten2, String hoten3, String hoten4, String tuoi1, String tuoi2, String tuoi3, String tuoi4)
        {
            if (ma_pdp == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                List<KhachHang> likh;
                tblPhieuDatPhong pt = db.tblPhieuDatPhongs.Find(Int32.Parse(ma_pdp));
                if (pt.thong_tin_khach_thue == null)
                {
                    likh = new List<KhachHang>();
                    likh.Add(new KhachHang("", ""));
                }
                else
                {
                    likh = JsonConvert.DeserializeObject<List<KhachHang>>(pt.thong_tin_khach_thue);
                }
                if (!hoten1.Equals(""))
                    likh.Add(new KhachHang(hoten1, tuoi1));
                if (!hoten2.Equals(""))
                    likh.Add(new KhachHang(hoten2, tuoi2));
                if (!hoten3.Equals(""))
                    likh.Add(new KhachHang(hoten3, tuoi3));
                if (!hoten4.Equals(""))
                    likh.Add(new KhachHang(hoten4, tuoi4));
                pt.thong_tin_khach_thue = JsonConvert.SerializeObject(likh);
                db.Entry(pt).State = EntityState.Modified;
                db.SaveChanges();

                tblHoaDon hd = new tblHoaDon();
                hd.ma_pdp = Int32.Parse(ma_pdp);
                hd.ma_tinh_trang = 1;
                try
                {
                    db.tblHoaDons.Add(hd);
                    tblPhieuDatPhong tgd = db.tblPhieuDatPhongs.Find(Int32.Parse(ma_pdp));
                    if (tgd == null)
                    {
                        return HttpNotFound();
                    }
                    tblPhong p = db.tblPhongs.Find(tgd.ma_phong);
                    if (p == null)
                    {
                        return HttpNotFound();
                    }
                    tgd.ma_tinh_trang = 2;
                    db.Entry(tgd).State = EntityState.Modified;
                    p.ma_tinh_trang = 2;
                    db.Entry(p).State = EntityState.Modified;
                    ViewBag.ngay_ra = tgd.ngay_ra;
                    db.SaveChanges();
                    ViewBag.Result = "success";
                }
                catch
                {
                    ViewBag.Result = "error";
                }
            }
            return View();
        }

        public ActionResult ResultGiaHan(string ma_pdp, string ngay_ra)
        {
            if (ma_pdp == null || ngay_ra == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                tblPhieuDatPhong pdp = db.tblPhieuDatPhongs.Find(Int32.Parse(ma_pdp));
                DateTime ngayra = DateTime.Parse(ngay_ra);
                pdp.ngay_ra = ngayra;
                ViewBag.result = "success";
                ViewBag.ngay_ra = ngay_ra;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ViewBag.result = "error: " + e;
            }
            return View();
        }

        public ActionResult GiaHanPhong(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblHoaDon tblHoaDon = db.tblHoaDons.Find(id);
            if (tblHoaDon == null)
            {
                return HttpNotFound();
            }
            tblPhieuDatPhong pdp = db.tblPhieuDatPhongs.Find(tblHoaDon.ma_pdp);
            string dt = null;
            try
            {
                DateTime d = (DateTime)db.tblPhieuDatPhongs.Where(t => t.ma_tinh_trang == 1 && t.ma_phong == pdp.tblPhong.ma_phong).Select(t => t.ngay_vao).OrderBy(t => t.Value).First();
                dt = d.ToString();
            }
            catch
            {

            }
            ViewBag.dateMax = dt;
            return View(pdp);
        }

        public ActionResult ThanhToan(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            tblHoaDon tblHoaDon = db.tblHoaDons.Find(id);
            if (tblHoaDon == null)
            {
                return HttpNotFound();
            }

            DateTime ngay_ra = DateTime.Now;
            DateTime ngay_vao = (DateTime)tblHoaDon.tblPhieuDatPhong.ngay_vao;
            DateTime ngay_du_kien = (DateTime)tblHoaDon.tblPhieuDatPhong.ngay_ra;

            DateTime dateStart = new DateTime(ngay_vao.Year, ngay_vao.Month, ngay_vao.Day, 12, 0, 0);
            DateTime dateEnd = new DateTime(ngay_ra.Year, ngay_ra.Month, ngay_ra.Day, 12, 0, 0);

            Double gia = (Double)tblHoaDon.tblPhieuDatPhong.tblPhong.tblLoaiPhong.gia;

            var songay = (dateEnd - dateStart).TotalDays;

            if (dateStart > ngay_vao)
                songay++;

            if (ngay_ra > dateEnd)
                songay++;

            var ti_le_phu_thu = tblHoaDon.tblPhieuDatPhong.tblPhong.tblLoaiPhong.ti_le_phu_thu;
            var so_ngay_phu_thu = Math.Abs(Math.Ceiling((ngay_ra - ngay_du_kien).TotalDays));

            System.Diagnostics.Debug.WriteLine("So ngay: " + so_ngay_phu_thu);
            System.Diagnostics.Debug.WriteLine("Gia: " + gia);
            System.Diagnostics.Debug.WriteLine("Ti le: :" + ti_le_phu_thu);

            var phuthu = so_ngay_phu_thu * gia * (ti_le_phu_thu / 100);
            ViewBag.phu_thu = phuthu;

            System.Diagnostics.Debug.WriteLine("Phu thu:" + phuthu);

            ViewBag.so_ngay_phu_thu = so_ngay_phu_thu;
            var tien_phong = songay * gia;
            ViewBag.tien_phong = tien_phong;
            ViewBag.so_ngay = songay;

            tblNhanVien nv = (tblNhanVien)Session["NhanVien"];
            if (nv != null)
            {
                ViewBag.ho_ten = nv.ho_ten;
            }

            List<tblDichVuDaDat> dsdv = db.tblDichVuDaDats.Where(u => u.ma_hd == id).ToList();

            ViewBag.list_dv = dsdv;
            double tongtiendv = 0;
            List<double> tongtien = new List<double>();
            foreach (var item in dsdv)
            {
                double t = (double)(item.so_luong * item.tblDichVu.gia);
                tongtiendv += t;
                tongtien.Add(t);
            }
            ViewBag.list_tt = tongtien;
            ViewBag.tien_dich_vu = tongtiendv;
            ViewBag.tong_tien = tien_phong + tongtiendv + phuthu;
            return View(tblHoaDon);
        }

        public ActionResult SuaDichVu(string ma_hd, string edit_id, string edit_so_luong)
        {
            if (ma_hd == null || edit_id == null || edit_so_luong == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblDichVuDaDat dsdv = db.tblDichVuDaDats.Find(Int32.Parse(edit_id));
            int sol = Int32.Parse(edit_so_luong);
            tblDichVu dv = db.tblDichVus.Find(dsdv.ma_dv);
            int del = (int)(sol - dsdv.so_luong);
            if (del > dv.ton_kho)
            {
                return RedirectToAction("GoiDichVu", "HoaDon", new { id = ma_hd });
            }
            else
            {
                dsdv.so_luong = sol;
                dv.ton_kho -= del;
                db.Entry(dsdv).State = EntityState.Modified;
                db.Entry(dv).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("GoiDichVu", "HoaDon", new { id = ma_hd });
        }

        public ActionResult XacNhanThanhToan(string ma_hd, string tien_phong, string tien_dich_vu, string phu_thu, string tong_tien)
        {
            if (ma_hd == null || tien_phong == null || tien_dich_vu == null || phu_thu == null || tong_tien == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                tblHoaDon hd = db.tblHoaDons.Find(Int32.Parse(ma_hd));
                tblNhanVien nv = (tblNhanVien)Session["NhanVien"];
                if (nv != null)
                    hd.ma_nv = nv.ma_nv;
           
                hd.tien_phong = Double.Parse(tien_phong);
                hd.tien_dich_vu = Double.Parse(tien_dich_vu);
                hd.phu_thu = Double.Parse(phu_thu);
                hd.tong_tien = Double.Parse(tong_tien);
                hd.ma_tinh_trang = 2;
                hd.ngay_tra_phong = DateTime.Now;
                db.Entry(hd).State = EntityState.Modified;

                tblPhong p = db.tblPhongs.Find(hd.tblPhieuDatPhong.ma_phong);
                p.ma_tinh_trang = 3;

                tblPhieuDatPhong pd = db.tblPhieuDatPhongs.Find(hd.tblPhieuDatPhong.ma_pdp);
                pd.ma_tinh_trang = 4;

                db.Entry(p).State = EntityState.Modified;
                db.Entry(pd).State = EntityState.Modified;
                db.SaveChanges();

                ViewBag.result = "success";
            }
            catch
            {
                ViewBag.result = "error";
            }
            ViewBag.ma_hd = ma_hd;
            return View();
        }

        public ActionResult XacNhanGoiDichVu(string ma_hd, string ma_dv, string so_luong)
        {
            if (ma_hd == null || ma_dv == null || so_luong == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int mahd = Int32.Parse(ma_hd);
            int madv = Int32.Parse(ma_dv);
            int soluong = Int32.Parse(so_luong);
            var danhSach = db.tblDichVuDaDats.Where(t => t.ma_hd == mahd).ToList();

            try
            {
                bool check = false;

                foreach (var item in danhSach)
                {
                    if (item.ma_dv == madv)
                    {
                        item.so_luong += soluong;
                        check = true;
                        break;
                    }
                }
                if (!check)
                {
                    tblDichVuDaDat dv = new tblDichVuDaDat();
                    dv.ma_hd = Int32.Parse(ma_hd);
                    dv.ma_dv = Int32.Parse(ma_dv);
                    dv.so_luong = Int32.Parse(so_luong);
                    db.tblDichVuDaDats.Add(dv);
                }
                tblDichVu dichvu = db.tblDichVus.Find(madv);
                dichvu.ton_kho -= soluong;
                db.SaveChanges();
            }
            catch
            {

            }
            return RedirectToAction("GoiDichVu", "HoaDon", new { id = ma_hd });
        }

        [HttpPost]
        public ActionResult Index(String beginDate, String endDate)
        {
            System.Diagnostics.Debug.WriteLine("your message here " + beginDate);
            List<tblHoaDon> dshd = new List<tblHoaDon>();
            String query = "select * from tblHoaDon where ma_tinh_trang=2 ";
            if (!beginDate.Equals(""))
                query += " and ngay_tra_phong >= '" + beginDate + "'";
            if (!endDate.Equals(""))
                query += " and ngay_tra_phong <= '" + endDate + "'";

            dshd = db.tblHoaDons.SqlQuery(query).ToList();
            double tong = 0;
            foreach (var item in dshd)
            {
                if (item.ma_tinh_trang == 2)
                {
                    tong += (double)item.tong_tien;
                }
            }
            ViewBag.tong_doanh_thu = tong.ToString("C");
            return View(dshd);
        }

        //[HttpGet]
        //public JsonResult LoadDataBill(string beginDate, string endDate, int page, int pageSize = 5)
        //{
        //    try
        //    {
        //        db.Configuration.ProxyCreationEnabled = false;
        //        //var model = db.tblDichVus.Where(x=>x.da_duoc_xoa == false).OrderBy(x=>x.ten_dv).Skip((page - 1) * pageSize).Take(pageSize);
        //        IQueryable<HoaDonKhachHangPhieuDatPhong> model = from phong in db.tblPhongs
        //                                                        join loaiphong in db.tblLoaiPhongs on phong.loai_phong equals loaiphong.loai_phong
        //                                                        join tang in db.tblTangs on phong.ma_tang equals tang.ma_tang
        //                                                        where phong.ma_tinh_trang < 5
        //                                                        select new PhongLoaiPhongTangModelView
        //                                                        {
        //                                                            Image = loaiphong.anh,
        //                                                            Name = phong.so_phong,
        //                                                            Type = loaiphong.mo_ta,
        //                                                            Level = tang.ten_tang,
        //                                                            ID = phong.ma_phong
        //                                                        };

        //        IQueryable<tblHoaDon> model = db.tblHoaDons;
        //        string query = "SELECT * from tblHoaDon where ma_tinh_trang = 2 ";

        //        if (!string.IsNullOrEmpty(beginDate) && !string.IsNullOrEmpty(endDate))
        //        {
        //            DateTime beginDateSelected = DateTime.Parse(beginDate);
        //            DateTime endDateSelected = DateTime.Parse(endDate);
        //            model = model.Where(x => (x.ma_tinh_trang == 2) && (x.ngay_tra_phong >= beginDateSelected) && (x.ngay_tra_phong <= endDateSelected));
        //        }

        //        int totalRow = model.Where(x => x.ma_tinh_trang == 2).Count();

        //        model = model.Where(x => x.ma_tinh_trang == 2).OrderBy(x => x.ngay_tra_phong).Skip((page - 1) * pageSize).Take(pageSize);

        //        return Json(new
        //        {
        //            data = model.Select(x => new
        //            {
        //                ID = x.ma_hd,
        //                Name = x.,
        //                Price = x.gia,
        //                Quantity = x.ton_kho,
        //                UnitPrice = x.don_vi,
        //                Status = x.trang_thai,
        //                ID = x.ma_dv
        //            }),
        //            total = totalRow,
        //            status = true
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception error)
        //    {
        //        return Json(new { success = false, message = error.ToString() }, JsonRequestBehavior.AllowGet);
        //    }
        //}
    }
}