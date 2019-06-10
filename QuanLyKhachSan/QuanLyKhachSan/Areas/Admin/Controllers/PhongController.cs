using BusinessLogic.ViewModel;
using DataProvider.Model;
using QuanLyKhachSan.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyKhachSan.Areas.Admin.Controllers
{
    public class PhongController : BaseController
    {
        private QuanLyKhachSanEntities db = new QuanLyKhachSanEntities();

        // GET: Admin/Phong
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetTypeAndLevelRoom()
        {
            bool status = false;
            string message = string.Empty;

            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                
                var levelRoom = db.tblTangs.ToList();
                var typeRoom = db.tblLoaiPhongs.ToList();
                status = true;

                return Json(new
                {                  
                    level = levelRoom.Select(x => new
                    {
                        ID = x.ma_tang,
                        Name = x.ten_tang
                    }),
                    type = typeRoom.Select(x => new
                    {
                        ID = x.loai_phong,
                        Name = x.mo_ta
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
        public JsonResult LoadDataRoom(string name, int page, int pageSize = 5)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                //var model = db.tblDichVus.Where(x=>x.da_duoc_xoa == false).OrderBy(x=>x.ten_dv).Skip((page - 1) * pageSize).Take(pageSize);
                IQueryable<PhongLoaiPhongTangModelView> model = from phong in db.tblPhongs
                                                                join loaiphong in db.tblLoaiPhongs on phong.loai_phong equals loaiphong.loai_phong
                                                                join tang in db.tblTangs on phong.ma_tang equals tang.ma_tang
                                                                where phong.ma_tinh_trang < 5
                                                                select new PhongLoaiPhongTangModelView
                                                                {
                                                                    Image = loaiphong.anh,
                                                                    Name = phong.so_phong,
                                                                    Type = loaiphong.mo_ta,
                                                                    Level = tang.ten_tang,
                                                                    ID = phong.ma_phong
                                                                };

                if (!string.IsNullOrEmpty(name))
                {
                    //model = model.Where(x=>(x.Name.Contains(name)) || x.Type.Contains(name) || x.ID.Equals(name) || x.Level.Contains(name));
                    model = model.Where(x => (x.Name.Contains(name)));
                }
                
                int totalRow = model.Count();

                model = model.OrderBy(x=>x.ID).Skip((page - 1) * pageSize).Take(pageSize);

                return Json(new
                {
                    data = model.Select(x => new
                    {
                        Image = x.Image,
                        Name = x.Name,
                        Type = x.Type,
                        Level = x.Level,                       
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

        [HttpPost]
        public JsonResult SaveDataRoom(LevelRoomViewModel model)
        {
            bool status = false;
            string message = string.Empty;  

            tblLoaiPhong modelLoaiPhong = db.tblLoaiPhongs.Where(x => x.mo_ta == model.Type).SingleOrDefault();
            tblTang modelTang = db.tblTangs.Where(x => x.ten_tang == model.Level).SingleOrDefault();

            tblPhong phong = new tblPhong();
            phong.ma_phong = model.ID;
            phong.so_phong = model.Name;
            phong.loai_phong = modelLoaiPhong.loai_phong;
            phong.ma_tang = modelTang.ma_tang;
            phong.ma_tinh_trang = 1;

            if(model.ID == 0)
            {
                db.tblPhongs.Add(phong);
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


                var entity = db.tblPhongs.Find(model.ID);
                entity.so_phong = model.Name;
                entity.ma_tang = modelTang.ma_tang;
                entity.loai_phong = modelLoaiPhong.loai_phong;

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

        [HttpGet]
        public JsonResult GetDetailRoom(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;

                var room = db.tblPhongs.Find(id);

                tblLoaiPhong modelLoaiPhong = db.tblLoaiPhongs.Where(x => x.loai_phong == room.loai_phong).SingleOrDefault();
                tblTang modelTang = db.tblTangs.Where(x => x.ma_tang == room.ma_tang).SingleOrDefault();

                LevelRoomViewModel roomViewModel = new LevelRoomViewModel();
                roomViewModel.ID = room.ma_phong;
                roomViewModel.Name = room.so_phong;
                roomViewModel.Level = modelTang.ten_tang;
                roomViewModel.Type = modelLoaiPhong.mo_ta;
               
                return Json(new
                {
                    data = roomViewModel,
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                return Json(new { status = false, message = error.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeleteRoom(int id)
        {
            //db.Configuration.ProxyCreationEnabled = false;
            var room = db.tblPhongs.Find(id);
            room.ma_tinh_trang = 5;

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
    }
}