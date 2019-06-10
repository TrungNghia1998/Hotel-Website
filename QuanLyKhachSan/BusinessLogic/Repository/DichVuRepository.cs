using DataProvider.Model;
using DataProvider.Repository;

namespace BusinessLogic.Repository
{
    public interface IDichVuRepository : IRepository<tblDichVu>
    {
    }

    public class DichVuRepository : BaseRepository<tblDichVu>, IDichVuRepository
    {
    }
}