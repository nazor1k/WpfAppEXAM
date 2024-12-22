using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppEXAM.Models;
using WpfAppEXAM.Repository;

namespace WpfAppEXAM.Services
{
    public class TableService
    {
        private readonly IRepository<User> userRepository;
        private readonly IRepository<Table> tableRepository;
        private readonly IRepository<Reservation> reservationRepository;
        private readonly LoggerService loggerService;

        public TableService(
            IRepository<User> userRepository,
            IRepository<Table> tableRepository,
            IRepository<Reservation> reservationRepository,
            LoggerService loggerService
            )
        {
            this.loggerService = loggerService;
            this.userRepository = userRepository;
            this.tableRepository = tableRepository;
            this.reservationRepository = reservationRepository;
           
        }


        public async Task<List<Table>> GetAllTablesAsync()
        {
            return await(await tableRepository.GetAllAsync()).ToListAsync();
        }

        public async Task<Table?> GetTableByIdAsync(int id)
        {
            return await (await tableRepository.GetByConditionAsync(x => x.Id == id)).FirstOrDefaultAsync();
        }

        public async Task AddTableAsync(Table table)
        {
            await tableRepository.AddAsync(table);
            await tableRepository.SaveChangesAsync();
        }

        public async Task UpdateTableAsync(Table table, int id)
        {
            var existingTable =await GetTableByIdAsync(id);
            if (existingTable == null)
            {
                loggerService.LogError("Table not found!");
                return;
            }

            await tableRepository.UpdateAsync(table);
            await tableRepository.SaveChangesAsync();
        }

        public async Task DeleteTableAsync(int id)
        {
            var tableToDelete = await GetTableByIdAsync(id);
            if (tableToDelete == null)
            {
                loggerService.LogError("Table not found!");
                return;
            }

            await tableRepository.DeleteAsync(tableToDelete);
            await tableRepository.SaveChangesAsync();
        }





    }
}
