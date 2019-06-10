using DataProvider.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyKhachSan.Areas.Admin.Models
{
    public class GoiDichVu
    {
        public IEnumerable<tblDichVu> DanhSachDichVu { get; set; }
        public IEnumerable<tblDichVuDaDat> DanhSachDichVuDaDat { get; set; }
    }
}