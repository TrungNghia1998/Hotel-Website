using DataProvider.Model;
using QuanLyKhachSan.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyKhachSan.Areas.Admin.Controllers
{
    public class TaiKhoanController : BaseController
    {
        private QuanLyKhachSanEntities db = new QuanLyKhachSanEntities();

        // GET: Admin/TaiKhoan
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LayChucVu()
        {
            bool status = false;
            string message = string.Empty;

            try
            {
                db.Configuration.ProxyCreationEnabled = false;

                var chucVu = db.tblChucVus.ToList();               
                status = true;

                return Json(new
                {
                    chucvu = chucVu.Select(x => new
                    {
                        ID = x.ma_chuc_vu,
                        Name = x.chuc_vu
                    }),
                    status = status
                });
            }
            catch (Exception ex)
            {
                status = false;
                return Json(new
                {
                    message = ex.Message,
                    status = status
                });
            }
        }

        [HttpGet]
        public JsonResult LayChiTietTaiKhoan(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;

                var taiKhoan = db.tblNhanViens.Find(id);
                
                tblChucVu modelChucVu = db.tblChucVus.Where(x => x.ma_chuc_vu == taiKhoan.ma_chuc_vu).SingleOrDefault();

                TaiKhoanViewModel taiKhoanViewModel = new TaiKhoanViewModel();
                taiKhoanViewModel.ID = taiKhoan.ma_nv;
                taiKhoanViewModel.HoTen = taiKhoan.ho_ten;
                taiKhoanViewModel.DiaChi = taiKhoan.dia_chi;
                taiKhoanViewModel.SoDienThoai = taiKhoan.sdt;
                taiKhoanViewModel.TenTaiKhoan = taiKhoan.tai_khoan;
                taiKhoanViewModel.Email = taiKhoan.mail;
                taiKhoanViewModel.MatKhau = taiKhoan.mat_khau;
                taiKhoanViewModel.ChucVu = modelChucVu.chuc_vu;
             
                return Json(new
                {
                    data = taiKhoanViewModel,
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                return Json(new { status = false, message = error.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult LuuTaiKhoan(TaiKhoanViewModel model)
        {
            bool status = false;
            string message = string.Empty;

            //tblLoaiPhong modelLoaiPhong = db.tblLoaiPhongs.Where(x => x.mo_ta == model.Type).SingleOrDefault();
            //tblTang modelTang = db.tblTangs.Where(x => x.ten_tang == model.Level).SingleOrDefault();
            tblChucVu modelChucVu = db.tblChucVus.Where(x => x.chuc_vu == model.ChucVu).SingleOrDefault();       

            tblNhanVien nhanVien = new tblNhanVien();
            nhanVien.ma_nv = model.ID;
            nhanVien.ho_ten = model.HoTen;
            nhanVien.dia_chi = model.DiaChi;
            nhanVien.sdt = model.SoDienThoai;
            nhanVien.tai_khoan = model.TenTaiKhoan;
            nhanVien.mat_khau = model.MatKhau;
            nhanVien.ma_chuc_vu = modelChucVu.ma_chuc_vu;
            nhanVien.trang_thai_tai_khoan = true;
            nhanVien.mail = model.Email;

            if (model.ID == 0)
            {
                db.tblNhanViens.Add(nhanVien);
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
                var entity = db.tblNhanViens.Find(model.ID);
                entity.ho_ten = model.HoTen;
                entity.dia_chi =  model.DiaChi;
                entity.sdt = model.SoDienThoai;
                entity.tai_khoan = model.TenTaiKhoan;
                entity.mat_khau = model.MatKhau;
                entity.ma_chuc_vu = modelChucVu.ma_chuc_vu;
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
        public JsonResult DeleteAccount(int id)
        {
            var taiKhoan = db.tblNhanViens.Find(id);
            taiKhoan.trang_thai_tai_khoan = false;

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

        [HttpGet]
        public JsonResult LoadDataAccount(string name, int page, int pageSize = 5)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                
                IQueryable<TaiKhoanChucVu> model = from nhanvien in db.tblNhanViens
                                                   join chucvu in db.tblChucVus on nhanvien.ma_chuc_vu equals chucvu.ma_chuc_vu
                                                   where nhanvien.trang_thai_tai_khoan == true
                                                   select new TaiKhoanChucVu
                                                   {
                                                       Username = nhanvien.tai_khoan,
                                                       Name = nhanvien.ho_ten,
                                                       Role = chucvu.chuc_vu,
                                                       DateOfBirth = nhanvien.ngay_sinh,
                                                       Email = nhanvien.mail,
                                                       ID = nhanvien.ma_nv,
                                                       Phone = nhanvien.sdt
                                                       //DateString = (nhanvien.ngay_sinh).ToString("dd/MM/YYYY", CultureInfo.InvariantCulture)
                                                   };

                if (!string.IsNullOrEmpty(name))
                {
                    //model = model.Where(x=>(x.Name.Contains(name)) || x.Type.Contains(name) || x.ID.Equals(name) || x.Level.Contains(name));
                    model = model.Where(x => (x.Name.Contains(name)));
                }

                int totalRow = model.Count();

                model = model.OrderBy(x => x.ID).Skip((page - 1) * pageSize).Take(pageSize);

                return Json(new
                {
                    data = model.Select(x => new
                    {
                        Username = x.Username,
                        Name = x.Name,
                        Email = x.Email,
                        Role = x.Role,
                        Phone = x.Phone,
                        DateOfBirth = x.DateOfBirth,
                        //DateString = x.DateString,
                        ID = x.ID
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