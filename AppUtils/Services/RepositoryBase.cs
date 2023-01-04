using AppUtils.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AppUtils.Services
{
    public abstract class RepositoryBase <T> where T : class
    {
        protected DbAppUtils _db { get; set; }
        protected DbSet<T> _dbComponent { get; set; }
        public RepositoryBase(DbAppUtils db)
        {
            this._db = db;
            this._dbComponent = _db.Set<T>();
        }
        public void Add(T t)
        {
            _dbComponent.Add(t);
        }

        public void Delete(T t)
        {
                _dbComponent.Remove(t);
        }

        
        public IEnumerable<T> GetAll()
        {
            return _dbComponent.ToList();
        }

        public T GetById(object attr)
        {
            T t = _dbComponent.Find(attr);
            return t;
        }

        public void Update(T t)
        {
                _dbComponent.Attach(t);
                _db.Entry(t).State = EntityState.Modified;
        }

        public IEnumerable<T> GetByCondition(Expression<Func<T, bool>> condition)
        {
            List<T> ts = _dbComponent.Where(condition).AsEnumerable().ToList();
            return ts;
        }

        public void Save()
        {
            _db.SaveChanges();
        }
      
    }
}
