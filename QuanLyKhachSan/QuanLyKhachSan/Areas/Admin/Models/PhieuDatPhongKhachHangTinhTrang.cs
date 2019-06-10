using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyKhachSan.Areas.Admin.Models
{
    public class PhieuDatPhongKhachHangTinhTrang
    {
        public int MaPhieuDatPhong { get; set; }
        public string TenKhachHang { get; set; }
        public DateTime? NgayDen { get; set; }
        public DateTime? NgayDi { get; set; }
        public DateTime? NgayDat { get; set; }
        public string TinhTrang { get; set; }
        public string NgayDenString { get; set; }
        public string NgayDiString { get; set; }
        public string NgayDatString { get; set; }
    }
}