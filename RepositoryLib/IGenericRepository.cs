using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLib
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        void Add(TEntity item);//Insert
        TEntity FindById(object id);//Select by id
        IEnumerable<TEntity> GetAll();//Select
        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate);//Select by expression

        void Remove(TEntity item);
        void Update(TEntity item);

    }
}
