using System.Linq.Expressions;

namespace DataAccsess.Reposetory.IReposetory
{ 
    public interface IRepository <T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, object>>[]? includeprop= null, Expression<Func<T, bool>>? expression = null, bool tracking = true);
        T? GetOne(Expression<Func<T, object>>[]? includeprop = null,Expression < Func<T, bool>> expresion =null , bool tracking = true);
        void Add(T entity);
        void Edit(T entity);
        void Delete(T entity);
        void Save();
    }
}
