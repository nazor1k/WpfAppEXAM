using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WpfAppEXAM.Context;
using WpfAppEXAM.Models;

namespace WpfAppEXAM.Repository
{
    public class UserRepository : IRepository<User>
    {
        private readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }


        public async Task AddAsync(User entity)
        {
            await _context.AddAsync(entity);
        }

        public async Task DeleteAsync(User entity)
        {
            _context.Remove(entity);
        }

        public async Task EnsureCreatedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
        }

        public async Task EnsureDeletedAsync()
        {
            await _context.Database.EnsureDeletedAsync();
        }

        public async Task<IQueryable<User>> GetAllAsync()
        {
            return  _context.Users;
        }

        public async Task<IQueryable<User>> GetByConditionAsync(Expression<Func<User, bool>> predicate)
        {
            return _context.Users.Where(predicate).AsQueryable();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User entity)
        {
            _context.Update(entity);
        }
    }
}
