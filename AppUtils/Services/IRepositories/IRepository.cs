using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AppUtils.Services.IRepositories
{
    public interface IRepository <T> where T : class
    {

        IEnumerable<T> GetAll();

        T GetById(object pk);

        void Add(T t);

        void Update(T t);

        void Delete(T t);

        IEnumerable<T> GetByCondition(Expression<Func<T, bool>> condition);
        void Save();
    }
}
