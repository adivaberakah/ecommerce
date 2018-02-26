using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace eCommerce.Contracts.Repositories
{
   public interface IRepositoryBase <TEntity> where TEntity : class 
    {
        TEntity GetById(object id);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetPaged(int top = 20, int skip = 0, object orderBy = null, object filter = null);
        IQueryable<TEntity> GetAll(object filter);
        TEntity GetFullObject(object id);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Delete(object id);
        void Commit();
        void Dispose();
        void Insert(TEntity entity);

    }
}
