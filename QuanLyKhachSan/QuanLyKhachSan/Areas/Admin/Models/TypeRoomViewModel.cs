using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyKhachSan.Areas.Admin.Models
{
    public class TypeRoomViewModel
    {
        public int ID { get; set; }
        public string TypeRoom { get; set; }
        public double? Price { get; set; }
        public int? Percent { get; set; }
        public string Image { get; set; }
    }
}