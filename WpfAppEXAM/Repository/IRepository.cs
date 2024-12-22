using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppEXAM.Repository
{
    public interface IRepository<T> where T : class
    {

        Task AddAsync(T entity);

        Task<IQueryable<T>> GetAllAsync();
        Task<IQueryable<T>> GetByConditionAsync(Expression<Func<T, bool>> predicate);

        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

        Task<int> SaveChangesAsync();
        Task EnsureCreatedAsync();
        Task EnsureDeletedAsync();




    }
}
