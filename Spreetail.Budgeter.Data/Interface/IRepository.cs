using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Spreetail.Budgeter.Data.Interface
{
    public interface IRepository<T> where T : class
    {        
        void Add(T entity);
        
        void Update(T entity);
        
        void Delete(T entity);

        T GetById(int id);

        T GetOne(Func<T, bool> expr);

        List<T> GetMany(Func<T, bool> expr);

        List<T> GetAll();

        void SaveChanges();
    }
}
