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
    public class ReservationRepository : IRepository<Reservation>
    {
        private readonly ApplicationContext _context;

        public ReservationRepository(ApplicationContext context) 
        {
            _context = context;
        }



        public async Task AddAsync(Reservation entity)
        {
            await _context.AddAsync(entity);
        }

        public async Task DeleteAsync(Reservation entity)
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

        public async Task<IQueryable<Reservation>> GetAllAsync()
        {
            return _context.Reservations;
        }

        public async Task<IQueryable<Reservation>> GetByConditionAsync(Expression<Func<Reservation, bool>> predicate)
        {
            return _context.Reservations.Where(predicate).AsQueryable();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Reservation entity)
        {
            _context.Update(entity);
        }
    }
}
