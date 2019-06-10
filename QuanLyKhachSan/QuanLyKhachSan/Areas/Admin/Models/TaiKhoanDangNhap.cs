using System;
using System.ComponentModel.DataAnnotations;

namespace QuanLyKhachSan.Areas.Admin.Models
{
    [Serializable]
    public class TaiKhoanDangNhap
    {
        [Required(ErrorMessage = "Bạn chưa nhập tài khoản ")]
        public string TenTaiKhoan { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu ")]
        public string MatKhau { get; set; }

        public bool RememberMe { get; set; }
    }
}