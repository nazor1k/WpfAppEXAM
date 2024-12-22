using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAppEXAM.Models;
using WpfAppEXAM.Services;
using WpfAppEXAM.Services.AuthorizeHelperServices;

namespace WpfAppEXAM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly UserService userService;
        private readonly LoggerService logger;

        public MainWindow(UserService _userService,LoggerService _logger)
        {
            userService=_userService;
            logger = _logger;
            
            InitializeComponent();


        }

        private async Task AutoAuthForDevTests(string login, string password) {
            AuthorizeHelperService.User = await userService.LoginUserAsync(login, password);
            if (AuthorizeHelperService.User == null)
            {
                logger.LogError("User is not found!!!");
                ClearForm();
                return;
            }
            var wind = await userService.GetWindowByUserIdAsync(AuthorizeHelperService.User.Id);
            if (wind != null)
            {
                wind.ShowDialog();
            }
        }

        private void ClearForm()
        {
            Name.Text = string.Empty;
            Password.Password = string.Empty;

        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            var wind = App.ServiceProvider.GetService<RegistrationWindow>();
            wind.ShowDialog();
            

        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            ////
            //string loginAdmin = "naz";
            //string passAdmin = "qaz";

            //string loginClient = "na";
            //string passClient = "qaz";

            ////await AutoAuthForDevTests(loginAdmin,passAdmin);
            //await AutoAuthForDevTests(loginClient, passClient);
            //return;
            ////


            if (string.IsNullOrWhiteSpace(Name.Text) && string.IsNullOrWhiteSpace(Password.Password))
            {
                logger.LogWarning("Write all lines!");
                return;
            }
            AuthorizeHelperService.User = await userService.LoginUserAsync(Name.Text, Password.Password);
            if (AuthorizeHelperService.User == null)
            {
                logger.LogError("User is not found!!!");
                ClearForm();
                return;
            }



            var wind =await userService.GetWindowByUserIdAsync(AuthorizeHelperService.User.Id);
            if (wind != null) {
                wind.ShowDialog();
            }
            
            


            


        }

    }
}