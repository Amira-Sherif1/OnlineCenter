using DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using DataAccsess.Reposetory.IReposetory;

namespace DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        ApplicationDbContext dbcontext;
        DbSet<T> set;

        public Repository(ApplicationDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
            set = dbcontext.Set<T>();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, object>>[]? includeprop = null, Expression<Func<T, bool>>? expression = null, bool tracking = true)
        {

            IQueryable<T> query = set;
            if (includeprop != null)
            {
                foreach (var prop in includeprop)
                {
                    query = query.Include(prop);

                }
            }
            if (expression != null)
            {
                query = query.Where(expression);
            }

            // Apply no-tracking if requested
            if (!tracking)
            {
                query = query.AsNoTracking();
            }
  

           return query.ToList();

        }
        public T? GetOne(Expression<Func<T, object>>[]? includeprop = null,Expression < Func<T, bool>> expresion=null , bool tracking = true)
        {
            return GetAll(includeprop,expresion,tracking).FirstOrDefault();

        }
        public void Add(T entity)
        {
            set.Add(entity);
        }
        public void Edit(T entity)
        {
            set.Update(entity);
        }
        public void Delete(T entity)
        {
            set.Remove(entity);
        }
        public void Save()
        {
            dbcontext.SaveChanges();
        }

    }
}
