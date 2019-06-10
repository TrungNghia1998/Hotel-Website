using DataProvider.Model;
using System;

namespace BusinessLogic.Repository.Base
{
    public class UnitOfWork : IDisposable
    {
        private QuanLyKhachSanEntities _dbContext = new QuanLyKhachSanEntities();

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}