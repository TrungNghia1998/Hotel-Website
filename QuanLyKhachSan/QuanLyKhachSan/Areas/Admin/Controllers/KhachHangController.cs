using DataProvider.Model;
using QuanLyKhachSan.Areas.Admin.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace QuanLyKhachSan.Areas.Admin.Controllers
{
    public class KhachHangController : BaseController
    {
        private QuanLyKhachSanEntities db = new QuanLyKhachSanEntities();

        // GET: Admin/KhachHang
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblKhachHang tblKhachHang = db.tblKhachHangs.Find(int.Parse(id));
            if (tblKhachHang == null)
            {
                return HttpNotFound();
            }
            return View(tblKhachHang);
        }

        [HttpGet]
        public JsonResult LayChiTietKhachHang(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;

                var khachHang = db.tblKhachHangs.Find(id);

                //tblChucVu modelChucVu = db.tblChucVus.Where(x => x.ma_chuc_vu == taiKhoan.ma_chuc_vu).SingleOrDefault();

                KhachHangViewModel khachHangViewModel = new KhachHangViewModel();
                khachHangViewModel.ID = khachHang.ma_kh;
                khachHangViewModel.HoTen = khachHang.ho_ten;
                khachHangViewModel.DiaChi = khachHang.dia_chi;
                khachHangViewModel.SoDienThoai = khachHang.sdt;
                khachHangViewModel.TenTaiKhoan = khachHang.ten_dang_nhap;
                khachHangViewModel.Email = khachHang.mail;
                khachHangViewModel.MatKhau = khachHang.mat_khau;
                khachHangViewModel.SoCMND = khachHang.cmt;

                return Json(new
                {
                    data = khachHangViewModel,
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                return Json(new { status = false, message = error.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ma_kh,ten_dang_nhap,mat_khau,ho_ten,cmt,sdt,mail,diem")] tblKhachHang tblKhachHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblKhachHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblKhachHang);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public JsonResult DeleteKhachHang(int id)
        {
            var khachHang = db.tblKhachHangs.Find(id);
            khachHang.trang_thai = false;

            try
            {
                db.SaveChanges();
                return Json(new
                {
                    status = true
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false,
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        public JsonResult LuuKhachHang(KhachHangViewModel model)
        {
            bool status = false;
            string message = string.Empty;

            //tblLoaiPhong modelLoaiPhong = db.tblLoaiPhongs.Where(x => x.mo_ta == model.Type).SingleOrDefault();
            //tblTang modelTang = db.tblTangs.Where(x => x.ten_tang == model.Level).SingleOrDefault();
            //tblChucVu modelChucVu = db.tblChucVus.Where(x => x.chuc_vu == model.ChucVu).SingleOrDefault();

            tblKhachHang khachHang = new tblKhachHang();
            khachHang.ma_kh = model.ID;
            khachHang.ten_dang_nhap = model.TenTaiKhoan;
            khachHang.mat_khau = model.MatKhau;
            khachHang.ho_ten = model.HoTen;
            khachHang.cmt = model.SoCMND;
            khachHang.so_cmnd = model.SoCMND;
            khachHang.sdt = model.SoDienThoai;
            khachHang.cmt = model.SoCMND;
            khachHang.mail = model.Email;
            khachHang.diem = 0;
            khachHang.trang_thai = true;

            if (model.ID == 0)
            {
                db.tblKhachHangs.Add(khachHang);
                try
                {
                    db.SaveChanges();
                    status = true;
                }
                catch (Exception ex)
                {
                    status = false;
                    message = ex.Message;
                }
            }
            else
            {
                //Đang cập nhật dữ liệu
                var entity = db.tblKhachHangs.Find(model.ID);
                entity.ho_ten = model.HoTen;
                entity.ten_dang_nhap = model.TenTaiKhoan;
                entity.mat_khau = model.MatKhau;               
                entity.cmt = model.SoCMND;
                entity.so_cmnd = model.SoCMND;
                entity.cmt = model.SoCMND;
                entity.sdt = model.SoDienThoai;
                entity.mail = model.Email;
            
                try
                {
                    db.SaveChanges();
                    status = true;
                }
                catch (Exception ex)
                {
                    status = false;
                    message = ex.Message;
                }
            }

            return Json(new
            {
                status = status,
                message = message
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "ten_dang_nhap,mat_khau,ho_ten,cmt,sdt,mail")] tblKhachHang tblKhachHang)
        {
            if (ModelState.IsValid)
            {
                if (db.tblKhachHangs.Find(tblKhachHang.ma_kh) == null)
                {
                    db.tblKhachHangs.Add(tblKhachHang);
                    db.SaveChanges();
                    return RedirectToAction("Index", "KhachHang");
                }
                else
                {
                    ModelState.AddModelError("", "Login data is incorrect!");
                }
            }

            return View(tblKhachHang);
        }


        [HttpPost]
        public JsonResult SaveData(KhachHangViewModel customer)
        {
            bool status = false;
            string message = string.Empty;

            tblKhachHang khachHang = new tblKhachHang();
            khachHang.cmt = customer.SoCMND;
            khachHang.ho_ten = customer.HoTen;
            khachHang.mat_khau = customer.MatKhau;
            khachHang.mail = customer.Email;
            khachHang.sdt = customer.SoDienThoai;
            khachHang.ten_dang_nhap = customer.TenTaiKhoan;
            khachHang.diem = 0;

            //Thêm mới
            db.tblKhachHangs.Add(khachHang);
            try
            {
                db.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                message = ex.Message;
            }

            return Json(new
            {
                status = status,
                message = message
            });
        }

        [HttpGet]
        public JsonResult LoadDataCustomer(string name, int page, int pageSize = 5)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                
                IQueryable<tblKhachHang> model = db.tblKhachHangs;

                if (!string.IsNullOrEmpty(name))
                {
                    model = model.Where(x => x.ho_ten.Contains(name) && x.trang_thai == true);
                }

                int totalRow = model.Count();

                model = model.OrderBy(x => x.ho_ten).Where(x=>x.trang_thai == true).Skip((page - 1) * pageSize).Take(pageSize);

                return Json(new
                {
                    data = model.Select(x => new
                    {
                        ID = x.ma_kh,
                        Username = x.ten_dang_nhap,
                        Name = x.ho_ten,
                        Email = x.mail,
                        Address = x.dia_chi,
                        Phone = x.sdt,
                        CMND = x.cmt
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