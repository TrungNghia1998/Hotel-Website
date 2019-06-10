using DataProvider.Model;
using DataProvider.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public interface INhanVienRepository : IRepository<tblNhanVien>
    {
        int KiemTraDangNhap(string userName, string passWord);
        tblNhanVien LayTenTaiKhoan(string username);
    }

    public class NhanVienRepository : BaseRepository<tblNhanVien>, INhanVienRepository
    {
        public int KiemTraDangNhap(string userName, string passWord)
        {
            var user = _dbContext.tblNhanViens.FirstOrDefault(u => u.tai_khoan == userName);

            if (user != null)
            {
                if (user.mat_khau == passWord)
                {
                    if (user.trang_thai_tai_khoan == true)
                    {
                        return 1;// Đăng nhập thành công
                    }
                    else
                    {
                        return -1;//Tài khoả đã bị khoá
                    }
                }
                else
                {
                    return 0;//Sai mật khẩu
                }
            }
            else
            {
                return -2;//Tài khoản không tồn tại
            }
        }

        public tblNhanVien LayTenTaiKhoan(string username)
        {
            var user = _dbContext.tblNhanViens.FirstOrDefault(u => u.tai_khoan == username);
            return user;
        }
    }
}
