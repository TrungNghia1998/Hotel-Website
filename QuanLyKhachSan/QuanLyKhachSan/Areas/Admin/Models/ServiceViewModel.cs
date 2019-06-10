using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyKhachSan.Areas.Admin.Models
{
    public class ServiceViewModel
    {
        public string Name { get; set; }
        public double? Price { get; set; }
        public string Type { get; set; }
        public int? Quantity { get; set; }
        public string Image { get; set; }
        public bool? Status { get; set; }
        public int ID { get; set; }
    }
}