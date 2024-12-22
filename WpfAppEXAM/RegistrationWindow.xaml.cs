using System.Windows;
using WpfAppEXAM.Models;
using WpfAppEXAM.Services;

namespace WpfAppEXAM
{
    /// <summary>
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        private readonly UserService userService;
        private readonly TableService tableService;
        private readonly ReservationService reservationService;
        private readonly LoggerService loggerService;
        public RegistrationWindow(UserService userService, TableService tableService, ReservationService reservationService, LoggerService loggerService)
        {
            this.userService = userService;
            this.tableService = tableService;
            this.reservationService = reservationService;
            this.loggerService = loggerService;
            InitializeComponent();
        }


        private void ClearForm()
        {
            Name.Text = string.Empty;
            Login.Text = string.Empty;
            Password.Password = string.Empty;
            Password_Repeat.Password = string.Empty;
        }

        private async void RegistrationBut_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Name.Text) ||
                string.IsNullOrWhiteSpace(Login.Text) ||
                string.IsNullOrWhiteSpace(Password.Password) ||
                string.IsNullOrWhiteSpace(Password_Repeat.Password))
            {
                loggerService.LogWarning("Write all lines!!!");


                return;
            }
            if (Password.Password != Password_Repeat.Password)
            {
                loggerService.LogWarning("Passwords is not equals!!!");
                return;
            }
            try
            {
                var UserRole = Role.Client;
                if (IsAdminCheckBox.IsChecked != null && IsAdminCheckBox.IsChecked == true) UserRole = Role.Admin;

                var IsCreated = false;

                IsCreated = await userService.AddUserAsync(new User()
                {
                    Login = Login.Text,
                    FullName = Name.Text,
                    PasswordHash = Password.Password,
                    Role = UserRole
                });
                if (!IsCreated)
                {
                    ClearForm();
                    return;
                }



            }
            catch
            {
                loggerService.LogError("Problem with adding User!");
                return;
            }

            ClearForm();
            loggerService.LogInfo("Succsesfull registration!");
            Close();



        }
    }
}
