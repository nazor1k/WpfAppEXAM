using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfAppEXAM.Models;
using WpfAppEXAM.Services;
using WpfAppEXAM.Services.AuthorizeHelperServices;

namespace WpfAppEXAM
{
    /// <summary>
    /// Interaction logic for UserEditReservationWindow.xaml
    /// </summary>
    public partial class UserEditReservationWindow : Window
    {
        private readonly ReservationService _reservationService;
        private readonly UserService _userService;
        private readonly LoggerService _loggerService;

        public UserEditReservationWindow(ReservationService reservationService, UserService userService, LoggerService loggerService)
        {
            InitializeComponent();
            _reservationService = reservationService;
            _userService = userService;
            _loggerService = loggerService;

            LoadUserReservations();
        }

        private async void LoadUserReservations()
        {
            try
            {
                var reservations = await _reservationService.GetReservationsForUserAsync(AuthorizeHelperService.User.Id);
                ReservationDataGrid.ItemsSource = reservations;
                
            }
            catch (Exception ex)
            {
                
                _loggerService.LogError($"Помилка завантаження бронювань: {ex.Message}");
            }
        }

        private async void DeleteReservationButton_Click(object sender, RoutedEventArgs e)
        {
            if (ReservationDataGrid.SelectedItem is Reservation selectedReservation)
            {
                try
                {
                    await _reservationService.DeleteReservationAsync(selectedReservation.Id);
                   
                    _loggerService.LogError("Бронювання скасовано успішно.");
                    LoadUserReservations();
                }
                catch (Exception ex)
                {
                    
                    _loggerService.LogError($"Помилка скасування бронювання: {ex.Message}");
                }
            }
            else
            {
                _loggerService.LogError("Виберіть бронювання для скасування.");
            }
        }

        private void EditReservationButton_Click(object sender, RoutedEventArgs e)
        {
            if (ReservationDataGrid.SelectedItem is Reservation selectedReservation)
            {
                AuthorizeHelperService.Reservation=selectedReservation;
                var editWindow = App.ServiceProvider.GetService<EditReservationWindow>();
                editWindow.ShowDialog();
                LoadUserReservations();
            }
            else
            {
                _loggerService.LogError("Виберіть бронювання для редагування.");
            }
        }
    }
}
