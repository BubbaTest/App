using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Gaia.BLL.Interface
{
    public interface IGenericRepository<TEntity>
    {
        TEntity SelectByID(object id);
        IQueryable<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> SelectAll();
        IQueryable<TEntity> ExecStoreProcedure(string sql, object[] parameters);
        void Update(TEntity entity);
        void Update(TEntity entity, params Expression<Func<TEntity, object>>[] modifiedProperties);
        void Insert(TEntity entity);
        void Insert(ICollection<TEntity> entity);
        void Delete(TEntity entity);
        void Save();
        void Dispose();
    }
}
