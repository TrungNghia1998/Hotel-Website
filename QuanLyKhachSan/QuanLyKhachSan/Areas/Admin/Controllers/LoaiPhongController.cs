using DataProvider.Model;
using QuanLyKhachSan.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyKhachSan.Areas.Admin.Controllers
{
    public class LoaiPhongController : BaseController
    {
        private QuanLyKhachSanEntities db = new QuanLyKhachSanEntities();

        // GET: Admin/LoaiPhong
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetDetailTypeRoom(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var typeRoom = db.tblLoaiPhongs.Find(id);
                TypeRoomViewModel typeRoomViewModel = new TypeRoomViewModel();
                typeRoomViewModel.ID = typeRoom.loai_phong;
                typeRoomViewModel.Image = typeRoom.anh;
                typeRoomViewModel.Percent = typeRoom.ti_le_phu_thu;
                typeRoomViewModel.Price = typeRoom.gia;
                typeRoomViewModel.TypeRoom = typeRoom.mo_ta;
                
                return Json(new
                {
                    data = typeRoomViewModel,
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                return Json(new { status = false, message = error.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeleteTypeRoom(int id)
        {
            //db.Configuration.ProxyCreationEnabled = false;
            var service = db.tblLoaiPhongs.Find(id);
            service.trang_thai = false;

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
        public JsonResult LoadDataTypeRoom(string name, int page, int pageSize = 5)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                //var model = db.tblDichVus.Where(x=>x.da_duoc_xoa == false).OrderBy(x=>x.ten_dv).Skip((page - 1) * pageSize).Take(pageSize);
                IQueryable<tblLoaiPhong> model = db.tblLoaiPhongs;

                if (!string.IsNullOrEmpty(name))
                {
                    model = model.Where(x => x.mo_ta.Contains(name));
                }
              
                int totalRow = model.Count();

                model = model.Where(x=>x.trang_thai == true).OrderBy(x=>x.loai_phong).Skip((page - 1) * pageSize).Take(pageSize);

                return Json(new
                {
                    data = model.Select(x => new
                    {
                        Image = x.anh,
                        Type = x.mo_ta,
                        Price = x.gia,
                        PhuThu = x.ti_le_phu_thu,                       
                        ID = x.loai_phong
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

                    file.SaveAs(Server.MapPath("/Content/Images/Phong/" + file.FileName));

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
        public JsonResult SaveDataTypeRoomNotImage(TypeRoomViewModel roomType)
        {
            bool status = false;
            string message = string.Empty;

            if (roomType.ID == 0)
            {
                tblLoaiPhong loaiPhong = new tblLoaiPhong();
                loaiPhong.loai_phong = roomType.ID;
                loaiPhong.gia = roomType.Price;
                loaiPhong.mo_ta = roomType.TypeRoom;
                loaiPhong.ti_le_phu_thu = roomType.Percent;
                loaiPhong.anh = null;

                db.tblLoaiPhongs.Add(loaiPhong);

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
                var entity = db.tblLoaiPhongs.Find(roomType.ID);

                entity.mo_ta = roomType.TypeRoom;
                entity.gia = roomType.Price;
                entity.ti_le_phu_thu = roomType.Percent;
                
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
        public JsonResult UpdatePrice(int ID, float Price)
        {
            try
            {
                var model = db.tblLoaiPhongs.Find(ID);
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
        public JsonResult UpdatePercent(int ID, int Percent)
        {
            try
            {
                var model = db.tblLoaiPhongs.Find(ID);
                model.ti_le_phu_thu = Percent;
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
        public JsonResult SaveDataTypeRoom(TypeRoomViewModel roomType)
        {
            bool status = false;
            string message = string.Empty;

            tblLoaiPhong loaiPhong = new tblLoaiPhong();
            loaiPhong.loai_phong = roomType.ID;
            loaiPhong.gia = roomType.Price;
            loaiPhong.ti_le_phu_thu = roomType.Percent;
            loaiPhong.mo_ta = roomType.TypeRoom;
            loaiPhong.anh = roomType.Image;
            
            if (roomType.ID == 0)
            {
                //Thêm mới
                db.tblLoaiPhongs.Add(loaiPhong);
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
                var entity = db.tblLoaiPhongs.Find(roomType.ID);
                entity.mo_ta = roomType.TypeRoom;
                entity.gia = roomType.Price;
                entity.ti_le_phu_thu = roomType.Percent;
                //entity.trang_thai = service.Status;
                entity.anh = roomType.Image;

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
    }
}