using BusinessLogic.Repository;
using QuanLyKhachSan.Areas.Admin.Models;
using QuanLyKhachSan.Common;
using System.Web.Mvc;

namespace QuanLyKhachSan.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        NhanVienRepository nhanVienRepository = new NhanVienRepository();
        // GET: Admin/Login
        public ActionResult Index()
        {
            if (Session[ThongSoCoDinh.USERSESSION] != null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public ActionResult Login(TaiKhoanDangNhap model)
        {
            if (ModelState.IsValid)
            {
                var result = nhanVienRepository.KiemTraDangNhap(model.TenTaiKhoan, model.MatKhau);
                if (result == 1)
                {
                    var user = nhanVienRepository.LayTenTaiKhoan(model.TenTaiKhoan);
                    //ThongTinDangNhap loginInformation = new ThongTinDangNhap();
                    //loginInformation.MaTaiKhoan = user.ma_nv;
                    //loginInformation.TenTaiKhoan = user.tai_khoan;
                    Session[ThongSoCoDinh.USERSESSION] = user;
                    Session["NV"] = user.ma_nv;
                    Session["NhanVien"] = user;
                    return RedirectToAction("Index", "Home");
                }
                else if (result == 0) {
                    ModelState.AddModelError("", "Mật khẩu không đúng");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản hiện đang bị khóa");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập thất bại");
                }
            }
            return View("Index");
        }

        public ActionResult Logout()
        {
            Session[ThongSoCoDinh.USERSESSION] = null;
            return Redirect("/Admin/Login");
        }
    }
}