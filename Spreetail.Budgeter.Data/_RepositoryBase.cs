using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Spreetail.Budgeter.Data
{
    public abstract class RepositoryBase<T> : IDisposable, Interface.IRepository<T> where T : class
    {
        private readonly IDbSet<T> DbSet;
        
        private BudgetContext DbContext;

        protected RepositoryBase()
        {
            DbContext = new BudgetContext();
            DbSet = DbContext.Set<T>();
        }

        public virtual void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            DbSet.Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            DbSet.Attach(entity);
            DbSet.Remove(entity);
        }

        public virtual T GetById(int id)
        {
            return DbSet.Find(id);
        }
            
        public virtual List<T> GetMany(Func<T, bool> expr)
        {
            return DbSet.Where(expr).ToList();
        }

        public virtual List<T> GetAll()
        {
            return DbSet.ToList();
        }


        public T GetOne(Func<T, bool> expr)
        {
            return DbSet.FirstOrDefault<T>(expr);
        }

        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }

        #region IDisposable
        
        // Flag: Has Dispose already been called?
        private bool disposed = false;

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                DbContext.Dispose();
            }

            disposed = true;
        }

        ~RepositoryBase()
        {
            Dispose(false);
        }

        #endregion


    }
}
