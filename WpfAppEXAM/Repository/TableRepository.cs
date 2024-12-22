﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WpfAppEXAM.Context;
using WpfAppEXAM.Models;

namespace WpfAppEXAM.Repository
{
    public class TableRepository : IRepository<Table>
    {
        private readonly ApplicationContext _context;

        public TableRepository(ApplicationContext context)
        {
            _context = context;
        }


        public async Task AddAsync(Table entity)
        {
            await _context.AddAsync(entity);
        }

        public async Task DeleteAsync(Table entity)
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

        public async Task<IQueryable<Table>> GetAllAsync()
        {
            return _context.Tables;
        }

        public async Task<IQueryable<Table>> GetByConditionAsync(Expression<Func<Table, bool>> predicate)
        {
            return _context.Tables.Where(predicate).AsQueryable();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Table entity)
        {
            _context.Update(entity);
        }
    }
}