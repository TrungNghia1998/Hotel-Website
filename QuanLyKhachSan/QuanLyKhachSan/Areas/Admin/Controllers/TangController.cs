using DataProvider.Model;
using QuanLyKhachSan.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyKhachSan.Areas.Admin.Controllers
{
    public class TangController : BaseController
    {
        private QuanLyKhachSanEntities db = new QuanLyKhachSanEntities();
        // GET: Admin/Tang
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveDataLevel(LevelRoomViewModel model)
        {
            bool status = false;
            string message = string.Empty;
            
            tblTang tang = new tblTang();
            tang.ma_tang = model.ID;
            tang.ten_tang = model.Name;        

            if (model.ID == 0)
            {
                db.tblTangs.Add(tang);
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


                var entity = db.tblTangs.Find(model.ID);
                entity.ten_tang = model.Name;
               
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
        public JsonResult DeleteLevel (int id)
        {
            //db.Configuration.ProxyCreationEnabled = false;
            var level = db.tblTangs.Find(id);
            db.tblTangs.Remove(level);

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
        public JsonResult GetDetailLevel(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;

                var level = db.tblTangs.Find(id);
               
                LevelRoomViewModel roomViewModel = new LevelRoomViewModel();
                roomViewModel.ID = level.ma_tang;
                roomViewModel.Name = level.ten_tang;
             
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

        [HttpGet]
        public JsonResult LoadDataLevel(string name, int page, int pageSize = 5)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;

                IQueryable<tblTang> model = db.tblTangs;

                if (!string.IsNullOrEmpty(name))
                {
                    model = model.Where(x => x.ten_tang.Contains(name));
                }

                int totalRow = model.Count();

                model = model.OrderBy(x => x.ma_tang).Skip((page - 1) * pageSize).Take(pageSize);

                return Json(new
                {
                    data = model.Select(x => new
                    {                     
                        Name = x.ten_tang,                  
                        ID = x.ma_tang
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