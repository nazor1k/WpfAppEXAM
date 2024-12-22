using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using WpfAppEXAM.Context;
using WpfAppEXAM.Models;
using WpfAppEXAM.Repository;
using WpfAppEXAM.Services;

namespace WpfAppEXAM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var serviceProvider = new ServiceCollection();
            ConfigurationServices(serviceProvider);
            ServiceProvider=serviceProvider.BuildServiceProvider();

            var loginWindow = ServiceProvider.GetService<MainWindow>();
            loginWindow.Show();

        }

        private void ConfigurationServices(ServiceCollection services)
        {
            services.AddTransient<LoggerService>();
            services.AddTransient<XorChipperService>();

            services.AddTransient<MainWindow>();
            services.AddTransient<RegistrationWindow>();
            services.AddTransient<AdminMenuWindow>();
            services.AddTransient<AdminReservationsWindow>();
            services.AddTransient<ClientTableBookerWindow>();
            services.AddTransient<UserEditReservationWindow>();
            services.AddTransient<EditReservationWindow>();

            services.AddTransient<IRepository<Reservation>,ReservationRepository>();
            services.AddTransient<IRepository<User>, UserRepository>();
            services.AddTransient<IRepository<Table>, TableRepository>();

            services.AddTransient<UserService>();
            services.AddTransient<TableService>();
            services.AddTransient<ReservationService>();

            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Lesson_Exam_Entity;Integrated Security=True;");
            });
        }
    }

}
