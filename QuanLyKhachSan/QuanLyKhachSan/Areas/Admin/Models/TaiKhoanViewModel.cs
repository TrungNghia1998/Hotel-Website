using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyKhachSan.Areas.Admin.Models
{
    public class TaiKhoanViewModel
    {
        public int ID { get; set; }
        public string HoTen { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public string TenTaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public string ChucVu { get; set; }
        public string Email { get; set; }
    }
}