using DataProvider.Model;
using DataProvider.Repository;

namespace BusinessLogic.Repository
{
    public interface IHoaDonRepository : IRepository<tblHoaDon>
    {
    }

    public class HoaDonRepository : BaseRepository<tblHoaDon>, IHoaDonRepository
    {
    }
}