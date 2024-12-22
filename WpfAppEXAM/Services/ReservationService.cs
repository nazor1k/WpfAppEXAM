using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppEXAM.Models;
using WpfAppEXAM.Repository;

namespace WpfAppEXAM.Services
{
    public class ReservationService
    {
        private readonly IRepository<User> userRepository;
        private readonly IRepository<Table> tableRepository;
        private readonly IRepository<Reservation> reservationRepository;
        private readonly LoggerService loggerService;

        public ReservationService(
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


        public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
        {
            return await (await reservationRepository.GetAllAsync()).ToListAsync();
        }

        public async Task<Reservation?> GetReservationByIdAsync(int id)
        {
            return await (await reservationRepository.GetByConditionAsync(x => x.Id == id)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(int userId)
        {
            return await reservationRepository.GetByConditionAsync(r => r.UserId == userId);
        }

        

        public async Task UpdateReservationAsync(Reservation reservation, int id)
        {
            var existingReservation = await (await reservationRepository.GetByConditionAsync(x => x.Id == id)).FirstOrDefaultAsync();
            if (existingReservation == null)
            {
                loggerService.LogError("Reservation not found!");
                return;
            }

            await reservationRepository.UpdateAsync(reservation);
            await reservationRepository.SaveChangesAsync();
        }

        



        public async Task<List<Reservation>> GetReservationsForTableAsync(int tableId)
        {
            return (await reservationRepository.GetAllAsync())
                .Where(r => r.TableId == tableId)
                .ToList();
        }

        public async Task<List<Reservation>> GetReservationsForUserAsync(int userId)
        {
            return (await reservationRepository.GetAllAsync())
                .Where(r => r.UserId == userId)
                .ToList();
        }

        public async Task CreateReservationAsync(Reservation reservation)
        {
            await reservationRepository.AddAsync(reservation);
            await reservationRepository.SaveChangesAsync();
        }

        public async Task DeleteReservationAsync(int reservationId)
        {
            var reservation = await GetReservationByIdAsync(reservationId);
            if (reservation != null)
            {
                await reservationRepository.DeleteAsync(reservation);
                await reservationRepository.SaveChangesAsync();
            }
        }



    }
}
