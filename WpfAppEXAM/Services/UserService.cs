using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WpfAppEXAM.Models;
using WpfAppEXAM.Repository;

namespace WpfAppEXAM.Services
{
    public class UserService
    {
        private readonly IRepository<User> userRepository;
        private readonly IRepository<Table> tableRepository;
        private readonly IRepository<Reservation> reservationRepository;
        private readonly XorChipperService xorChipperService;
        private readonly LoggerService loggerService;

        public UserService(
            IRepository<User> userRepository,
            IRepository<Table> tableRepository,
            IRepository<Reservation> reservationRepository,
            XorChipperService xorChipperService,
            LoggerService loggerService
            )
        {
            this.userRepository = userRepository;
            this.tableRepository = tableRepository;
            this.reservationRepository = reservationRepository;
            this.xorChipperService = xorChipperService;
            this.loggerService = loggerService;
        }

        public async Task<bool> AddUserAsync(User user)
        {
            if (user == null)
            {
                loggerService.LogError("User is null!");
                return false;
            }
            if (await (await userRepository.GetByConditionAsync(x => x.Login == user.Login)).FirstOrDefaultAsync() == null)
            {
                user.PasswordHash = xorChipperService.Encrypt(user.PasswordHash);
                await userRepository.AddAsync(user);
                await userRepository.SaveChangesAsync();
                return true;
            }
            else loggerService.LogError($"User with login {user.Login} already Exist");
            return false;

        }

        //public async Task DeleteUserAsync(User user)
        //{
        //    if (user == null)
        //    {
        //        loggerService.LogError("User is null!");
        //        return;
        //    }
        //    await userRepository.DeleteAsync(user);
        //}

        public async Task<User?> LoginUserAsync(string login, string password)
        {
            var hashPass = xorChipperService.Encrypt(password);
            return await (await userRepository.GetByConditionAsync(x => x.Login == login && x.PasswordHash == hashPass)).FirstOrDefaultAsync();
        }

        public async Task<Window?> GetWindowByUserIdAsync(int userId)
        {
            var user = await (await userRepository.GetByConditionAsync(x => x.Id == userId)).FirstOrDefaultAsync();
            if (user == null)
            {
                loggerService.LogError("User not found!");
                return null;
            }


            if (user.Role == Role.Admin) return App.ServiceProvider.GetService<AdminMenuWindow>();

            else if (user.Role == Role.Client) return App.ServiceProvider.GetService<ClientTableBookerWindow>();




            loggerService.LogError("Inncorrect users data!!!");
            return null;
        }

    }
}
