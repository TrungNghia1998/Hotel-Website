using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyKhachSan.Areas.Admin.Models
{
    public class TaiKhoanChucVu
    {
        public string Username { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int ID { get; set; }
        public string DateString { get; set; }
    }
}