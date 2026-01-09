using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //T - Category or any other model

        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperaties = null);
        //the parameter is getting a linq exp of type T "which means that it's generic",
        //and the out result will be bool.
        T Get(Expression<Func<T,bool>> filter, string? includeProperaties = null, bool tracked = false); //get by id
        void Add(T entity);
        //void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);


    }
}
