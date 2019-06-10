using DataProvider.Model;
using QuanLyKhachSan.Areas.Admin.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace QuanLyKhachSan.Areas.Admin.Controllers
{
    public class DichVuController : BaseController
    {
        private QuanLyKhachSanEntities db = new QuanLyKhachSanEntities();

        // GET: Admin/DichVu
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetDetail(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var service = db.tblDichVus.Find(id);
                ServiceViewModel serviceViewModel = new ServiceViewModel();
                serviceViewModel.ID = service.ma_dv;
                serviceViewModel.Image = service.anh;
                serviceViewModel.Name = service.ten_dv;
                serviceViewModel.Price = service.gia;
                serviceViewModel.Quantity = service.ton_kho;
                serviceViewModel.Status = service.trang_thai;
                serviceViewModel.Type = service.don_vi;

                return Json(new
                {
                    data = serviceViewModel,
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                return Json(new { status = false, message = error.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GoiDichVu(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPhong phong = new tblPhong();
            phong = db.tblPhongs.Find(id);

            tblPhieuDatPhong phieudatphong = new tblPhieuDatPhong();
            phieudatphong = db.tblPhieuDatPhongs.Where(x => x.ma_phong == phong.ma_phong && x.ma_tinh_trang == 2 && phong.ma_tinh_trang == 2).FirstOrDefault();

            tblHoaDon hoadon = new tblHoaDon();
            hoadon = db.tblHoaDons.Where(x => x.ma_pdp == phieudatphong.ma_pdp && phieudatphong.ma_tinh_trang == 2 && x.ma_tinh_trang == 1).SingleOrDefault();

            //int ma_hd = db.tblHoaDons.Where(x => x.ma_phong == id && u.ma_tinh_trang == 2).First().ma_hd;
            return RedirectToAction("GoiDichVu", "HoaDon", new { id = hoadon.ma_hd });
        }

        [HttpPost]
        public JsonResult SaveDataImage(ImageViewModel model)
        {
            var file = model.ImageFile;

            string message = string.Empty;
            var status = false;

            try
            {
                if (file != null) //Có hình ảnh truyền lên
                {
                    status = true;
                    //var fileName = Path.GetFileName(file.FileName);
                    //var extention = Path.GetExtension(file.FileName);
                    //var filenamewithoutextension = Path.GetFileNameWithoutExtension(file.FileName);

                    file.SaveAs(Server.MapPath("/Content/Images/DichVu/" + file.FileName));

                    return Json(new
                    {
                        fileName = file.FileName,
                        status = status,
                        message = message
                    });
                }
                else
                {
                    status = false;
                    return Json(new
                    {
                        status = status
                    });
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Json(new
                {
                    message = message
                });
            }
        }

        [HttpPost]
        public JsonResult SaveDataServiceNotImage(ServiceViewModel service)
        {
            bool status = false;
            string message = string.Empty;

            if(service.ID == 0)
            {
                tblDichVu dichVu = new tblDichVu();
                dichVu.ma_dv = service.ID;
                dichVu.gia = service.Price;
                dichVu.ten_dv = service.Name;
                dichVu.ton_kho = service.Quantity;
                dichVu.trang_thai = true;
                dichVu.da_duoc_xoa = false;
                dichVu.don_vi = service.Type;
                dichVu.anh = null;

                db.tblDichVus.Add(dichVu);

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
                var entity = db.tblDichVus.Find(service.ID);

                entity.ten_dv = service.Name;
                entity.ton_kho = service.Quantity;
                entity.gia = service.Price;            
                entity.don_vi = service.Type;
                

                //entity.CreatedDate = DateTime.Now;
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
        public JsonResult SaveDataService(ServiceViewModel service)
        {
            bool status = false;
            string message = string.Empty;

            tblDichVu dichVu = new tblDichVu();
            dichVu.ma_dv = service.ID;
            dichVu.gia = service.Price;
            dichVu.ten_dv = service.Name;
            dichVu.ton_kho = service.Quantity;
            dichVu.trang_thai = true;
            dichVu.da_duoc_xoa = false;
            dichVu.don_vi = service.Type;
            dichVu.anh = service.Image;

            if (service.ID == 0)
            {
                //Thêm mới
                db.tblDichVus.Add(dichVu);
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
                var entity = db.tblDichVus.Find(service.ID);
                entity.ten_dv = service.Name;
                entity.ton_kho = service.Quantity;
                entity.gia = service.Price;
                //entity.trang_thai = service.Status;
                entity.don_vi = service.Type;
                entity.anh = service.Image;

                //entity.CreatedDate = DateTime.Now;
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
        public JsonResult DeleteService(int id)
        {
            //db.Configuration.ProxyCreationEnabled = false;
            var service = db.tblDichVus.Find(id);
            service.da_duoc_xoa = true;

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
        public JsonResult LoadDataService(string name, string status, int page, int pageSize = 5)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                //var model = db.tblDichVus.Where(x=>x.da_duoc_xoa == false).OrderBy(x=>x.ten_dv).Skip((page - 1) * pageSize).Take(pageSize);
                IQueryable<tblDichVu> model = db.tblDichVus;

                if (!string.IsNullOrEmpty(name))
                {                   
                    model = model.Where(x=>(x.da_duoc_xoa == false) && (x.ten_dv.Contains(name)));
                }

                if (!string.IsNullOrEmpty(status))
                {
                    var statusBool = bool.Parse(status);
                    model = model.Where(x => (x.da_duoc_xoa == false) && (x.trang_thai == statusBool));
                }

                int totalRow = model.Where(x=>x.da_duoc_xoa == false).Count();

                model = model.Where(x => x.da_duoc_xoa == false).OrderBy(x => x.ten_dv).Skip((page - 1) * pageSize).Take(pageSize);
              
                return Json(new
                {
                    data = model.Select(x => new
                    {
                        Image = x.anh,
                        Name = x.ten_dv,
                        Price = x.gia,
                        Quantity = x.ton_kho,
                        UnitPrice = x.don_vi,
                        Status = x.trang_thai,
                        ID = x.ma_dv
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

        [HttpPost]
        public JsonResult UpdatePrice(int ID, float Price)
        {
            try
            {               
                var model = db.tblDichVus.Find(ID);
                model.gia = Price;
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
        public JsonResult UpdateQuantity(int ID, int Quantity)
        {
            try
            {
                var model = db.tblDichVus.Find(ID);
                model.ton_kho = Quantity;
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
        public JsonResult ChangeStatus(int id)
        {
            var service = db.tblDichVus.Find(id);
            try
            {
                service.trang_thai = !service.trang_thai;
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
    }
}