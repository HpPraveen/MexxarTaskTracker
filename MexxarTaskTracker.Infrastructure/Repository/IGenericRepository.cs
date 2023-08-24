using System.Linq.Expressions;

namespace Invoicing.Infrastructure.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        List<T> Get(Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "",
            Expression<Func<T, bool>>[]? filters = null);

        T GetById(object id);

        void Insert(T entity);

        void Update(T entityToUpdate);

        void Delete(object id);

        void Delete(T entityToDelete);
    }
}
