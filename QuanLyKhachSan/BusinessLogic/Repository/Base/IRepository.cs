﻿using System.Linq;

namespace DataProvider.Repository
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);

        void Create(T entity);

        void Delete(T entity);

        void Update(T entity);

        IQueryable<T> GetAll(string[] includes = null);
    }
}