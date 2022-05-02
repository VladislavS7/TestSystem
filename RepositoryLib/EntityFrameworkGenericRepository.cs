using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLib
{
    public class EntityFrameworkGenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        DbContext context;
        DbSet<TEntity> dbSet;
        public EntityFrameworkGenericRepository(DbContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }

        public void Add(TEntity item)
        {
            dbSet.Add(item);
            context.SaveChanges();
        }

        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return dbSet.Where(predicate).ToList();
        }

        public TEntity FindById(object id)
        {
            return dbSet.Find(id);//Знайти елемент по ключу
        }

        public IEnumerable<TEntity> GetAll()
        {
            return dbSet.ToList();// select * from table
        }

        public void Remove(TEntity item)
        {
            dbSet.Remove(item);
            context.SaveChanges();
        }

        //Передається об'єкт в цьому методі об'єкт перезаписується(модифікується)
        public void Update(TEntity item)
        {
            context.Entry(item).State = EntityState.Modified;//Переведення стану об'єкта в стан модифікації
            context.SaveChanges();
        }
    }
}
