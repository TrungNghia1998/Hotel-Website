using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyKhachSan.Areas.Admin.Models
{
    public class ThongTinChiTietDonDatPhong
    {
        public int MaPhieuDatPhong { get; set; }
        public int MaPhong { get; set; }
        public DateTime? NgayDen { get; set; }
        public string NgayDenFormat { get; set; }
        public DateTime? NgayDat { get; set; }
        public string NgayDatFormat { get; set; }
        public DateTime? NgayDi { get; set; }
        public string NgayDiFormat { get; set; }
        public string TenPhong { get; set; }
        public string LoaiPhong { get; set; }
        public double? GiaPhong { get; set; }
        public string TenKhachHang { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string NgaySinhNotTime { get; set; }
        public string GioiTinh { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string CMND { get; set; }
    }
}